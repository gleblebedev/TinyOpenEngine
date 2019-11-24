using System;
using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public class NodeAsset : AbstractAsset, INodeAsset
    {
        public NodeAsset(string id) : base(id)
        {
            Transform = new LocalTransform();
        }

        public Node<INodeAsset> GraphNode { get; internal set; }

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
        public ICameraAsset Camera { get; set; }
        public ICameraAsset Light { get; set; }
    }
}