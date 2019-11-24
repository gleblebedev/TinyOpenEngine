using System;
using System.Collections.Generic;
using System.Threading;

namespace Toe.SceneGraph
{
    public abstract class NodeContainer<TEntity> : INodeContainer<TEntity>
    {
        private readonly Lazy<HashSet<Node<TEntity>>> _children = new Lazy<HashSet<Node<TEntity>>>(LazyThreadSafetyMode.PublicationOnly);

        public IReadOnlyCollection<Node<TEntity>> Children => _children.Value;

        public bool HasChildren => _children.IsValueCreated && _children.Value.Count != 0;

        protected void AddTo(Node<TEntity> node, NodeContainer<TEntity> container)
        {
            container._children.Value.Add(node);
        }

        protected void RemoveFrom(Node<TEntity> node, NodeContainer<TEntity> container)
        {
            container._children.Value.Remove(node);
        }
    }
}