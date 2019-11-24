using System.Collections;
using System.Collections.Generic;
using Toe.SceneGraph;

namespace Toe.ContentPipeline
{
    public interface ISceneAsset : IAsset
    {
        IReadOnlyCollection<INodeAsset> ChildNodes { get; }
    }
}