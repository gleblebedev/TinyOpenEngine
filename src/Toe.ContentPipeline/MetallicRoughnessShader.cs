using System.Collections.Generic;
using System.Numerics;

namespace Toe.ContentPipeline
{
    public class MetallicRoughnessShader : ShaderAsset
    {
        //PBR Metallic-Roughness                 
        public Vector4 BaseColorFactor { get; set; } = DefaultBaseColorFactor.Value;
        public SamplerParameters BaseColorTexture { get; set; }
        public float MetallicFactor { get; set; } = DefaultMetallicFactor.Value;
        public float RoughnessFactor { get; set; } = DefaultRoughnessFactor.Value;
        public SamplerParameters MetallicRoughnessTexture { get; set; }

        public override IEnumerable<IShaderParameter> Parameters
        {
            get
            {
                yield return BaseColorFactor.AsShaderParameter(ShaderParameterKey.BaseColorFactor);
                if (BaseColorTexture.Image != null)
                    yield return BaseColorTexture.AsShaderParameter(ShaderParameterKey.BaseColorTexture);
                yield return MetallicFactor.AsShaderParameter(ShaderParameterKey.MetallicFactor);
                yield return RoughnessFactor.AsShaderParameter(ShaderParameterKey.RoughnessFactor);
                if (MetallicRoughnessTexture.Image != null)
                    yield return
                        MetallicRoughnessTexture.AsShaderParameter(ShaderParameterKey.MetallicRoughnessTexture);
                foreach (var shaderParameter in base.Parameters) yield return shaderParameter;
            }
        }

        public override void Set(IShaderParameter shaderParameter)
        {
            switch (shaderParameter.Key)
            {
                case ShaderParameterKey.BaseColorFactor:
                    BaseColorFactor = ((IShaderParameter<Vector4>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.BaseColorTexture:
                    BaseColorTexture = ((IShaderParameter<SamplerParameters>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.MetallicFactor:
                    MetallicFactor = ((IShaderParameter<float>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.RoughnessFactor:
                    RoughnessFactor = ((IShaderParameter<float>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.MetallicRoughnessTexture:
                    MetallicRoughnessTexture = ((IShaderParameter<SamplerParameters>) shaderParameter).Value;
                    break;
                default:
                    base.Set(shaderParameter);
                    break;
            }
        }
    }
}