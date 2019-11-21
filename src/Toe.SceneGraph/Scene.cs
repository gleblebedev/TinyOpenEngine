using System;

namespace Toe.SceneGraph
{
    public class Scene : NodeContainer, IDisposable
    {
        private readonly Node.WorldMatrixUpdateQueue _worldMatrixUpdateQueue = new Node.WorldMatrixUpdateQueue();

        public void Dispose()
        {
        }

        public WorldMatrixToken EnqueueWorldTransformUpdate(Node node)
        {
            return _worldMatrixUpdateQueue.Add(node);
        }

        public void UpdateWorldTransforms()
        {
            _worldMatrixUpdateQueue.Update();
        }

        public Node CreateNode(Node parent)
        {
            return new Node(this, new LocalTransform(), new WorldTransform()) {Parent = parent};
        }

        public Node CreateNodeWithNoTransform(Node parent)
        {
            return new Node(this, null, null) {Parent = parent};
        }
    }
}