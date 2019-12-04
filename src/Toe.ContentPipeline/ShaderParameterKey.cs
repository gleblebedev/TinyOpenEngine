namespace Toe.ContentPipeline
{
    public static class ShaderParameterKey
    {
        public const string NormalTexture = "NormalTexture";
        public const string NormalTextureScale = "NormalTextureScale";
        public const string OcclusionTexture = "OcclusionTexture";
        public const string OcclusionTextureStrength = "OcclusionTextureStrength";
        public const string EmissiveTexture = "EmissiveTexture";
        public const string EmissiveFactor = "EmissiveFactor";

        //public const string AlphaCutoff = "AlphaCutoff";

        //PBR Metallic-Roughness
        public const string BaseColorFactor = "BaseColorFactor";
        public const string BaseColorTexture = "BaseColorTexture";
        public const string MetallicFactor = "MetallicFactor";
        public const string RoughnessFactor = "RoughnessFactor";

        public const string MetallicRoughnessTexture = "MetallicRoughnessTexture";

        //PBR Specular-Glossiness
        public const string DiffuseFactor = "DiffuseFactor";
        public const string DiffuseTexture = "DiffuseTexture";
        public const string SpecularFactor = "SpecularFactor";
        public const string GlossinessFactor = "GlossinessFactor";
        public const string SpecularGlossinessTexture = "SpecularGlossinessTexture";
    }
}