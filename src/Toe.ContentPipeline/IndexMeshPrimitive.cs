using System;

namespace Toe.ContentPipeline
{
    public class IndexMeshPrimitive : AbstractPrimitive, IMeshPrimitive
    {
        public IndexMeshPrimitive(IBufferView bufferView) : base(bufferView)
        {
        }
    }
}