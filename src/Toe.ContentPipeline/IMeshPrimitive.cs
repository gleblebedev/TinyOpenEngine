using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IMeshPrimitive
    {
        PrimitiveTopology Topology { get; }
    }
}