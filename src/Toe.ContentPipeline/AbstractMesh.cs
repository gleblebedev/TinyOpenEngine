using System;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public abstract class AbstractMesh<P> : AbstractAsset where P : IMeshPrimitive
    {
        protected readonly IList<IMeshPrimitive> _abstractPrimitives;
        protected readonly List<P> _primitives;

        protected readonly Dictionary<StreamKey, IMeshStream> availableStreams =
            new Dictionary<StreamKey, IMeshStream>();

        public AbstractMesh(string id) : base(id)
        {
            _primitives = new List<P>();
            _abstractPrimitives = new ListProxy<IMeshPrimitive, P>(_primitives);
        }

        public AbstractMesh() : this(null)
        {
        }

        public IMeshStream GetStream(StreamKey key)
        {
            IMeshStream list;
            if (availableStreams.TryGetValue(key, out list)) return list;
            return null;
        }

        public IEnumerable<StreamKey> GetStreams()
        {
            return availableStreams.Keys;
        }

        public bool HasStream(StreamKey key)
        {
            return GetStream(key) != null;
        }

        public T SetStream<T>(StreamKey key, T stream) where T : class, IMeshStream
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "Stream can't be null");
            availableStreams[key] = stream;
            return stream;
        }

        public virtual bool DeleteStream(StreamKey key)
        {
            return availableStreams.Remove(key);
        }
    }
}