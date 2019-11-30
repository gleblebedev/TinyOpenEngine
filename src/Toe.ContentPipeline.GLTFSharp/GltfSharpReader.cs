using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SharpGLTF.Schema2;
using Toe.ContentPipeline;

namespace Toe.ContentPipeline.GLTFSharp
{
    public class GltfSharpReader : IStreamReader
    {
        private readonly ReadSettings _readSettings;

        public GltfSharpReader(AssetReader assetReader = null, bool skipValidation = true)
        {
            _readSettings = new ReadSettings
            {
                FileReader = assetReader,
                SkipValidation = skipValidation
            };
        }

        public Task<IContentContainer> ReadAsync(Stream stream)
        {
            return Task.Run(() => ReadStream(stream));
        }

        private IContentContainer Read(ModelRoot modelRoot)
        {
            var context = new ReaderContext
            {
                ModelRoot = modelRoot,
                Container = new ContentContainer()
            };
            var skinsPerMesh = new Skin[context.ModelRoot.LogicalMeshes.Count];

            foreach (var node in context.ModelRoot.LogicalNodes)
                if (node.Mesh != null && node.Skin != null)
                {
                    if (skinsPerMesh[node.Mesh.LogicalIndex] != null &&
                        skinsPerMesh[node.Mesh.LogicalIndex] != node.Skin)
                        throw new NotImplementedException("Multiple skins per mesh aren't supported");
                    skinsPerMesh[node.Mesh.LogicalIndex] = node.Skin;
                }

            var meshAndSkin = context.ModelRoot.LogicalMeshes.Zip(skinsPerMesh, (m, s) => new {Mesh = m, Skin = s})
                .ToList();
            context.Cameras = context.Container.Cameras.AddRange(context.ModelRoot.LogicalCameras, _ => _.Name,
                (m, id) => TransformCamera(m, id, context));
            context.Lights = context.Container.Lights.AddRange(context.ModelRoot.LogicalPunctualLights, _ => _.Name,
                (m, id) => TransformLight(m, id, context));
            context.Materials = context.Container.Materials.AddRange(context.ModelRoot.LogicalMaterials, _ => _.Name,
                (m, id) => TransformMaterial(m, id, context));
            context.Meshes = context.Container.Meshes.AddRange(meshAndSkin, _ => _.Mesh.Name,
                (m, id) => TransformMesh(id, m.Mesh, m.Skin));

            context.Nodes = context.Container.Nodes.AddRange(context.ModelRoot.LogicalNodes, _ => _.Name,
                (node, id) => TransformNode(node, id, context));

            context.Container.Scenes.AddRange(context.ModelRoot.LogicalScenes, _ => _.Name,
                (s, id) => TransformScene(s, id, context));

            return context.Container;
        }

        private ICameraAsset TransformCamera(Camera o, string id, ReaderContext context)
        {
            return new CameraAsset(id);
        }

        private ILightAsset TransformLight(PunctualLight o, string id, ReaderContext context)
        {
            return new LightAsset(id);
        }

        private IMaterialAsset TransformMaterial(Material material, string id, ReaderContext context)
        {
            return new MaterialAsset(id);
        }

        private IContentContainer ReadStream(Stream stream)
        {
            return Read(ModelRoot.Read(stream, _readSettings));
        }

        private IMesh TransformMesh(string id, Mesh mesh, Skin skin)
        {
            var gpuMesh = new GpuMesh(id);
            gpuMesh.Primitives.Capacity = mesh.Primitives.Count;

            var bufferViews = new Dictionary<BufferViewKey, BufferViewReader>();

            //for (var targetIndex = 0; targetIndex < primitive.MorphTargetsCount; ++targetIndex)
            //{
            //    foreach (var vertexAccessor in primitive.GetMorphTargetAccessors(targetIndex))
            //    {
            //        var key = "TARGET_"+vertexAccessor.Key+ targetIndex;
            //        var accessor = vertexAccessor.Value;
            //        accessorCollection.Register(key, accessor)
            //    }
            //}

            foreach (var primitive in mesh.Primitives)
            {
                var vertexAccessors = primitive.VertexAccessors;
                var bufferViewKey = new BufferViewKey(vertexAccessors);
                if (!bufferViews.TryGetValue(bufferViewKey, out var bufferViewReader))
                {
                    bufferViewReader = new BufferViewReader {BufferView = new MeshBufferView()};
                    foreach (var stream in bufferViewKey)
                    {
                        var streamReader = CreateStream(stream);
                        bufferViewReader.StreamReaders.Add(streamReader);
                        if (streamReader != null)
                            bufferViewReader.BufferView.SetStream(stream.Key, streamReader.GetMeshStream());
                    }
                }

                var anyStreamReader = bufferViewReader.StreamReaders.FirstOrDefault(_ => _ != null);
                if (anyStreamReader == null)
                    continue;

                var offset = anyStreamReader.GetMeshStream().Count;
                var (topology, indices) = TransformPrimitive(primitive);

                var indexMap = new DictionaryMeshStream<int>(StreamConverterFactory.Default);
                var remappedIndices = new List<int>();
                foreach (var index in indices) remappedIndices.Add(indexMap.Add(index) + offset);
                foreach (var stream in bufferViewKey.Zip(bufferViewReader.StreamReaders,
                    (k, r) => new {Key = k, Reader = r}))
                    if (stream.Reader != null)
                    {
                        var accessor = vertexAccessors[stream.Key.AcessorKey];
                        stream.Reader.Append(accessor, indexMap);
                    }

                gpuMesh.Primitives.Add(new GpuPrimitive(topology, remappedIndices, bufferViewReader.BufferView));
            }

            return gpuMesh;
        }

        private BufferViewReader.StreamReader CreateStream(BufferStreamKey stream)
        {
            //if (stream.Encoding == EncodingType.FLOAT)
            {
                switch (stream.Dimensions)
                {
                    case DimensionType.SCALAR:
                        return new BufferViewReader.ScalarBufferStreamReader();
                    case DimensionType.VEC2:
                        return new BufferViewReader.Vec2BufferStreamReader();
                    case DimensionType.VEC3:
                        return new BufferViewReader.Vec3BufferStreamReader();
                    case DimensionType.VEC4:
                        return new BufferViewReader.Vec4BufferStreamReader();
                    default:
                        throw new NotImplementedException(stream.Dimensions + " not supported yet.");
                }
            }
            return null;
        }


        private (PrimitiveTopology, IEnumerable<int>) TransformPrimitive(MeshPrimitive primitive)
        {
            switch (primitive.DrawPrimitiveType)
            {
                case PrimitiveType.TRIANGLES:
                    return (PrimitiveTopology.TriangleList,
                        primitive.GetTriangleIndices().SelectMany(_ => new[] {_.Item1, _.Item2, _.Item3}));
                case PrimitiveType.TRIANGLE_STRIP:
                    return (PrimitiveTopology.TriangleList,
                        primitive.GetTriangleIndices().SelectMany(_ => new[] {_.Item1, _.Item2, _.Item3}));
                case PrimitiveType.TRIANGLE_FAN:
                    return (PrimitiveTopology.TriangleList,
                        primitive.GetTriangleIndices().SelectMany(_ => new[] {_.Item1, _.Item2, _.Item3}));
                case PrimitiveType.POINTS:
                    return (PrimitiveTopology.PointList, primitive.GetPointIndices());
                case PrimitiveType.LINES:
                    return (PrimitiveTopology.LineList,
                        primitive.GetLineIndices().SelectMany(_ => new[] {_.Item1, _.Item2}));
                case PrimitiveType.LINE_STRIP:
                    return (PrimitiveTopology.LineList,
                        primitive.GetLineIndices().SelectMany(_ => new[] {_.Item1, _.Item2}));
                case PrimitiveType.LINE_LOOP:
                    return (PrimitiveTopology.LineList,
                        primitive.GetLineIndices().SelectMany(_ => new[] {_.Item1, _.Item2}));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ISceneAsset TransformScene(Scene scene, string id, ReaderContext context)
        {
            var sceneAsset = new SceneAsset(id);
            foreach (var node in scene.VisualChildren)
            {
                var nodeAsset = (NodeAsset) context.Nodes[node.LogicalIndex];
                sceneAsset.Add(nodeAsset);
                AttachChildren(node, nodeAsset, context);
            }

            return sceneAsset;
        }

        private void AttachChildren(Node node, NodeAsset parentAsset, ReaderContext context)
        {
            foreach (var childNode in node.VisualChildren)
            {
                var nodeAsset = (NodeAsset) context.Nodes[childNode.LogicalIndex];
                nodeAsset.Parent = parentAsset;
                AttachChildren(childNode, nodeAsset, context);
            }
        }

        private INodeAsset TransformNode(Node node, string id, ReaderContext context)
        {
            var nodeAsset = new NodeAsset(id);
            nodeAsset.Transform.Matrix = node.LocalMatrix;
            if (node.Mesh != null)
                nodeAsset.Mesh = new MeshInstance(context.Meshes[node.Mesh.LogicalIndex],
                    node.Mesh.Primitives.Select(_ => context.Materials[_.LogicalIndex]).ToList());
            if (node.Camera != null) nodeAsset.Camera = context.Cameras[node.Camera.LogicalIndex];
            if (node.PunctualLight != null) nodeAsset.Light = context.Cameras[node.PunctualLight.LogicalIndex];
            return nodeAsset;
        }
    }
}