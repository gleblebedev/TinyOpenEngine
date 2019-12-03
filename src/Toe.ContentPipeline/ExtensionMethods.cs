using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public static class ExtensionMethods
    {
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