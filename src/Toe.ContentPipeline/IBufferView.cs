using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IBufferView
    {
        IEnumerable<StreamKey> GetStreams();

        bool HasStream(StreamKey key);

        IMeshStream GetStream(StreamKey key);

        T SetStream<T>(StreamKey key, T stream) where T : class, IMeshStream;
        bool DeleteStream(StreamKey key);
    }
}