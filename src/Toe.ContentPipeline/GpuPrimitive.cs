using System.Collections;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class GpuPrimitive : AbstractPrimitive, IMeshPrimitive, IReadOnlyList<int>
    {
        readonly IReadOnlyList<int> _indices;
        public GpuPrimitive(IEnumerable<int> indices):this(PrimitiveTopology.TriangleList, indices)
        {
        }
        public GpuPrimitive(IReadOnlyList<int> indices) : this(PrimitiveTopology.TriangleList, indices)
        {
        }
        public GpuPrimitive(PrimitiveTopology topology, IEnumerable<int> indices)
        {
            Topology = topology;
            var list = new List<int>();
            list.AddRange(indices);
            _indices = list;
        }
        public GpuPrimitive(PrimitiveTopology topology, IReadOnlyList<int> indices)
        {
            Topology = topology;
            _indices = indices;
        }
        public IEnumerator<int> GetEnumerator()
        {
            return _indices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _indices.Count; }
        }

        public int this[int index] => _indices[index];
    }
}