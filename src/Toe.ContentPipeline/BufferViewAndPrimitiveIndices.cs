using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline
{
    public class BufferViewAndPrimitiveIndices<T>:IEnumerable<T>
    {
        public IBufferView BufferView { get; set; }

        public IList<PrimitiveAndIndex<T>> Primitives { get; } = new List<PrimitiveAndIndex<T>>();
        public IEnumerator<T> GetEnumerator()
        {
            return Primitives.Select(_=>_.Primitive).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}