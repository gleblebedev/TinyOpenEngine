using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public struct SamplerParameters
    {
        public IImageAsset Image { get; set; }
        public LocalTransform TextureTransform { get; set; }
        public int TextureCoordinate { get; set; }
    }
}