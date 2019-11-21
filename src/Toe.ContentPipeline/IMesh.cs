using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IMesh : IAsset
    {
        IList<IMeshPrimitive> Primitives { get; }

        IEnumerable<StreamKey> GetStreams();
        bool HasStream(StreamKey key);

        IMeshStream GetStream(StreamKey key);

        T SetStream<T>(StreamKey key, T stream) where T : class, IMeshStream;

        bool DeleteStream(StreamKey key);
    }
}