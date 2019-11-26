using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public interface INodeAsset : INodeContainer, IAsset
    {
        LocalTransform Transform { get; }

        INodeAsset Parent { get; set; }

        IMeshInstance Mesh { get; set; }
    }
}