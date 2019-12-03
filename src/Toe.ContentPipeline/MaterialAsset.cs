namespace Toe.ContentPipeline
{
    public class MaterialAsset : AbstractAsset, IMaterialAsset
    {
        public MaterialAsset(string id) : base(id)
        {
        }

        public MaterialAsset()
        {
        }

        public float AlphaCutoff { get; set; } = 0.5f;
        public AlphaMode Alpha { get; set; } = AlphaMode.Opaque;
        public bool DoubleSided { get; set; } = false;
        public IShaderAsset Shader { get; set; }
        public bool Unlit { get; set; } = false;
    }
}