using System.Collections;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class GpuPrimitive : AbstractPrimitive, IMeshPrimitive, IReadOnlyList<int>
    {
        private readonly IReadOnlyList<int> _indices;

        public GpuPrimitive(IEnumerable<int> indices, IBufferView bufferView) : this(PrimitiveTopology.TriangleList,
            indices, bufferView)
        {
        }

        public GpuPrimitive(IReadOnlyList<int> indices, IBufferView bufferView) : this(PrimitiveTopology.TriangleList,
            indices, bufferView)
        {
        }

        public GpuPrimitive(PrimitiveTopology topology, IEnumerable<int> indices, IBufferView bufferView) : base(
            bufferView)
        {
            Topology = topology;
            var list = new List<int>();
            list.AddRange(indices);
            _indices = list;
        }

        public GpuPrimitive(PrimitiveTopology topology, IReadOnlyList<int> indices, IBufferView bufferView) :
            base(bufferView)
        {
            Topology = topology;
            _indices = indices;
        }

        public override IReadOnlyList<int> GetIndexReader(StreamKey key)
        {
            return _indices;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _indices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _indices.Count;

        public int this[int index] => _indices[index];
    }
}