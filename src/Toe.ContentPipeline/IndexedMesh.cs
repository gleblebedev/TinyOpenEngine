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

        public static IndexedMesh Optimize(IMesh source)
        {
            throw new NotImplementedException();
        }
    }
}