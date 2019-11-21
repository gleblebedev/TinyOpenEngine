using System;
using System.Collections.Generic;
using System.Numerics;

namespace Toe.SceneGraph
{
    public class Node : NodeContainer
    {
        private readonly NodeComponent _nodeComponent;
        private readonly Scene _scene;
        private Node _parent;

        private WorldMatrixToken _worldMatrixVersion = WorldMatrixToken.Empty;
        private WorldMatrixToken _worldTransformToken;

        public Node(Scene scene, LocalTransform localTransform, WorldTransform worldTransform)
        {
            //TODO: Make Node constructor internal
            _scene = scene;
            _nodeComponent.Node = this;
            Transform = localTransform;
            Transform.OnUpdate += HandleTransformUpdate;
            Transform.Reset();

            WorldTransform = worldTransform;
            WorldTransform.WorldMatrix = Matrix4x4.Identity;

            Add(this, scene);
        }

        public LocalTransform Transform { get; }

        public Node Parent
        {
            get => _parent;
            set
            {
                if (_parent != value)
                {
                    if (value != null)
                    {
                        if (value._scene != _scene)
                            throw new InvalidOperationException(
                                "Can't move Node to a different scene. Please create a new node in the scene.");

                        if (Transform != null && value.Transform == null)
                            throw new InvalidOperationException(
                                "Can't attach node with transform to a parent node with no transform");
                    }

                    if (_parent != null)
                        Remove(this, _parent);
                    else
                        Remove(this, _scene);

                    _parent = value;
                    if (_parent != null)
                        Add(this, _parent);
                    else
                        Add(this, _scene);

                    InvalidateWorldTransform();
                }
            }
        }

        public WorldTransform WorldTransform { get; }

        private void HandleTransformUpdate(object sender, TransformUpdatedArgs e)
        {
            InvalidateWorldTransform();
        }

        private void InvalidateWorldTransform()
        {
            if (Transform != null && _worldTransformToken == WorldMatrixToken.Empty)
                // Don't schedule an update if parent is already scheduled.
                // This could save time on prefab spawn and animations.
                if (_parent == null || _parent._worldTransformToken == WorldMatrixToken.Empty)
                    _worldTransformToken = _scene.EnqueueWorldTransformUpdate(this);
        }

        private void ResetToken()
        {
            _worldTransformToken = WorldMatrixToken.Empty;
        }

        private void UpdateSubtreeWorldTransform(WorldMatrixToken updateToken, Queue<Node> updateQueue)
        {
            EnsureWorldTransformIsUpToDate(updateToken);
            if (HasChildren)
                foreach (var child in Children)
                    updateQueue.Enqueue(child);

            while (updateQueue.Count > 0)
            {
                var n = updateQueue.Dequeue();
                if (n.Transform != null && n._worldTransformToken == WorldMatrixToken.Empty)
                {
                    if (n._worldMatrixVersion != updateToken)
                    {
                        n.WorldTransform.WorldMatrix = n.Transform.Matrix * n._parent.WorldTransform.WorldMatrix;
                        n._worldMatrixVersion = updateToken;
                    }

                    if (n.HasChildren)
                        foreach (var child in n.Children)
                            updateQueue.Enqueue(child);
                }
            }
        }

        public void EvaluateWorldTransform(out Matrix4x4 m)
        {
            if (_parent != null)
            {
                _parent.EvaluateWorldTransform(out var parent);
                if (Transform != null)
                {
                    m = Transform.Matrix * parent;
                    return;
                }

                m = parent;
                return;
            }

            if (Transform != null)
            {
                m = Transform.Matrix;
                return;
            }

            m = Matrix4x4.Identity;
        }

        private void EnsureWorldTransformIsUpToDate(WorldMatrixToken updateToken)
        {
            if (_worldMatrixVersion != updateToken)
            {
                _worldMatrixVersion = updateToken;
                if (_parent == null)
                {
                    WorldTransform.WorldMatrix = Transform.Matrix;
                }
                else
                {
                    _parent.EnsureWorldTransformIsUpToDate(updateToken);
                    WorldTransform.WorldMatrix = Transform.Matrix * _parent.WorldTransform.WorldMatrix;
                }
            }
        }

        public class WorldMatrixUpdateQueue
        {
            private readonly List<Node> _queue = new List<Node>(128);
            private readonly Queue<Node> _updateQueue = new Queue<Node>(128);
            private int _updateCounter;

            public void Update()
            {
                while (_queue.Count != 0)
                {
                    ++_updateCounter;
                    var updateToken = new WorldMatrixToken(_updateCounter);
                    foreach (var node in _queue) node.UpdateSubtreeWorldTransform(updateToken, _updateQueue);
                    foreach (var node in _queue) node.ResetToken();
                    _queue.Clear();
                }
            }

            public WorldMatrixToken Add(Node node)
            {
                _queue.Add(node);
                return new WorldMatrixToken(_queue.Count);
            }
        }
    }
}