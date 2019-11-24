using System.Collections.Generic;

namespace Toe.SceneGraph
{
    public interface INodeContainer<TEntity>
    {
        IReadOnlyCollection<Node<TEntity>> Children { get; }

        bool HasChildren { get; }
    }
}