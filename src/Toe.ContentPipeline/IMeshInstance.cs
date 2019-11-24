using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IMeshInstance
    {
        IMesh Mesh { get; }
        IList<IMaterialAsset> Materials { get; }
    }
}