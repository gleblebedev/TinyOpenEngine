using System;
using System.Runtime.CompilerServices;

namespace Toe.SceneGraph
{
    public class Scene<TEntity> : NodeContainer<TEntity>, IDisposable
    {
        private readonly Node<TEntity>.WorldMatrixUpdateQueue _worldMatrixUpdateQueue = new Node<TEntity>.WorldMatrixUpdateQueue();

        public void Dispose()
        {
        }

        public WorldMatrixToken EnqueueWorldTransformUpdate(Node<TEntity> node)
        {
            return _worldMatrixUpdateQueue.Add(node);
        }

        public void UpdateWorldTransforms()
        {
            _worldMatrixUpdateQueue.Update();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node<TEntity> CreateNode(Node<TEntity> parent, TEntity entity)
        {
            return CreateNode(parent, entity, new LocalTransform());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node<TEntity> CreateNode(Node<TEntity> parent, TEntity entity, LocalTransform localTransform)
        {
            return new Node<TEntity>(this, localTransform, (localTransform != null)?new WorldTransform():null, entity) { Parent = parent };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Node<TEntity> CreateNodeWithNoTransform(Node<TEntity> parent, TEntity entity)
        {
            return CreateNode(parent, entity, null);
        }
    }
}