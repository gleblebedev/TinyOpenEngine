using System.Collections.Generic;
using System.Numerics;

namespace Toe.ContentPipeline
{
    public class SpecularGlossinessShader : ShaderAsset
    {
        public Vector4 DiffuseFactor { get; set; } = DefaultDiffuseFactor.Value;
        public SamplerParameters DiffuseTexture { get; set; }
        public Vector3 SpecularFactor { get; set; } = DefaultSpecularFactor.Value;
        public float GlossinessFactor { get; set; } = DefaultGlossinessFactor.Value;
        public SamplerParameters SpecularGlossinessTexture { get; set; }

        public override IEnumerable<IShaderParameter> Parameters
        {
            get
            {
                yield return DiffuseFactor.AsShaderParameter(ShaderParameterKey.DiffuseFactor);
                if (DiffuseTexture.Image != null) yield return DiffuseTexture.AsShaderParameter(ShaderParameterKey.DiffuseTexture);
                yield return SpecularFactor.AsShaderParameter(ShaderParameterKey.SpecularFactor);
                yield return GlossinessFactor.AsShaderParameter(ShaderParameterKey.GlossinessFactor);
                if (SpecularGlossinessTexture.Image != null) yield return SpecularGlossinessTexture.AsShaderParameter(ShaderParameterKey.SpecularGlossinessTexture);
                foreach (var shaderParameter in base.Parameters) yield return shaderParameter;
            }
        }

        public override void Set(IShaderParameter shaderParameter)
        {
            switch (shaderParameter.Key)
            {
                case ShaderParameterKey.DiffuseFactor:
                    DiffuseFactor = ((IShaderParameter<Vector4>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.DiffuseTexture:
                    DiffuseTexture = ((IShaderParameter<SamplerParameters>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.SpecularFactor:
                    SpecularFactor = ((IShaderParameter<Vector3>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.GlossinessFactor:
                    GlossinessFactor = ((IShaderParameter<float>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.SpecularGlossinessTexture:
                    SpecularGlossinessTexture = ((IShaderParameter<SamplerParameters>) shaderParameter).Value;
                    break;
                default:
                    base.Set(shaderParameter);
                    break;
            }
        }
    }
}