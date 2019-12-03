using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using SharpGLTF.Schema2;

namespace Toe.ContentPipeline.GLTFSharp
{
    public class GltfSharpWriter : IFileWriter
    {
        public Task WriteAsync(Stream stream, IContentContainer content)
        {
            return Task.Run(async () =>
            {
                var context = new WriterContext
                {
                    Container = content,
                    ModelRoot = ModelRoot.CreateModel()
                };
                foreach (var textureAsset in content.Images)
                {
                    var image = context.ModelRoot.CreateImage(textureAsset.Id);
                    image.SetSatelliteContent((await textureAsset.GetContentAsync()).ToArray());
                    context.Textures.Add(textureAsset, image);
                }

                foreach (var materialAsset in content.Materials)
                {
                    var material = context.ModelRoot.CreateMaterial(materialAsset.Id);
                    material.AlphaCutoff = materialAsset.AlphaCutoff;
                    material.Alpha = GetAlphaMode(materialAsset.Alpha);
                    material.DoubleSided = materialAsset.DoubleSided;
                    if (materialAsset.Unlit) material = material.WithUnlit();

                    var metallicRoughnessShader = materialAsset.Shader as MetallicRoughnessShader;
                    if (metallicRoughnessShader != null)
                    {
                        material = material.WithPBRMetallicRoughness();
                    }
                    else
                    {
                        var specularGlossinessShader = materialAsset.Shader as SpecularGlossinessShader;
                        if (specularGlossinessShader != null)
                            material = material.WithPBRSpecularGlossiness();
                        else
                            material.WithDefault();
                    }

                    if (materialAsset.Shader != null)
                        foreach (var parameter in materialAsset.Shader.Parameters)
                        {
                            var parameterKey = parameter.Key;
                            var materialChannel = material.FindChannel(parameterKey);
                            if (materialChannel != null)
                            {
                                if (parameter.Image != null)
                                {
                                    var contextTexture = context.Textures[parameter.Image];
                                    material = material.WithChannelTexture(parameterKey, parameter.TextureCoordinate,
                                        contextTexture);
                                }

                                switch (parameter.Key)
                                {
                                    case ShaderParameterKey.Emissive:
                                        if (parameter.Value != ShaderAsset.DefaultEmissive.Value)
                                            material = material.WithChannelParameter(parameterKey, parameter.Value);
                                        break;
                                    case ShaderParameterKey.BaseColor:
                                        if (parameter.Value != ShaderAsset.DefaultBaseColor.Value)
                                            material = material.WithChannelParameter(parameterKey, parameter.Value);
                                        break;
                                    case ShaderParameterKey.MetallicRoughness:
                                        if (parameter.Value != ShaderAsset.DefaultMetallicRoughness.Value)
                                            material = material.WithChannelParameter(parameterKey, parameter.Value);
                                        break;
                                    case ShaderParameterKey.Diffuse:
                                        if (parameter.Value != ShaderAsset.DefaultDiffuse.Value)
                                            material = material.WithChannelParameter(parameterKey, parameter.Value);
                                        break;
                                    case ShaderParameterKey.Normal:
                                        if (parameter.Value != ShaderAsset.DefaultNormal.Value && parameter.Image != null)
                                            material = material.WithChannelParameter(parameterKey, parameter.Value);
                                        break;
                                    case ShaderParameterKey.SpecularGlossiness:
                                        if (parameter.Value != ShaderAsset.DefaultSpecularGlossiness.Value)
                                            material = material.WithChannelParameter(parameterKey, parameter.Value);
                                        break;
                                    case ShaderParameterKey.Occlusion:
                                        if (parameter.Value != ShaderAsset.DefaultOcclusion.Value && parameter.Image != null)
                                            material = material.WithChannelParameter(parameterKey, parameter.Value);
                                        break;
                                    default:
                                        material = material.WithChannelParameter(parameterKey, parameter.Value);
                                        break;
                                }
                            }
                        }

                    context.Materials.Add(materialAsset, material);
                }

                foreach (var nodeAsset in GetAllNodes(content))
                    if (nodeAsset.Mesh != null)
                    {
                        IList<IMaterialAsset> materials;
                        if (context.MeshInstances.TryGetValue(nodeAsset.Mesh.Mesh, out materials))
                        {
                            if (materials.Count != nodeAsset.Mesh.Materials.Count)
                                throw new FormatException("Mesh materials doesn't match all mesh instances");

                            if (!materials.Zip(nodeAsset.Mesh.Materials, (a, b) => a == b).All(_ => _))
                                throw new FormatException("Mesh materials doesn't match all mesh instances");
                            continue;
                        }

                        context.MeshInstances.Add(nodeAsset.Mesh.Mesh, nodeAsset.Mesh.Materials);
                    }

                foreach (var meshAsset in content.Meshes) TransformMesh(context, meshAsset);
                foreach (var sceneAsset in content.Scenes)
                {
                    var scene = context.ModelRoot.UseScene(sceneAsset.Id);
                    if (context.ModelRoot.DefaultScene == null)
                        context.ModelRoot.DefaultScene = scene;

                    foreach (var nodeAsset in sceneAsset.ChildNodes)
                        TransformNode(context, nodeAsset, scene.CreateNode(nodeAsset.Id));
                }

                context.ModelRoot.WriteGLB(stream);
            });
        }

        private SharpGLTF.Schema2.AlphaMode GetAlphaMode(AlphaMode materialAssetAlpha)
        {
            switch (materialAssetAlpha)
            {
                case AlphaMode.Opaque:
                    return SharpGLTF.Schema2.AlphaMode.OPAQUE;
                case AlphaMode.Mask:
                    return SharpGLTF.Schema2.AlphaMode.MASK;
                case AlphaMode.Blend:
                    return SharpGLTF.Schema2.AlphaMode.BLEND;
                default:
                    throw new ArgumentOutOfRangeException(nameof(materialAssetAlpha), materialAssetAlpha, null);
            }
        }

        private IEnumerable<INodeAsset> GetAllNodes(IContentContainer content)
        {
            var queue = new Queue<INodeAsset>();
            foreach (var sceneAsset in content.Scenes)
            foreach (var nodeAsset in sceneAsset.ChildNodes)
            {
                yield return nodeAsset;
                if (nodeAsset.HasChildren) queue.Enqueue(nodeAsset);
            }

            while (queue.Count > 0)
            {
                var nodeAsset = queue.Dequeue();
                foreach (var childNode in nodeAsset.ChildNodes)
                {
                    yield return childNode;
                    if (childNode.HasChildren) queue.Enqueue(childNode);
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

            var memoryStream = new MemoryStream(4096);

            var bufferViewMap = new Dictionary<IBufferView, BufferVeiwData>();
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                foreach (var bufferView in gpuMesh.BufferViews)
                {
                    var bufferViewContext = new BufferVeiwData();
                    bufferViewMap[bufferView] = bufferViewContext;
                    bufferViewContext.BufferByteOffset = (int) memoryStream.Position;
                    foreach (var streamKey in bufferView.GetStreams())
                    {
                        var stream = bufferView.GetStream(streamKey);
                        var accessorData = new AccessorData();
                        accessorData.Key = streamKey;
                        accessorData.AttributeKey = GetAttributeKey(streamKey);
                        accessorData.Source = GetSource(accessorData.Key, stream);
                        accessorData.Dimentions = GetDimensionType(stream.MetaInfo);
                        accessorData.Encoding = accessorData.Source.Encoding;
                        accessorData.Normalized = false;
                        accessorData.ItemCount = stream.Count;
                        if (bufferViewContext.ItemCount != 0 && bufferViewContext.ItemCount != stream.Count)
                            throw new InvalidOperationException(
                                "Number of elements in stream should be constant for all streams in gpu buffer");
                        bufferViewContext.ItemCount = stream.Count;
                        accessorData.BufferByteOffset = bufferViewContext.Stride;
                        bufferViewContext.Stride += accessorData.Source.ElementSizeInBytes;
                        bufferViewContext.Accessors.Add(accessorData);
                    }

                    for (var i = 0; i < bufferViewContext.ItemCount; ++i)
                        foreach (var accessorData in bufferViewContext.Accessors)
                            accessorData.Source.WriteNext(binaryWriter);
                }
            }

            var buffer = mesh.LogicalParent.UseBuffer(memoryStream.ToArray());
            foreach (var bufferVeiwData in bufferViewMap.Values)
            {
                var bufferView = mesh.LogicalParent.UseBufferView(buffer, bufferVeiwData.BufferByteOffset, null,
                    bufferVeiwData.Stride,
                    BufferMode.ARRAY_BUFFER);
                foreach (var data in bufferVeiwData.Accessors)
                {
                    var accessor = mesh.LogicalParent.CreateAccessor();
                    accessor.SetVertexData(bufferView, data.BufferByteOffset, data.ItemCount, data.Dimentions,
                        data.Encoding, data.Normalized);
                    data.MemoryAccessor = accessor;
                }
            }

            for (var primitiveIndex = 0; primitiveIndex < gpuMesh.Primitives.Count; primitiveIndex++)
            {
                var gpuPrimitive = gpuMesh.Primitives[primitiveIndex];
                var bufView = bufferViewMap[gpuPrimitive.BufferView];
                var primitive = mesh.CreatePrimitive();
                primitive.WithMaterial(context.Materials[materials[primitiveIndex % materials.Count]]);
                foreach (var accessor in bufView.Accessors)
                    primitive.SetVertexAccessor(accessor.AttributeKey, accessor.MemoryAccessor);

                primitive.WithIndicesAccessor(GetPrimitiveType(gpuPrimitive.Topology), gpuPrimitive);
            }

            return mesh;
        }

        private Source GetSource(StreamKey key, IMeshStream stream)
        {
            if (key.Key == Streams.Joints) return new Vec4USSource(stream);
            switch (stream.MetaInfo.ComponentsPerSet)
            {
                case 1:
                    return new FloatSource(stream);
                case 2:
                    return new Vec2Source(stream);
                case 3:
                    return new Vec3Source(stream);
                case 4:
                    return new Vec4Source(stream);
            }

            throw new NotImplementedException();
        }

        private DimensionType GetDimensionType(IStreamMetaInfo streamMetaInfo)
        {
            if (streamMetaInfo.NumberOfSets == 1)
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

            throw new NotImplementedException(
                $"Can't deduce dimension type from {streamMetaInfo.NumberOfSets}x{streamMetaInfo.ComponentsPerSet} data dimensions");
        }

        private void CopyStream(IMeshStream stream, BinaryWriter binaryWriter)
        {
            var reader = stream.GetReader<IEnumerable<float>>();
            foreach (var value in reader.SelectMany(_ => _)) binaryWriter.Write(value);
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
            foreach (var value in reader) binaryWriter.Write(value);
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
            node.LocalMatrix = nodeAsset.Transform.Matrix;

            if (nodeAsset.Mesh != null)
            {
                Mesh mesh;
                if (!context.Meshes.TryGetValue(nodeAsset.Mesh.Mesh, out mesh))
                    mesh = TransformMesh(context, nodeAsset.Mesh.Mesh, nodeAsset.Mesh.Materials);
                node.Mesh = mesh;
            }

            foreach (var childNode in nodeAsset.ChildNodes)
                TransformNode(context, childNode, node.CreateNode(childNode.Id));
        }

        private class BufferVeiwData
        {
            public int BufferByteOffset { get; set; }
            public IList<AccessorData> Accessors { get; } = new List<AccessorData>();
            public int Stride { get; set; }
            public int ItemCount { get; set; }
        }

        private class AccessorData
        {
            public StreamKey Key { get; set; }
            public string AttributeKey { get; set; }
            public DimensionType Dimentions { get; set; }
            public EncodingType Encoding { get; set; }
            public bool Normalized { get; set; }
            public Accessor MemoryAccessor { get; set; }
            public Source Source { get; set; }
            public int ItemCount { get; set; }
            public int BufferByteOffset { get; set; }
        }

        internal abstract class Source
        {
            public abstract int ElementSizeInBytes { get; }

            public virtual EncodingType Encoding => EncodingType.FLOAT;

            public abstract void WriteNext(BinaryWriter binaryWriter);
        }

        internal abstract class Source<T> : Source
        {
            protected readonly IList<T> _data;
            protected int _index;

            public Source(IMeshStream data)
            {
                _data = data.GetReader<T>();
            }
        }

        internal class FloatSource : Source<float>
        {
            public FloatSource(IMeshStream data) : base(data)
            {
            }

            public override int ElementSizeInBytes => sizeof(float);

            public override void WriteNext(BinaryWriter binaryWriter)
            {
                binaryWriter.Write(_data[_index]);
                ++_index;
            }
        }

        internal class Vec2Source : Source<Vector2>
        {
            public Vec2Source(IMeshStream data) : base(data)
            {
            }

            public override int ElementSizeInBytes => sizeof(float) * 2;

            public override void WriteNext(BinaryWriter binaryWriter)
            {
                binaryWriter.Write(_data[_index]);
                ++_index;
            }
        }

        internal class Vec3Source : Source<Vector3>
        {
            public Vec3Source(IMeshStream data) : base(data)
            {
            }

            public override int ElementSizeInBytes => sizeof(float) * 3;

            public override void WriteNext(BinaryWriter binaryWriter)
            {
                binaryWriter.Write(_data[_index]);
                ++_index;
            }
        }

        internal class Vec4Source : Source<Vector4>
        {
            public Vec4Source(IMeshStream data) : base(data)
            {
            }

            public override int ElementSizeInBytes => sizeof(float) * 4;

            public override void WriteNext(BinaryWriter binaryWriter)
            {
                binaryWriter.Write(_data[_index]);
                ++_index;
            }
        }

        internal class Vec4USSource : Source<Vector4us>
        {
            public Vec4USSource(IMeshStream data) : base(data)
            {
            }

            public override int ElementSizeInBytes => sizeof(ushort) * 4;
            public override EncodingType Encoding => EncodingType.UNSIGNED_SHORT;

            public override void WriteNext(BinaryWriter binaryWriter)
            {
                binaryWriter.Write(_data[_index]);
                ++_index;
            }
        }
    }
}