using System;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public abstract class IndexedMesh : AbstractMesh<IndexMeshPrimitive>, IMesh
    {
        public IndexedMesh(string id) : base(id)
        {
        }

        public IList<IndexMeshPrimitive> Primitives => _primitives;
        IList<IMeshPrimitive> IMesh.Primitives => _abstractPrimitives;

        public override bool DeleteStream(StreamKey key)
        {
            var res = base.DeleteStream(key);
            {
                foreach (var primitive in _primitives) res |= primitive.DeleteStream(key);
            }
            return res;
        }

        public static IndexedMesh Optimize(IMesh source)
        {
            throw new NotImplementedException();
        }
    }
}