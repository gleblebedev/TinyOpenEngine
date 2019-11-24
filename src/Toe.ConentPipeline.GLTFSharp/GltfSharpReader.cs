using System;
using System.Collections.Generic;
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

        private IContentContainer Read(ModelRoot modelRoot)
        {
            var context = new ReaderContext()
            {
                ModelRoot = modelRoot,
                Container = new ContentContainer(),
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

            var meshAndSkin = context.ModelRoot.LogicalMeshes.Zip(skinsPerMesh, (m, s) => new { Mesh = m, Skin = s }).ToList();
            context.Cameras = context.Container.Cameras.AddRange(context.ModelRoot.LogicalCameras, _ => _.Name, (m, id) => TransformCamera(m, id, context));
            context.Lights = context.Container.Lights.AddRange(context.ModelRoot.LogicalPunctualLights, _ => _.Name, (m, id) => TransformLight(m, id, context));
            context.Materials = context.Container.Materials.AddRange(context.ModelRoot.LogicalMaterials, _ => _.Name, (m, id) => TransformMaterial(m, id, context));
            context.Meshes = context.Container.Meshes.AddRange(meshAndSkin, _ => _.Mesh.Name, (m, id) => TransformMesh(id, m.Mesh, m.Skin));

            context.Nodes = context.Container.Nodes.AddRange(context.ModelRoot.LogicalNodes, _ => _.Name, (node, id) => TransformNode(node, id, context));

            context.Container.Scenes.AddRange(context.ModelRoot.LogicalScenes, _ => _.Name, (s, id) => TransformScene(s, id, context));

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

        private ISceneAsset TransformScene(Scene scene, string id, ReaderContext context)
        {
            var sceneAsset = new SceneAsset(id);
            foreach (var node in scene.VisualChildren)
            {
                var nodeAsset = (NodeAsset)context.Nodes[node.LogicalIndex];
                sceneAsset.Add(nodeAsset);
                AttachChildren(node, nodeAsset, context);
            }
            return sceneAsset;
        }

        private void AttachChildren(Node node, NodeAsset parentAsset, ReaderContext context)
        {
            foreach (var childNode in node.VisualChildren)
            {
                var nodeAsset = (NodeAsset)context.Nodes[childNode.LogicalIndex];
                nodeAsset.Parent = parentAsset;
                AttachChildren(childNode, nodeAsset, context);
            }
        }

        private INodeAsset TransformNode(Node node, string id, ReaderContext context)
        {
            var nodeAsset = new NodeAsset(id);
            nodeAsset.Transform.Matrix = node.LocalMatrix;
            if (node.Mesh != null)
            {
                nodeAsset.Mesh = new MeshInstance(context.Meshes[node.Mesh.LogicalIndex], node.Mesh.Primitives.Select(_=>context.Materials[_.LogicalIndex]).ToList());
            }
            if (node.Camera != null)
            {
                nodeAsset.Camera = context.Cameras[node.Camera.LogicalIndex];
            }
            if (node.PunctualLight != null)
            {
                nodeAsset.Light = context.Cameras[node.PunctualLight.LogicalIndex];
            }
            return nodeAsset;
        }
    }
}