using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using SharpGLTF.Memory;
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


        class AccessorData
        {
            public int BufferByteOffset { get; set; }
            public int ItemCount { get; set; }
            public StreamKey Key { get; set; }
            public string AttributeKey { get; set; }
            public DimensionType Dimentions { get; set; }
            public EncodingType Encoding { get; set; }
            public bool Normalized { get; set; }
        }

        private Mesh TransformMesh(WriterContext context, IMesh meshAsset, IList<IMaterialAsset> materials)
        {
            var gpuMesh = meshAsset.ToGpuMesh();
            var mesh = context.ModelRoot.CreateMesh(meshAsset.Id);
            context.Meshes.Add(meshAsset, mesh);

            var buffer = new MemoryStream(4096);
            var accessorData = new List<AccessorData>();
            using (var binaryWriter = new BinaryWriter(buffer))
            {
                foreach (var streamKey in gpuMesh.GetStreams())
                {
                    var offset = buffer.Position;
                    var stream = gpuMesh.GetStream(streamKey);
                    if (stream.MetaInfo.NumberOfSets == 1)
                    {
                        switch (stream.MetaInfo.ComponentsPerSet)
                        {
                            case 1:
                                CopyFloatStream(stream, binaryWriter);
                                break;
                            case 2:
                                CopyVec2Stream(stream, binaryWriter);
                                break;
                            case 3:
                                CopyVec3Stream(stream, binaryWriter);
                                break;
                            case 4:
                                CopyVec4Stream(stream, binaryWriter);
                                break;
                            default:
                                CopyStream(stream, binaryWriter);
                                break;
                        }
                    }
                    else
                    {
                        CopyStream(stream, binaryWriter);
                    }
                    accessorData.Add(new AccessorData()
                    {
                        Key = streamKey,
                        AttributeKey = GetAttributeKey(streamKey),
                        BufferByteOffset = (int)offset,
                        ItemCount = stream.Count,
                        Dimentions = GetDimensionType(stream.MetaInfo),
                        Encoding = EncodingType.FLOAT,
                        Normalized = false
                    });
                }
            }

            var bufferArray = mesh.LogicalParent.UseBuffer(buffer.ToArray());
            var accessors = new Accessor[accessorData.Count];
           for (var accessorIndex = 0; accessorIndex < accessorData.Count; accessorIndex++)
            {
                var data = accessorData[accessorIndex];
                BufferView bufferView = mesh.LogicalParent.UseBufferView(bufferArray, data.BufferByteOffset, new int?(), 0, new BufferMode?(BufferMode.ARRAY_BUFFER));
                Accessor accessor = mesh.LogicalParent.CreateAccessor((string) null);
                accessors[accessorIndex] = accessor;
                accessor.SetVertexData(bufferView, 0, data.ItemCount, data.Dimentions, data.Encoding, data.Normalized);
            }

            int index = 0;
            foreach (var primitiveAsset in gpuMesh.Primitives)
            {
                var primitive = mesh.CreatePrimitive()
                    .WithMaterial(context.Materials[materials[index]]);

                for (var accessorIndex = 0; accessorIndex < accessors.Length; accessorIndex++)
                {
                    var accessor = accessors[accessorIndex];
                    primitive.SetVertexAccessor(accessorData[accessorIndex].AttributeKey, accessor);
                }
                primitive.WithIndicesAccessor(GetPrimitiveType(primitiveAsset.Topology), primitiveAsset);

                //primitive.SetVertexAccessor(StreamKey.Position.Key, accessor);


                //var posIndices = new List<int>();
                //foreach (var vertexIndex in primitiveAsset)
                //{
                //    posIndices.Add(positions.Add(positionStream[vertexIndex]));
                //}

                //ModelRoot logicalParent = primitive.LogicalParent.LogicalParent;
                //BufferView buffer = logicalParent.UseBufferView(new byte[12 * positions.Count], 0, new int?(), 0, new BufferMode?(BufferMode.ARRAY_BUFFER));
                //new Vector3Array(buffer.Content, 0, EncodingType.FLOAT, false).Fill((IEnumerable<Vector3>)positions, 0);
                //Accessor accessor = logicalParent.CreateAccessor((string) null);
                //accessor.SetVertexData(buffer, 0, positions.Count, DimensionType.VEC3, EncodingType.FLOAT, false);
                //primitive.SetVertexAccessor(StreamKey.Position.Key, accessor);

                //primitive.WithVertexAccessor(StreamKey.Position.Key, positions.ToList());
                //primitive.WithIndicesAccessor(GetPrimitiveType(primitiveAsset.Topology), posIndices);
                ++index;
            }
            return mesh;
        }

        private DimensionType GetDimensionType(IStreamMetaInfo streamMetaInfo)
        {
            if (streamMetaInfo.NumberOfSets == 1)
            {
                switch (streamMetaInfo.ComponentsPerSet)
                {
                    case 1:
                        return DimensionType.SCALAR;
                    case 2:
                        return DimensionType.VEC2;
                    case 3:
                        return DimensionType.VEC3;
                    case 4:
                        return DimensionType.VEC4;
                }
            }
            throw new NotImplementedException($"Can't deduce dimension type from {streamMetaInfo.NumberOfSets}x{streamMetaInfo.ComponentsPerSet} data dimensions");
        }

        private void CopyStream(IMeshStream stream, BinaryWriter binaryWriter)
        {
            var reader = stream.GetReader<IEnumerable<float>>();
            foreach (var value in reader.SelectMany(_=>_))
            {
                binaryWriter.Write(value);
            }
        }

        private void CopyVec2Stream(IMeshStream stream, BinaryWriter binaryWriter)
        {
            var reader = stream.GetReader<Vector2>();
            foreach (var value in reader)
            {
                binaryWriter.Write(value.X);
                binaryWriter.Write(value.Y);
            }
        }
        private void CopyVec3Stream(IMeshStream stream, BinaryWriter binaryWriter)
        {

            var reader = stream.GetReader<Vector3>();
            foreach (var value in reader)
            {
                binaryWriter.Write(value.X);
                binaryWriter.Write(value.Y);
                binaryWriter.Write(value.Z);
            }
        }
        private void CopyVec4Stream(IMeshStream stream, BinaryWriter binaryWriter)
        {

            var reader = stream.GetReader<Vector4>();
            foreach (var value in reader)
            {
                binaryWriter.Write(value.X);
                binaryWriter.Write(value.Y);
                binaryWriter.Write(value.Z);
                binaryWriter.Write(value.W);
            }
        }
        private void CopyFloatStream(IMeshStream stream, BinaryWriter binaryWriter)
        {
            var reader = stream.GetReader<float>();
            foreach (var value in reader)
            {
                binaryWriter.Write(value);
            }
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