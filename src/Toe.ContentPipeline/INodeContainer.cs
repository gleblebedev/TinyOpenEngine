using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface INodeContainer
    {
        IReadOnlyCollection<INodeAsset> ChildNodes { get; }
        bool HasChildren { get; }
    }
}