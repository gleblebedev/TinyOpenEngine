using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using SharpGLTF.Schema2;
using Toe.SceneGraph;

namespace Toe.ContentPipeline.GLTFSharp
{
    public class GltfSharpReader : IStreamReader
    {
        private readonly ReadSettings _readSettings;
        public GltfSharpReader(AssetReader assetReader, bool skipValidation = true)
        {
            _readSettings = new ReadSettings
            {
                FileReader = assetReader,
                SkipValidation = skipValidation
            };
        }

        public GltfSharpReader(bool skipValidation = true):this(null, skipValidation)
        {
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
            var textures = context.ModelRoot.LogicalTextures.ToLookup(_ => _.PrimaryImage);
            context.Images = context.Container.Images.AddRange(context.ModelRoot.LogicalImages,
                _ => textures[_].Select(_1 => _1.Name).Concat(new[] {_.Name}).FirstOrDefault(),
                (m, id) => TransformImage(m, id, context));
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

        private IImageAsset TransformImage(Image o, string id, ReaderContext context)
        {
            var transformImage = new EmbeddedImage(o.GetImageContent());
            transformImage.Id = id;
            transformImage.FileExtension = o.FileExtension;
            return transformImage;
        }

        private ILightAsset TransformLight(PunctualLight o, string id, ReaderContext context)
        {
            return new LightAsset(id);
        }

        private IMaterialAsset TransformMaterial(Material material, string id, ReaderContext context)
        {
            var materialAsset = new MaterialAsset(id);
            materialAsset.AlphaCutoff = material.AlphaCutoff;
            materialAsset.Alpha = GetAlphaMode(material.Alpha);
            materialAsset.DoubleSided = material.DoubleSided;
            materialAsset.Unlit = material.Unlit;

            var shaderParameters = material.Channels.Select(_ => TransformShaderParameter(_, context)).SelectMany(_=>_)
                .ToDictionary(_ => _.Key, _ => _);
            var metallicRoughness = 0;
            var specularGlossiness = 0;
            foreach (var shaderParameter in shaderParameters)
                switch (shaderParameter.Key)
                {
                    case ShaderParameterKey.BaseColorFactor:
                    case ShaderParameterKey.BaseColorTexture:
                    case ShaderParameterKey.MetallicFactor:
                    case ShaderParameterKey.MetallicRoughnessTexture:
                        ++metallicRoughness;
                        break;
                    case ShaderParameterKey.DiffuseFactor:
                    case ShaderParameterKey.DiffuseTexture:
                    case ShaderParameterKey.SpecularFactor:
                    case ShaderParameterKey.GlossinessFactor:
                    case ShaderParameterKey.SpecularGlossinessTexture:
                        ++specularGlossiness;
                        break;
                }

            if (metallicRoughness > 0 && specularGlossiness == 0)
            {
                var shader = new MetallicRoughnessShader();
                materialAsset.Shader = shader;
            }
            else if (metallicRoughness == 0 && specularGlossiness >= 0)
            {
                var shader = new SpecularGlossinessShader();
                materialAsset.Shader = shader;
            }
            else
            {
                var shader = new ShaderAsset();
                materialAsset.Shader = shader;
            }

            foreach (var shaderParameter in shaderParameters) materialAsset.Shader.Set(shaderParameter.Value);

            return materialAsset;
        }

        private IEnumerable<IShaderParameter> TransformShaderParameter(MaterialChannel materialChannel, ReaderContext context)
        {
            var parameter = materialChannel.Parameter;
            switch (materialChannel.Key)
            {
                case "BaseColor":
                {
                    var samplerParameters = TransfromSamplerParameters(materialChannel, context);
                    if (samplerParameters.HasValue)
                    {
                        yield return new ShaderParameter<SamplerParameters>(ShaderParameterKey.BaseColorTexture, samplerParameters.Value);
                    }
                }
                    yield return new ShaderParameter<Vector4>(ShaderParameterKey.BaseColorFactor, parameter);
                    yield break;
                case "Diffuse":
                {
                    var samplerParameters = TransfromSamplerParameters(materialChannel, context);
                    if (samplerParameters.HasValue)
                    {
                        yield return new ShaderParameter<SamplerParameters>(ShaderParameterKey.DiffuseTexture, samplerParameters.Value);
                    }
                }
                    yield return new ShaderParameter<Vector4>(ShaderParameterKey.DiffuseFactor, parameter);
                    yield break;
                case "MetallicRoughness":
                {
                    var samplerParameters = TransfromSamplerParameters(materialChannel, context);
                    if (samplerParameters.HasValue)
                    {
                        yield return new ShaderParameter<SamplerParameters>(ShaderParameterKey.MetallicRoughnessTexture, samplerParameters.Value);
                    }
                }
                    yield return new ShaderParameter<float>(ShaderParameterKey.MetallicFactor, parameter.X);
                    yield return new ShaderParameter<float>(ShaderParameterKey.RoughnessFactor, parameter.Y);
                    yield break;
                case "SpecularGlossiness":
                {
                    var samplerParameters = TransfromSamplerParameters(materialChannel, context);
                    if (samplerParameters.HasValue)
                    {
                        yield return new ShaderParameter<SamplerParameters>(ShaderParameterKey.SpecularGlossinessTexture, samplerParameters.Value);
                    }
                }
                    yield return new ShaderParameter<Vector3>(ShaderParameterKey.SpecularFactor, new Vector3(parameter.X, parameter.Y, parameter.Z));
                    yield return new ShaderParameter<float>(ShaderParameterKey.GlossinessFactor, parameter.W);
                    yield break;
                case "Normal":
                {
                    var samplerParameters = TransfromSamplerParameters(materialChannel, context);
                    if (samplerParameters.HasValue)
                    {
                        yield return new ShaderParameter<SamplerParameters>(ShaderParameterKey.NormalTexture, samplerParameters.Value);
                    }
                }
                    yield return new ShaderParameter<float>(ShaderParameterKey.NormalTextureScale, parameter.X);
                    yield break;
                case "Occlusion":
                {
                    var samplerParameters = TransfromSamplerParameters(materialChannel, context);
                    if (samplerParameters.HasValue)
                    {
                        yield return new ShaderParameter<SamplerParameters>(ShaderParameterKey.OcclusionTexture, samplerParameters.Value);
                    }
                }
                    yield return new ShaderParameter<float>(ShaderParameterKey.OcclusionTextureStrength, parameter.X);
                    yield break;
                case "Emissive":
                {
                    var samplerParameters = TransfromSamplerParameters(materialChannel, context);
                    if (samplerParameters.HasValue)
                    {
                        yield return new ShaderParameter<SamplerParameters>(ShaderParameterKey.EmissiveTexture,
                            samplerParameters.Value);
                    }
                }
                {
                    yield return new ShaderParameter<Vector3>(ShaderParameterKey.EmissiveFactor, new Vector3(parameter.X, parameter.Y, parameter.Z));
                }
                    yield break;

                default:
                    throw new NotImplementedException($"Material channel {materialChannel.Key} is not supported yet");
            }
        }

        private SamplerParameters? TransfromSamplerParameters(MaterialChannel materialChannel, ReaderContext context)
        {
            var materialChannelTexture = materialChannel.Texture;
            if (materialChannelTexture == null)
                return null;
            var primaryImage = materialChannelTexture.PrimaryImage;
            if (primaryImage == null)
                return null;
            return new SamplerParameters
            {
                Image = context.Images[primaryImage.LogicalIndex],
                TextureCoordinate = materialChannel.TextureCoordinate,
                TextureTransform = TransformTextureTransform(materialChannel.TextureTransform)
            };
        }

        private LocalTransform TransformTextureTransform(TextureTransform textureTransform)
        {
            if (textureTransform == null)
                return null;
            return null;
        }

        private AlphaMode GetAlphaMode(SharpGLTF.Schema2.AlphaMode materialAlpha)
        {
            switch (materialAlpha)
            {
                case SharpGLTF.Schema2.AlphaMode.OPAQUE:
                    return AlphaMode.Opaque;
                case SharpGLTF.Schema2.AlphaMode.MASK:
                    return AlphaMode.Mask;
                case SharpGLTF.Schema2.AlphaMode.BLEND:
                    return AlphaMode.Blend;
                default:
                    throw new ArgumentOutOfRangeException(nameof(materialAlpha), materialAlpha, null);
            }
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
                    node.Mesh.Primitives.Select(_ => context.Materials[_.Material.LogicalIndex]).ToList());
            if (node.Camera != null) nodeAsset.Camera = context.Cameras[node.Camera.LogicalIndex];
            if (node.PunctualLight != null) nodeAsset.Light = context.Cameras[node.PunctualLight.LogicalIndex];
            return nodeAsset;
        }
    }
}