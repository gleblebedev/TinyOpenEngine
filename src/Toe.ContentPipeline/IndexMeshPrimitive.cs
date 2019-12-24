using System;
using System.Collections.Generic;
using System.Numerics;

namespace Toe.ContentPipeline
{
    public class IndexMeshPrimitive : AbstractPrimitive, IMeshPrimitive
    {
        private readonly Dictionary<StreamKey, IReadOnlyList<int>> _streams =
            new Dictionary<StreamKey, IReadOnlyList<int>>();

        public IndexMeshPrimitive(IBufferView bufferView) : base(bufferView)
        {
        }

        public override IReadOnlyList<int> GetIndexReader(StreamKey key)
        {
            return !_streams.TryGetValue(key, out var streamReader) ? null : streamReader;
        }

        public override bool DeleteStream(StreamKey key)
        {
            return _streams.Remove(key);
        }

        public BoundingBox3 CalculateActualBounds()
        {
            var positions = BufferView.GetStreamReader<Vector3>(StreamKey.Position);
            var boundingBoxMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            var boundingBoxMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            foreach (var index in GetIndexReader(StreamKey.Position))
            {
                var position = positions[index];
                if (boundingBoxMax.X < position.X) boundingBoxMax.X = position.X;
                if (boundingBoxMax.Y < position.Y) boundingBoxMax.Y = position.Y;
                if (boundingBoxMax.Z < position.Z) boundingBoxMax.Z = position.Z;
                if (boundingBoxMin.X > position.X) boundingBoxMin.X = position.X;
                if (boundingBoxMin.Y > position.Y) boundingBoxMin.Y = position.Y;
                if (boundingBoxMin.Z > position.Z) boundingBoxMin.Z = position.Z;
            }

            return new BoundingBox3(boundingBoxMin, boundingBoxMax);
        }

        public T SetIndexStream<T>(StreamKey key, T stream) where T : IReadOnlyList<int>
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream), "Stream can't be null");
            _streams[key] = stream;
            return stream;
        }
    }
}