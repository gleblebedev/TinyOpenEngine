using System.Numerics;
using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public class ShaderParameter : IShaderParameter
    {
        public ShaderParameter(string key)
        {
            Key = key;
        }

        public string Key { get; }
        public Vector4 Value { get; set; }
        public IImageAsset Image { get; set; }
        public LocalTransform TextureTransform { get; set; }
        public int TextureCoordinate { get; set; }

        public override string ToString()
        {
            return Key + ":" + (Image != null ? Image.ToString() : Value.ToString());
        }
    }
}