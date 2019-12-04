using System.Collections.Generic;
using System.Numerics;

namespace Toe.ContentPipeline
{
    public class ShaderAsset : IShaderAsset
    {
        public static readonly IShaderParameter<float> DefaultNormalTextureScale = new ShaderParameter<float>(ShaderParameterKey.NormalTextureScale, 1.0f);

        public static readonly IShaderParameter<float> DefaultOcclusionTextureStrength = new ShaderParameter<float>(ShaderParameterKey.OcclusionTextureStrength, 1.0f);

        public static readonly IShaderParameter<Vector3> DefaultEmissiveFactor = new ShaderParameter<Vector3>(ShaderParameterKey.EmissiveFactor, new Vector3(0.0f, 0.0f, 0.0f));

        //public static readonly IShaderParameter<float> DefaultAlphaCutoff = new ShaderParameter<float>(ShaderParameterKey.AlphaCutoff, 0.5f);

        //PBR Metallic-Roughness                 
        public static readonly IShaderParameter<Vector4> DefaultBaseColorFactor = new ShaderParameter<Vector4>(ShaderParameterKey.BaseColorFactor, new Vector4(1.0f, 1.0f, 1.0f, 1.0f));

        public static readonly IShaderParameter<float> DefaultMetallicFactor = new ShaderParameter<float>(ShaderParameterKey.MetallicFactor, 1.0f);

        public static readonly IShaderParameter<float> DefaultRoughnessFactor = new ShaderParameter<float>(ShaderParameterKey.RoughnessFactor, 1.0f);

        //PBR Specular-Glossiness                
        public static readonly IShaderParameter<Vector4> DefaultDiffuseFactor = new ShaderParameter<Vector4>(ShaderParameterKey.DiffuseFactor, new Vector4(1.0f, 1.0f, 1.0f, 1.0f));

        public static readonly IShaderParameter<Vector3> DefaultSpecularFactor = new ShaderParameter<Vector3>(ShaderParameterKey.SpecularFactor, new Vector3(1.0f, 1.0f, 1.0f));

        public static readonly IShaderParameter<float> DefaultGlossinessFactor = new ShaderParameter<float>(ShaderParameterKey.GlossinessFactor, 1.0f);

        private readonly IDictionary<string, IShaderParameter> _extraParameters = new Dictionary<string, IShaderParameter>();

        public SamplerParameters NormalTexture { get; set; }
        public float NormalTextureScale { get; set; } = DefaultNormalTextureScale.Value;
        public SamplerParameters OcclusionTexture { get; set; }
        public float OcclusionTextureStrength { get; set; } = DefaultOcclusionTextureStrength.Value;
        public SamplerParameters EmissiveTexture { get; set; }
        public Vector3 EmissiveFactor { get; set; } = DefaultEmissiveFactor.Value;
        //public float AlphaCutoff { get; set; } = DefaultAlphaCutoff.Value;

        public virtual IEnumerable<IShaderParameter> Parameters
        {
            get
            {
                if (NormalTexture.Image != null) yield return NormalTexture.AsShaderParameter(ShaderParameterKey.NormalTexture);
                yield return NormalTextureScale.AsShaderParameter(ShaderParameterKey.NormalTextureScale);
                if (OcclusionTexture.Image != null) yield return OcclusionTexture.AsShaderParameter(ShaderParameterKey.OcclusionTexture);
                yield return OcclusionTextureStrength.AsShaderParameter(ShaderParameterKey.OcclusionTextureStrength);
                if (EmissiveTexture.Image != null) yield return EmissiveTexture.AsShaderParameter(ShaderParameterKey.EmissiveTexture);
                yield return EmissiveFactor.AsShaderParameter(ShaderParameterKey.EmissiveFactor);
                //if (AlphaCutoff != null) yield return AlphaCutoff;
                foreach (var shaderParameter in _extraParameters) yield return shaderParameter.Value;
            }
        }

        public virtual void Set(IShaderParameter shaderParameter)
        {
            switch (shaderParameter.Key)
            {
                case ShaderParameterKey.NormalTexture:
                    NormalTexture = ((IShaderParameter<SamplerParameters>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.NormalTextureScale:
                    NormalTextureScale = ((IShaderParameter<float>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.OcclusionTexture:
                    OcclusionTexture = ((IShaderParameter<SamplerParameters>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.OcclusionTextureStrength:
                    OcclusionTextureStrength = ((IShaderParameter<float>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.EmissiveTexture:
                    EmissiveTexture = ((IShaderParameter<SamplerParameters>) shaderParameter).Value;
                    break;
                case ShaderParameterKey.EmissiveFactor:
                    EmissiveFactor = ((IShaderParameter<Vector3>) shaderParameter).Value;
                    break;
                //case ShaderParameterKey.AlphaCutoff:
                //    AlphaCutoff = (IShaderParameter<float>) shaderParameter;
                //    break;
                default:
                    _extraParameters[shaderParameter.Key] = shaderParameter;
                    break;
            }
        }
    }
}