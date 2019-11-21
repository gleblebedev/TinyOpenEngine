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
            var accessorCollection = new AccessorCollection();
            foreach (var primitive in mesh.Primitives)
            {
                foreach (var vertexAccessor in primitive.VertexAccessors)
                {
                    var key = vertexAccessor.Key;
                    var accessor = vertexAccessor.Value;
                    accessorCollection.Add(key, accessor);
                }

                //for (var targetIndex = 0; targetIndex < primitive.MorphTargetsCount; ++targetIndex)
                //{
                //    foreach (var vertexAccessor in primitive.GetMorphTargetAccessors(targetIndex))
                //    {
                //        var key = "TARGET_"+vertexAccessor.Key+ targetIndex;
                //        var accessor = vertexAccessor.Value;
                //        accessorCollection.Add(key, accessor)
                //    }
                //}

                gpuMesh.Primitives.Add(TransformPrimitive(gpuMesh, primitive));
            }

            foreach (var accessor in accessorCollection)
            {
                gpuMesh.SetStream(GetStreamKey(accessor.Key), TransformAccessor(accessor.Value));
            }
            return gpuMesh;
        }

        private IMeshStream TransformAccessor(Accessor accessor)
        {
            switch (accessor.Dimensions)
            {
                case DimensionType.SCALAR:
                    return new ListMeshStream<float>( accessor.AsScalarArray());
                case DimensionType.VEC2:
                    return new ListMeshStream<Vector2>(accessor.AsVector2Array());
                case DimensionType.VEC3:
                    return new ListMeshStream<Vector3>(accessor.AsVector3Array());
                case DimensionType.VEC4:
                    return new ListMeshStream<Vector4>(accessor.AsVector4Array());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private StreamKey GetStreamKey(string key)
        {
            if (key.Length > 2 && key[key.Length-2] == '_' && char.IsDigit(key[key.Length - 1]))
                return new StreamKey(key.Substring(0,key.Length-2), int.Parse(key.Substring(key.Length-1), CultureInfo.InvariantCulture));
            return new StreamKey(key, 0);
        }

        private GpuPrimitive TransformPrimitive(GpuMesh gpuMesh, MeshPrimitive primitive)
        {
            switch (primitive.DrawPrimitiveType)
            {
                case PrimitiveType.TRIANGLES:
                    return new GpuPrimitive(PrimitiveTopology.TriangleList, primitive.GetTriangleIndices().SelectMany(_ => new[] { _.Item1, _.Item2, _.Item3 }));
                case PrimitiveType.TRIANGLE_STRIP:
                    return new GpuPrimitive(PrimitiveTopology.TriangleList, primitive.GetTriangleIndices().SelectMany(_ => new[] { _.Item1, _.Item2, _.Item3 }));
                case PrimitiveType.TRIANGLE_FAN:
                    return new GpuPrimitive(PrimitiveTopology.TriangleList, primitive.GetTriangleIndices().SelectMany(_ => new[] { _.Item1, _.Item2, _.Item3 }));
                case PrimitiveType.POINTS:
                    return new GpuPrimitive(PrimitiveTopology.PointList, primitive.GetPointIndices());
                case PrimitiveType.LINES:
                    return new GpuPrimitive(PrimitiveTopology.LineList, primitive.GetLineIndices().SelectMany(_ => new[] { _.Item1, _.Item2}));
                case PrimitiveType.LINE_STRIP:
                    return new GpuPrimitive(PrimitiveTopology.LineList, primitive.GetLineIndices().SelectMany(_ => new[] { _.Item1, _.Item2 }));
                case PrimitiveType.LINE_LOOP:
                    return new GpuPrimitive(PrimitiveTopology.LineList, primitive.GetLineIndices().SelectMany(_ => new[] { _.Item1, _.Item2 }));
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