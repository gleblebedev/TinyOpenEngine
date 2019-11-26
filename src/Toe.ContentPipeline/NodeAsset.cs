using System;
using System.Collections.Generic;
using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public class NodeAsset : AbstractAsset, INodeAsset
    {
        private Node<INodeAsset> _graphNode;

        public NodeAsset(string id) : base(id)
        {
            Transform = new LocalTransform();
        }

        public Node<INodeAsset> GraphNode
        {
            get { return _graphNode; }
            internal set
            {
                _graphNode = value;
                if (_graphNode != null)
                {
                    ChildNodes = new NodeContainerAdapter<INodeAsset>(_graphNode);
                }
                else
                {
                    ChildNodes = null;
                }
            }
        }

        public IReadOnlyCollection<INodeAsset> ChildNodes { get; private set; }

        public LocalTransform Transform { get; }

        public INodeAsset Parent
        {
            get => GraphNode?.Parent?.Entity;
            set
            {
                if (GraphNode == null)
                {
                    if (value == null)
                        return;
                    var nodeAsset = (NodeAsset) value;
                    if (nodeAsset.GraphNode == null)
                        throw new InvalidOperationException("Parent node is not attached to scene yet");
                    GraphNode = nodeAsset.GraphNode.Scene.CreateNode(nodeAsset.GraphNode, this);
                }
                else
                {
                    if (value == null)
                    {
                        GraphNode.Parent = null;
                    }
                    else
                    {
                        var nodeAsset = (NodeAsset) value;
                        if (nodeAsset.GraphNode == null)
                            throw new InvalidOperationException("Parent node is not attached to scene yet");
                        GraphNode.Parent = nodeAsset.GraphNode;
                    }
                }
            }
        }

        public IMeshInstance Mesh { get; set; }

        public bool HasChildren
        {
            get
            {
                if (GraphNode == null)
                    return false;
                return GraphNode.HasChildren;
            }
        }

        public ICameraAsset Camera { get; set; }
        public ICameraAsset Light { get; set; }
    }
}