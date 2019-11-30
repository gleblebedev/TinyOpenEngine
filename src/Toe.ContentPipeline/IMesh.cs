using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IMesh : IAsset
    {
        IList<IMeshPrimitive> Primitives { get; }

        IEnumerable<IBufferView> BufferViews { get; }

        bool DeleteStream(StreamKey key);
    }
}