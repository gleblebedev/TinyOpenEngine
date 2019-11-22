using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using SharpGLTF.Schema2;
using Toe.ContentPipeline;

namespace Toe.ConentPipeline.GLTFSharp
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

        private IContentContainer ReadStream(Stream stream)
        {
            var container = new ContentContainer();
            var model = ModelRoot.Read(stream, _readSettings);
            var skinsPerMesh = new Skin[model.LogicalMeshes.Count];

            foreach (var node in model.LogicalNodes)
                if (node.Mesh != null && node.Skin != null)
                {
                    if (skinsPerMesh[node.Mesh.LogicalIndex] != null &&
                        skinsPerMesh[node.Mesh.LogicalIndex] != node.Skin)
                        throw new NotImplementedException("Multiple skins per mesh aren't supported");
                    skinsPerMesh[node.Mesh.LogicalIndex] = node.Skin;
                }

            var meshAndSkin = model.LogicalMeshes.Zip(skinsPerMesh, (m, s) => new {Mesh = m, Skin = s}).ToList();
            container.Meshes.AddRange(meshAndSkin, _ => _.Mesh.Name, (m, id) => TransformMesh(id, m.Mesh, m.Skin));

            container.Nodes.AddRange(model.LogicalNodes, _ => _.Name, TransformNode);

            container.Scenes.AddRange(model.LogicalScenes, _ => _.Name, TransformScene);

            return container;
        }

        private IMesh TransformMesh(string id, Mesh mesh, Skin skin)
        {
            var gpuMesh = new GpuMesh(id);
            gpuMesh.Primitives.Capacity = mesh.Primitives.Count;
            var meshStreamsCollection = new MeshStreamsCollection();

            foreach (var primitive in mesh.Primitives)
            foreach (var vertexAccessor in primitive.VertexAccessors)
            {
                var key = vertexAccessor.Key;
                var accessor = vertexAccessor.Value;
                meshStreamsCollection.Register(key, accessor);
            }
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
                gpuMesh.Primitives.Add(TransformPrimitive(primitive, meshStreamsCollection.First().Value.Count));

                foreach (var keyAndStream in meshStreamsCollection)
                    if (primitive.VertexAccessors.TryGetValue(keyAndStream.Key, out var accessor))
                    {
                        switch (accessor.Dimensions)
                        {
                            case DimensionType.SCALAR:
                                ((IMeshStream<float>) keyAndStream.Value).AddRange(accessor.AsScalarArray());
                                break;
                            case DimensionType.VEC2:
                                ((IMeshStream<Vector2>) keyAndStream.Value).AddRange(accessor.AsVector2Array());
                                break;
                            case DimensionType.VEC3:
                                ((IMeshStream<Vector3>) keyAndStream.Value).AddRange(accessor.AsVector3Array());
                                break;
                            case DimensionType.VEC4:
                                ((IMeshStream<Vector4>) keyAndStream.Value).AddRange(accessor.AsVector4Array());
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        var items = primitive.VertexAccessors.First().Value.Count;
                        keyAndStream.Value.AddDefault(items);
                    }
            }

            foreach (var accessor in meshStreamsCollection)
                gpuMesh.SetStream(GetStreamKey(accessor.Key), accessor.Value);
            return gpuMesh;
        }

        private StreamKey GetStreamKey(string key)
        {
            if (key.Length > 2 && key[key.Length - 2] == '_' && char.IsDigit(key[key.Length - 1]))
                return new StreamKey(key.Substring(0, key.Length - 2),
                    int.Parse(key.Substring(key.Length - 1), CultureInfo.InvariantCulture));
            return new StreamKey(key, 0);
        }

        private GpuPrimitive TransformPrimitive(MeshPrimitive primitive, int indexOffset)
        {
            switch (primitive.DrawPrimitiveType)
            {
                case PrimitiveType.TRIANGLES:
                    return new GpuPrimitive(PrimitiveTopology.TriangleList,
                        primitive.GetTriangleIndices().SelectMany(_ => new[] {_.Item1, _.Item2, _.Item3})
                            .Select(_ => _ + indexOffset));
                case PrimitiveType.TRIANGLE_STRIP:
                    return new GpuPrimitive(PrimitiveTopology.TriangleList,
                        primitive.GetTriangleIndices().SelectMany(_ => new[] {_.Item1, _.Item2, _.Item3})
                            .Select(_ => _ + indexOffset));
                case PrimitiveType.TRIANGLE_FAN:
                    return new GpuPrimitive(PrimitiveTopology.TriangleList,
                        primitive.GetTriangleIndices().SelectMany(_ => new[] {_.Item1, _.Item2, _.Item3})
                            .Select(_ => _ + indexOffset));
                case PrimitiveType.POINTS:
                    return new GpuPrimitive(PrimitiveTopology.PointList,
                        primitive.GetPointIndices().Select(_ => _ + indexOffset));
                case PrimitiveType.LINES:
                    return new GpuPrimitive(PrimitiveTopology.LineList,
                        primitive.GetLineIndices().SelectMany(_ => new[] {_.Item1, _.Item2})
                            .Select(_ => _ + indexOffset));
                case PrimitiveType.LINE_STRIP:
                    return new GpuPrimitive(PrimitiveTopology.LineList,
                        primitive.GetLineIndices().SelectMany(_ => new[] {_.Item1, _.Item2})
                            .Select(_ => _ + indexOffset));
                case PrimitiveType.LINE_LOOP:
                    return new GpuPrimitive(PrimitiveTopology.LineList,
                        primitive.GetLineIndices().SelectMany(_ => new[] {_.Item1, _.Item2})
                            .Select(_ => _ + indexOffset));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ISceneAsset TransformScene(Scene scene, string id)
        {
            var sceneAsset = new SceneAsset(id);
            return sceneAsset;
        }

        private INodeAsset TransformNode(Node node, string id)
        {
            var nodeAsset = new NodeAsset(id);
            nodeAsset.Transform.Matrix = node.LocalMatrix;
            return nodeAsset;
        }
    }
}