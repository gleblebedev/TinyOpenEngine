using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using SharpGLTF.Schema2;
using Toe.ContentPipeline;

namespace Toe.ConentPipeline.GLTFSharp
{
    public class GltfSharpWriter : IFileWriter
    {
        public Task WriteAsync(Stream stream, IContentContainer content)
        {
            return Task.Run(() =>
            {
                var context = new WriterContext()
                {
                    Container = content,
                    ModelRoot = ModelRoot.CreateModel(),
                };
                foreach (var materialAsset in content.Materials)
                {
                    var material = context.ModelRoot.CreateMaterial(materialAsset.Id);
                    context.Materials.Add(materialAsset, material);
                }

                foreach (var nodeAsset in GetAllNodes(content))
                {
                    if (nodeAsset.Mesh != null)
                    {
                        IList<IMaterialAsset> materials;
                        if (context.MeshInstances.TryGetValue(nodeAsset.Mesh.Mesh, out materials))
                        {
                            if (materials.Count != nodeAsset.Mesh.Materials.Count)
                            {
                                throw new FormatException("Mesh materials doesn't match all mesh instances");
                            }

                            if (!materials.Zip(nodeAsset.Mesh.Materials, (a, b) => a == b).All(_ => _))
                            {
                                throw new FormatException("Mesh materials doesn't match all mesh instances");
                            }
                            continue;
                        }

                        context.MeshInstances.Add(nodeAsset.Mesh.Mesh, nodeAsset.Mesh.Materials);
                    }
                }
                foreach (var meshAsset in content.Meshes)
                {
                    TransformMesh(context, meshAsset);
                }
                foreach (var sceneAsset in content.Scenes)
                {
                    var scene = context.ModelRoot.UseScene(sceneAsset.Id);
                    if (context.ModelRoot.DefaultScene == null)
                        context.ModelRoot.DefaultScene = scene;

                    foreach (var nodeAsset in sceneAsset.ChildNodes)
                    {
                        TransformNode(context, nodeAsset, scene.CreateNode(nodeAsset.Id));
                    }
                }
                context.ModelRoot.WriteGLB(stream);
            });
        }

        private IEnumerable<INodeAsset> GetAllNodes(IContentContainer content)
        {
            var queue = new Queue<INodeAsset>();
            foreach (var sceneAsset in content.Scenes)
            {
                foreach (var nodeAsset in sceneAsset.ChildNodes)
                {
                    yield return nodeAsset;
                    if (nodeAsset.HasChildren)
                    {
                        queue.Enqueue(nodeAsset);
                    }
                }
            }

            while (queue.Count > 0)
            {
                var nodeAsset = queue.Dequeue();
                foreach (var childNode in nodeAsset.ChildNodes)
                {
                    yield return childNode;
                    if (childNode.HasChildren)
                    {
                        queue.Enqueue(childNode);
                    }
                }
            }
        }

        private Mesh TransformMesh(WriterContext context, IMesh meshAsset)
        {
            return TransformMesh(context, meshAsset, context.MeshInstances[meshAsset]);
        }

        private Mesh TransformMesh(WriterContext context, IMesh meshAsset, IList<IMaterialAsset> materials)
        {
            var gpuMesh = meshAsset.ToGpuMesh();
            var mesh = context.ModelRoot.CreateMesh(meshAsset.Id);
            context.Meshes.Add(meshAsset, mesh);

            var positionStream = gpuMesh.GetStream(StreamKey.Position).GetReader<Vector3>();
            int index = 0;
            foreach (var primitiveAsset in gpuMesh.Primitives)
            {
                var primitive = mesh.CreatePrimitive()
                    .WithMaterial(context.Materials[materials[index]]);

                DictionaryMeshStream<Vector3> positions = new DictionaryMeshStream<Vector3>(StreamConverterFactory.Default);
                var posIndices = new List<int>();
                foreach (var vertexIndex in primitiveAsset)
                {
                    posIndices.Add(positions.Add(positionStream[vertexIndex]));
                }

                primitive.WithVertexAccessor(StreamKey.Position.Key, positions.ToList());
                primitive.WithIndicesAccessor(GetPrimitiveType(primitiveAsset.Topology), posIndices);
                ++index;
            }
            return mesh;
        }

        private string GetAttributeKey(StreamKey streamKey)
        {
            if (streamKey.Channel != 0)
                return streamKey.Key + "_" + streamKey.Channel;
            if (streamKey == StreamKey.Position)
                return streamKey.Key;
            if (streamKey == StreamKey.Normal)
                return streamKey.Key;
            if (streamKey == StreamKey.Tangent)
                return streamKey.Key;
            return streamKey.Key + "_" + streamKey.Channel;
        }

        private PrimitiveType GetPrimitiveType(PrimitiveTopology primitiveAssetTopology)
        {
            switch (primitiveAssetTopology)
            {
                case PrimitiveTopology.TriangleList:
                    return PrimitiveType.TRIANGLES;
                case PrimitiveTopology.TriangleStrip:
                    return PrimitiveType.TRIANGLE_STRIP;
                case PrimitiveTopology.LineList:
                    return PrimitiveType.LINES;
                case PrimitiveTopology.LineStrip:
                    return PrimitiveType.LINE_STRIP;
                case PrimitiveTopology.PointList:
                    return PrimitiveType.POINTS;
                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveAssetTopology), primitiveAssetTopology, null);
            }
        }

        private void TransformNode(WriterContext context, INodeAsset nodeAsset, Node node)
        {
            node.LocalTransform = nodeAsset.Transform.Matrix;

            if (nodeAsset.Mesh != null)
            {
                Mesh mesh;
                if (!context.Meshes.TryGetValue(nodeAsset.Mesh.Mesh, out mesh))
                {
                    mesh = TransformMesh(context, nodeAsset.Mesh.Mesh, nodeAsset.Mesh.Materials);
                }
                node.Mesh = context.Meshes[nodeAsset.Mesh.Mesh];
            }
            foreach (var childNode in nodeAsset.ChildNodes)
            {
                TransformNode(context, childNode, node.CreateNode(childNode.Id));
            }
        }
    }
}