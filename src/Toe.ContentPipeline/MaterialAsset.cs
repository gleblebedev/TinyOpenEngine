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

        public float AlphaCutoff { get; set; }
        public AlphaMode Alpha { get; set; }
        public bool DoubleSided { get; set; }
        public IShaderAsset Shader { get; set; }
        public bool Unlit { get; set; }
    }
}