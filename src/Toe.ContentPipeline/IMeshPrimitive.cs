using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IMeshPrimitive
    {
        PrimitiveTopology Topology { get; }

        IBufferView BufferView { get; }

        IReadOnlyList<int> GetIndexReader(StreamKey key);

        bool DeleteStream(StreamKey key);
    }
}