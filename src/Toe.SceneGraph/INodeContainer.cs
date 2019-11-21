using System.Collections.Generic;

namespace Toe.SceneGraph
{
    public interface INodeContainer
    {
        IReadOnlyCollection<Node> Children { get; }

        bool HasChildren { get; }
    }
}