using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public class NodeAsset : AbstractAsset, INodeAsset
    {
        public NodeAsset(string id) : base(id)
        {
        }

        public LocalTransform Transform { get; } = new LocalTransform();
    }
}