using Veldrid;

namespace Toe.ContentPipeline.VeldridMesh
{
    public struct VeldridPrimitive
    {
        public Veldrid.PrimitiveTopology PrimitiveTopology;
        public VertexLayoutDescription VertexLayout;
        public uint VertexBufferOffset;
        public uint IndexBufferOffset;
    }
}