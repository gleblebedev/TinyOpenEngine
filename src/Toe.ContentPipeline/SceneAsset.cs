using System;
using System.Collections.Generic;
using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public class SceneAsset : AbstractAsset, ISceneAsset
    {
        private readonly Scene<INodeAsset> _sceneGraph;

        public SceneAsset(string id) : base(id)
        {
            _sceneGraph = new Scene<INodeAsset>();
            ChildNodes = new NodeContainerAdapter<INodeAsset>(_sceneGraph);
        }

        public void Add(NodeAsset node)
        {
            if (node.GraphNode != null)
            {
                if (node.GraphNode.Scene == _sceneGraph)
                    return;
                throw new InvalidOperationException($"Node {node.Id} is already attached to a scene.");
            }
            node.GraphNode = _sceneGraph.CreateNode(null, node, node.Transform);
        }

        public IReadOnlyCollection<INodeAsset> ChildNodes { get; }
    }
}