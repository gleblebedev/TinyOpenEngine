using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline
{
    public abstract class AbstractMesh<P> : AbstractAsset where P:IMeshPrimitive
    {
        protected readonly IList<IMeshPrimitive> _abstractPrimitives;
        protected readonly List<P> _primitives;


        public AbstractMesh(string id) : base(id)
        {
            _primitives = new List<P>(1);
            _abstractPrimitives = new ListProxy<IMeshPrimitive, P>(_primitives);
        }

        public AbstractMesh() : this(null)
        {
        }

        public IEnumerable<IBufferView> BufferViews
        {
            get
            {
                return _primitives.Select(_ => _.BufferView).Distinct();
            }
        }

        public bool DeleteStream(StreamKey key)
        {
            var res = false;
            foreach (var primitive in _primitives)
            {
                res |= primitive.DeleteStream(key);
            }
            foreach (var primitive in _primitives)
            {
                res |= primitive.BufferView.DeleteStream(key);
            }

            return res;
        }
    }
}