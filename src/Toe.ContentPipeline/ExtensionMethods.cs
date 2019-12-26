using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public static class ExtensionMethods
    {
        public static IContentContainer ToContentContainer(this ISceneAsset scene)
        {
            var content = new ContentContainer();
            content.Scenes.Add(scene);
            var visitedCameras = new HashSet<ICameraAsset>();
            var visitedLights = new HashSet<ILightAsset>();
            var visitedMeshes = new HashSet<IMesh>();
            var visitedMaterials = new HashSet<IMaterialAsset>();
            var visitedImages = new HashSet<IImageAsset>();
            foreach (var nodeAsset in scene.EnumerateNodesBreadthFirst())
            {
                content.Nodes.Add(nodeAsset);
                if (nodeAsset.Camera != null)
                    if (visitedCameras.Add(nodeAsset.Camera))
                        content.Cameras.Add(nodeAsset.Camera);
                if (nodeAsset.Light != null)
                    if (visitedLights.Add(nodeAsset.Light))
                        content.Lights.Add(nodeAsset.Light);
                if (nodeAsset.Mesh != null)
                {
                    if (nodeAsset.Mesh.Mesh != null)
                        if (visitedMeshes.Add(nodeAsset.Mesh.Mesh))
                            content.Meshes.Add(nodeAsset.Mesh.Mesh);

                    if (nodeAsset.Mesh.Materials != null)
                        foreach (var materialAsset in nodeAsset.Mesh.Materials)
                            if (visitedMaterials.Add(materialAsset))
                            {
                                content.Materials.Add(materialAsset);
                                if (materialAsset.Shader != null)
                                    foreach (var parameter in materialAsset.Shader.Parameters)
                                        if (parameter.ValueType == typeof(SamplerParameters))
                                        {
                                            var samper = parameter as IShaderParameter<SamplerParameters>;
                                            if (samper?.Value.Image != null)
                                                if (visitedImages.Add(samper.Value.Image))
                                                    content.Images.Add(samper.Value.Image);
                                        }
                            }
                }
            }

            return content;
        }

        /// <summary>
        ///     Enumerate child nodes "Breadth-First".
        /// </summary>
        /// <param name="root">Parent node.</param>
        /// <returns>Yielded nodes.</returns>
        public static IEnumerable<INodeAsset> EnumerateNodesBreadthFirst(this INodeContainer root)
        {
            if (root == null) yield break;
            var containers = new Stack<INodeAsset>(16);
            foreach (var n in root.ChildNodes) containers.Push(n);
            while (containers.Count > 0)
            {
                var node = containers.Pop();
                yield return node;
                foreach (var n in node.ChildNodes) containers.Push(n);
            }
        }

        /// <summary>
        ///     Enumerate child nodes "Depth-First".
        /// </summary>
        /// <param name="root">Parent node.</param>
        /// <returns>Yielded nodes.</returns>
        public static IEnumerable<INodeAsset> EnumerateNodesDepthFirst(this INodeContainer root)
        {
            if (root == null) yield break;
            var containers = new Queue<INodeAsset>(16);
            foreach (var n in root.ChildNodes) containers.Enqueue(n);
            while (containers.Count > 0)
            {
                var node = containers.Dequeue();
                yield return node;
                foreach (var n in node.ChildNodes) containers.Enqueue(n);
            }
        }

        public static void ForEachStream(this IBufferView bufferView, Action<StreamKey, int, IMeshStream> callback)
        {
            var i = 0;
            var streamKeys = bufferView.GetStreams();
            foreach (var stream in streamKeys)
            {
                callback(stream, i, bufferView.GetStream(stream));
                ++i;
            }
        }

        public static IReadOnlyList<T> ForEachStream<T>(this IBufferView bufferView,
            Func<StreamKey, int, IMeshStream, T> callback)
        {
            var streamKeys = bufferView.GetStreams();
            var res = new T[streamKeys.Count];
            var i = 0;
            foreach (var stream in streamKeys)
            {
                res[i] = callback(stream, i, bufferView.GetStream(stream));
                ++i;
            }

            return res;
        }

        public static IReadOnlyList<To> Project<TFrom, To>(this IReadOnlyCollection<TFrom> values,
            Func<TFrom, int, To> callback)
        {
            var res = new To[values.Count];
            var i = 0;
            foreach (var stream in values)
            {
                res[i] = callback(stream, i);
                ++i;
            }

            return res;
        }

        public static IReadOnlyList<T> GetStreamReader<T>(this IBufferView bufferView, StreamKey streamKey)
        {
            var stream = bufferView.GetStream(streamKey);
            if (stream == null)
                return null;
            return stream.GetReader<T>();
        }

        public static IEnumerable<(int, int, int)> GetFaces(this IMeshPrimitive source, StreamKey key)
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
                        for (;;)
                        {
                            if (!enumerator.MoveNext()) yield break;
                            var a = enumerator.Current;
                            if (!enumerator.MoveNext()) yield break;
                            var b = enumerator.Current;
                            if (!enumerator.MoveNext()) yield break;
                            var c = enumerator.Current;
                            yield return (a, b, c);
                        }
                    case PrimitiveTopology.TriangleStrip:
                    {
                        if (!enumerator.MoveNext()) yield break;
                        var a = enumerator.Current;
                        if (!enumerator.MoveNext()) yield break;
                        var b = enumerator.Current;
                        if (!enumerator.MoveNext()) yield break;
                        var c = enumerator.Current;
                        for (;;)
                        {
                            if (a != b && b != c && c != a) yield return (a, b, c);
                            if (!enumerator.MoveNext()) yield break;
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

        public static IReadOnlyList<T> AddRange<T>(this IAssetContainer<T> container, IReadOnlyList<T> assets)
            where T : class, IAsset
        {
            return AddRange(container, assets, _ => _.Id, (a, id) => a);
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

        public static IReadOnlyCollection<BufferViewAndPrimitiveIndices<IMeshPrimitive>> GroupPrimitives(
            this IMesh mesh)
        {
            var map = new Dictionary<IBufferView, BufferViewAndPrimitiveIndices<IMeshPrimitive>>(mesh.Primitives.Count);
            for (var index = 0; index < mesh.Primitives.Count; index++)
            {
                var meshPrimitive = mesh.Primitives[index];
                var bufferView = meshPrimitive.BufferView;
                if (!map.TryGetValue(bufferView, out var indices))
                    map.Add(bufferView,
                        indices = new BufferViewAndPrimitiveIndices<IMeshPrimitive> {BufferView = bufferView});

                indices.Primitives.Add(new PrimitiveAndIndex<IMeshPrimitive>(meshPrimitive, index));
            }

            return map.Values;
        }

        public static IReadOnlyCollection<BufferViewAndPrimitiveIndices<GpuPrimitive>> GroupPrimitives(
            this GpuMesh mesh)
        {
            var map = new Dictionary<IBufferView, BufferViewAndPrimitiveIndices<GpuPrimitive>>(mesh.Primitives.Count);
            for (var index = 0; index < mesh.Primitives.Count; index++)
            {
                var meshPrimitive = mesh.Primitives[index];
                var bufferView = meshPrimitive.BufferView;
                if (!map.TryGetValue(bufferView, out var indices))
                    map.Add(bufferView,
                        indices = new BufferViewAndPrimitiveIndices<GpuPrimitive> {BufferView = bufferView});

                indices.Primitives.Add(new PrimitiveAndIndex<GpuPrimitive>(meshPrimitive, index));
            }

            return map.Values;
        }

        public static IReadOnlyCollection<BufferViewAndPrimitiveIndices<IndexMeshPrimitive>> GroupPrimitives(
            this IndexedMesh mesh)
        {
            var map =
                new Dictionary<IBufferView, BufferViewAndPrimitiveIndices<IndexMeshPrimitive>>(mesh.Primitives.Count);
            for (var index = 0; index < mesh.Primitives.Count; index++)
            {
                var meshPrimitive = mesh.Primitives[index];
                var bufferView = meshPrimitive.BufferView;
                if (!map.TryGetValue(bufferView, out var indices))
                    map.Add(bufferView,
                        indices = new BufferViewAndPrimitiveIndices<IndexMeshPrimitive> {BufferView = bufferView});

                indices.Primitives.Add(new PrimitiveAndIndex<IndexMeshPrimitive>(meshPrimitive, index));
            }

            return map.Values;
        }
    }
}