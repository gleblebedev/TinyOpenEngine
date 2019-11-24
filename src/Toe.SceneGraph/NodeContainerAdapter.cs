using System.Collections;
using System.Collections.Generic;

namespace Toe.SceneGraph
{
    public class NodeContainerAdapter<TEntity>: IReadOnlyCollection<TEntity>
    {
        private readonly INodeContainer<TEntity> _nodes;

        public NodeContainerAdapter(INodeContainer<TEntity> nodes)
        {
            _nodes = nodes;
        }
        public IEnumerator<TEntity> GetEnumerator()
        {
            if (!_nodes.HasChildren)
            {
                yield break;
            }

            foreach (var node in _nodes.Children)
            {
                yield return node.Entity;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _nodes.Children.Count; }
        }
    }
}