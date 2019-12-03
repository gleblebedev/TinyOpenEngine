namespace Toe.ContentPipeline
{
    public interface IMaterialAsset : IAsset
    {
        float AlphaCutoff { get; }
        AlphaMode Alpha { get; }
        bool DoubleSided { get; }

        IShaderAsset Shader { get; }
        bool Unlit { get; }
    }
}