using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public interface INodeAsset : IAsset
    {
        LocalTransform Transform { get; }

        INodeAsset Parent { get; set; }

        IMeshInstance Mesh { get; set; }
    }
}