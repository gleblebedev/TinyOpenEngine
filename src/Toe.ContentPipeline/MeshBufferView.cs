using System;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class MeshBufferView : IBufferView
    {
        protected readonly Dictionary<StreamKey, IMeshStream> _availableStreams =
            new Dictionary<StreamKey, IMeshStream>();

        public IMeshStream GetStream(StreamKey key)
        {
            IMeshStream list;
            if (_availableStreams.TryGetValue(key, out list)) return list;
            return null;
        }

        public IReadOnlyCollection<StreamKey> GetStreams()
        {
            return _availableStreams.Keys;
        }

        public bool HasStream(StreamKey key)
        {
            return GetStream(key) != null;
        }

        public T SetStream<T>(StreamKey key, T stream) where T : class, IMeshStream
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "Stream can't be null");
            _availableStreams[key] = stream;
            return stream;
        }

        public bool DeleteStream(StreamKey key)
        {
            return _availableStreams.Remove(key);
        }
    }
}