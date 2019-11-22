using System;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class GpuMesh : AbstractMesh<GpuPrimitive>, IMesh
    {
        public GpuMesh(string id) : base(id)
        {
        }

        public GpuMesh()
        {
        }

        public List<GpuPrimitive> Primitives => _primitives;
        IList<IMeshPrimitive> IMesh.Primitives => _abstractPrimitives;

        public static GpuMesh Optimize(IMesh source)
        {
            throw new NotImplementedException();
        }
    }
}