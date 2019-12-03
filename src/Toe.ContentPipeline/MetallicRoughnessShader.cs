using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class MetallicRoughnessShader : ShaderAsset
    {
        public override IEnumerable<IShaderParameter> Parameters
        {
            get
            {
                if (MetallicRoughness != null)
                    yield return MetallicRoughness;
                if (BaseColor != null)
                    yield return BaseColor;
                foreach (var shaderParameter in base.Parameters) yield return shaderParameter;
            }
        }

        public IShaderParameter BaseColor { get; set; } = DefaultBaseColor;

        public IShaderParameter MetallicRoughness { get; set; } = DefaultMetallicRoughness;

        public override void Set(IShaderParameter shaderParameter)
        {
            switch (shaderParameter.Key)
            {
                case ShaderParameterKey.MetallicRoughness:
                    MetallicRoughness = shaderParameter;
                    break;
                case ShaderParameterKey.BaseColor:
                    BaseColor = shaderParameter;
                    break;
                default:
                    base.Set(shaderParameter);
                    break;
            }
        }
    }
}