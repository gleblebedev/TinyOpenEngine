using System.Collections.Generic;
using System.Numerics;

namespace Toe.ContentPipeline
{
    public class ShaderAsset : IShaderAsset
    {
        public static readonly IShaderParameter DefaultBaseColor = new ShaderParameter(ShaderParameterKey.BaseColor)
            {Value = new Vector4(1, 1, 1, 1)};

        public static readonly IShaderParameter DefaultMetallicRoughness =
            new ShaderParameter(ShaderParameterKey.MetallicRoughness) {Value = new Vector4(1, 1, 0, 0)};

        public static readonly IShaderParameter DefaultDiffuse = new ShaderParameter(ShaderParameterKey.Diffuse)
            {Value = new Vector4(1, 1, 1, 1)};

        public static readonly IShaderParameter DefaultSpecularGlossiness =
            new ShaderParameter(ShaderParameterKey.SpecularGlossiness) {Value = new Vector4(1, 1, 1, 1)};

        public static readonly IShaderParameter DefaultNormal = new ShaderParameter(ShaderParameterKey.Normal)
            {Value = new Vector4(1, 0, 0, 0)};

        public static readonly IShaderParameter DefaultOcclusion = new ShaderParameter(ShaderParameterKey.Occlusion)
            {Value = new Vector4(1, 0, 0, 0)};

        public static readonly IShaderParameter DefaultEmissive = new ShaderParameter(ShaderParameterKey.Emissive)
            {Value = new Vector4(0, 0, 0, 1)};

        protected Dictionary<string, IShaderParameter> _extraParameters = new Dictionary<string, IShaderParameter>();

        public IShaderParameter Emissive { get; set; } = DefaultEmissive;

        public IShaderParameter Occlusion { get; set; } = DefaultOcclusion;

        public IShaderParameter Normal { get; set; } = DefaultNormal;

        public virtual IEnumerable<IShaderParameter> Parameters
        {
            get
            {
                if (Emissive != null)
                    yield return Emissive;
                if (Occlusion != null)
                    yield return Occlusion;
                if (Normal != null)
                    yield return Normal;
                foreach (var shaderParameter in _extraParameters) yield return shaderParameter.Value;
            }
        }

        public virtual void Set(IShaderParameter shaderParameter)
        {
            switch (shaderParameter.Key)
            {
                case ShaderParameterKey.Emissive:
                    Emissive = shaderParameter;
                    break;
                case ShaderParameterKey.Occlusion:
                    Occlusion = shaderParameter;
                    break;
                case ShaderParameterKey.Normal:
                    Normal = shaderParameter;
                    break;
                default:
                    _extraParameters[shaderParameter.Key] = shaderParameter;
                    break;
            }
        }
    }
}