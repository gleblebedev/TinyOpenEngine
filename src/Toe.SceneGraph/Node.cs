using System;
using System.Collections.Generic;
using System.Numerics;

namespace Toe.SceneGraph
{
    public class Node<TEntity> : NodeContainer<TEntity>
    {
        private readonly Scene<TEntity> _scene;
        private readonly TEntity _entity;
        private Node<TEntity> _parent;

        private WorldMatrixToken _worldMatrixVersion = WorldMatrixToken.Empty;
        private WorldMatrixToken _worldTransformToken;

        public Node(Scene<TEntity> scene, LocalTransform localTransform, WorldTransform worldTransform, TEntity entity)
        {
            //TODO: Make Node constructor internal
            _scene = scene;
            _entity = entity;
            Transform = localTransform;
            Transform.OnUpdate += HandleTransformUpdate;

            WorldTransform = worldTransform;
            WorldTransform.WorldMatrix = Matrix4x4.Identity;

            AddTo(this, scene);

            InvalidateWorldTransform();
        }

        public LocalTransform Transform { get; }

        public Scene<TEntity> Scene
        {
            get { return _scene; }
        }

        public TEntity Entity
        {
            get { return _entity; }
        }

        public Node<TEntity> Parent
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
                        RemoveFrom(this, _parent);
                    else
                        RemoveFrom(this, _scene);

                    _parent = value;
                    if (_parent != null)
                        AddTo(this, _parent);
                    else
                        AddTo(this, _scene);

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

        private void UpdateSubtreeWorldTransform(WorldMatrixToken updateToken, Queue<Node<TEntity>> updateQueue)
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
            private readonly List<Node<TEntity>> _queue = new List<Node<TEntity>>(128);
            private readonly Queue<Node<TEntity>> _updateQueue = new Queue<Node<TEntity>>(128);
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

            public WorldMatrixToken Add(Node<TEntity> node)
            {
                _queue.Add(node);
                return new WorldMatrixToken(_queue.Count);
            }
        }
    }
}