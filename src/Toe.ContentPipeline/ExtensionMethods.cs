using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public static class ExtensionMethods
    {
        public static IReadOnlyList<T> GetStreamReader<T>(this IBufferView bufferView, StreamKey streamKey)
        {
            var stream = bufferView.GetStream(streamKey);
            if (stream == null)
                return null;
            return stream.GetReader<T>();
        }

        public static IEnumerable<(int,int,int)> GetFaces(this IMeshPrimitive source, StreamKey key)
        {
            return source.GetIndexReader(key).GetFaces(source.Topology);
        }
        public static IEnumerable<(int, int, int)> GetFaces(this IEnumerable<int> source, PrimitiveTopology type)
        {
            using (var enumerator = source.GetEnumerator())
            {
                switch (type)
                {
                    case PrimitiveTopology.TriangleList:
                        for (; ; )
                        {
                            if (!enumerator.MoveNext())
                            {
                                yield break;
                            }
                            var a = enumerator.Current;
                            if (!enumerator.MoveNext())
                            {
                                yield break;
                            }
                            var b = enumerator.Current;
                            if (!enumerator.MoveNext())
                            {
                                yield break;
                            }
                            var c = enumerator.Current;
                            yield return (a, b, c);
                        }
                    case PrimitiveTopology.TriangleStrip:
                        {
                            if (!enumerator.MoveNext())
                            {
                                yield break;
                            }
                            var a = enumerator.Current;
                            if (!enumerator.MoveNext())
                            {
                                yield break;
                            }
                            var b = enumerator.Current;
                            if (!enumerator.MoveNext())
                            {
                                yield break;
                            }
                            var c = enumerator.Current;
                            for (; ; )
                            {
                                if (a != b && b != c && c != a)
                                {
                                    yield return (a, b, c);
                                }
                                if (!enumerator.MoveNext())
                                {
                                    yield break;
                                }
                                a = b;
                                b = c;
                                c = enumerator.Current;
                            }
                        }
                    case PrimitiveTopology.LineList:
                        yield break;
                    case PrimitiveTopology.LineStrip:
                        yield break;
                    case PrimitiveTopology.PointList:
                        yield break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }


        public static IShaderParameter<T> AsShaderParameter<T>(this T value, string key) where T : struct
        {
            return new ShaderParameter<T>(key, value);
        }

        public static IContentContainer Load(this IStreamReader reader, string fileName)
        {
            return LoadAsync(reader, fileName).Result;
        }

        public static async Task<IContentContainer> LoadAsync(this IStreamReader reader, string fileName)
        {
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return await reader.ReadAsync(stream);
            }
        }

        public static async Task SaveAsync(this IFileWriter writer, string fileName, IContentContainer container)
        {
            using (var stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                await writer.WriteAsync(stream, container);
            }
        }

        public static string EnsureUniqueKey<TValue>(this IDictionary<string, TValue> collection, string id)
        {
            return EnsureUniqueKey(id ?? typeof(TValue).Name, collection.ContainsKey);
        }

        public static string EnsureUniqueKey<TValue>(this IReadOnlyDictionary<string, TValue> collection, string id)
        {
            return EnsureUniqueKey(id ?? typeof(TValue).Name, collection.ContainsKey);
        }

        public static string EnsureUniqueKey(string id, Func<string, bool> containsKey)
        {
            var prefix = id;
            for (var i = 0;;)
            {
                if (!containsKey(id))
                    return id;
                ++i;
                id = prefix + i;
            }
        }

        public static IReadOnlyList<T> AddRange<T>(this IAssetContainer<T> container, IReadOnlyList<T> assets) where T : class, IAsset
        {
            return AddRange<T, T>(container, assets, _ => _.Id, (a,id) => a);
        }

        public static IReadOnlyList<T> AddRange<A, T>(this IAssetContainer<T> container, IReadOnlyList<A> assets,
            Func<A, string> getId, Func<A, string, T> transform) where T : class, IAsset
        {
            var res = new T[assets.Count];
            var visitedNames = new HashSet<string>();

            //Prefer original id where possible
            for (var index = 0; index < res.Length; ++index)
            {
                var asset = assets[index];
                var id = getId(asset);
                if (id != null)
                {
                    id = EnsureUniqueKey(id, _ => container.ContainsKey(_) || visitedNames.Contains(_));
                    visitedNames.Add(id);
                    var createdAsset = transform(asset, id);
                    if (createdAsset == null)
                        throw new InvalidOperationException($"Transform function returned null result for {id}");
                    res[index] = createdAsset;
                }
            }

            //Generate unique ids where no id was supplied
            var offset = container.Count;
            var lastKnownAvailableIndex = -1;
            for (var index = 0; index < res.Length; ++index)
            {
                if (res[index] != null)
                {
                    container.Add(res[index]);
                }
                else
                {
                    var nameIndex = index + offset;
                    if (nameIndex <= lastKnownAvailableIndex)
                        nameIndex = lastKnownAvailableIndex + 1;
                    var prefix = typeof(T).Name;
                    string id;
                    for (;; ++nameIndex)
                    {
                        id = prefix + nameIndex.ToString(CultureInfo.InvariantCulture);
                        if (!container.ContainsKey(nameIndex.ToString(CultureInfo.InvariantCulture)))
                            break;
                    }

                    lastKnownAvailableIndex = nameIndex;
                    var asset = transform(assets[index], id);
                    container.Add(asset);
                    res[index] = asset;
                }
            }

            return res;
        }

        public static GpuMesh ToGpuMesh(this IMesh source)
        {
            if (source is GpuMesh) return (GpuMesh) source;
            return GpuMesh.Optimize(source);
        }

        public static IndexedMesh ToIndexedMesh(this IMesh source)
        {
            if (source is IndexedMesh) return (IndexedMesh) source;
            return IndexedMesh.Optimize(source);
        }
    }
}