using System.Numerics;
using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public interface IShaderParameter
    {
        string Key { get; }
        Vector4 Value { get; }
        IImageAsset Image { get; }
        LocalTransform TextureTransform { get; }
        int TextureCoordinate { get; }
    }
}