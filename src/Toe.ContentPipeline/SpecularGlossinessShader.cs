using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class SpecularGlossinessShader : ShaderAsset
    {
        public override IEnumerable<IShaderParameter> Parameters
        {
            get
            {
                if (SpecularGlossiness != null)
                    yield return SpecularGlossiness;
                if (Diffuse != null)
                    yield return Diffuse;
                foreach (var shaderParameter in base.Parameters) yield return shaderParameter;
            }
        }

        public IShaderParameter Diffuse { get; set; } = DefaultDiffuse;

        public IShaderParameter SpecularGlossiness { get; set; } = DefaultSpecularGlossiness;

        public override void Set(IShaderParameter shaderParameter)
        {
            switch (shaderParameter.Key)
            {
                case ShaderParameterKey.SpecularGlossiness:
                    SpecularGlossiness = shaderParameter;
                    break;
                case ShaderParameterKey.Diffuse:
                    Diffuse = shaderParameter;
                    break;
                default:
                    base.Set(shaderParameter);
                    break;
            }
        }
    }
}