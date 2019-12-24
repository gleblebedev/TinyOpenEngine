using System.IO;
using System.Numerics;
using Veldrid;

namespace Toe.ContentPipeline.VeldridMesh
{
    internal class Float2StreamAccessor : StreamAccessor<Vector2>, IStreamAccessor
    {
        public Float2StreamAccessor(StreamKey key, IMeshStream meshStream) : base(key, meshStream, VertexElementFormat.Float2) { }
        public void Write(int index, BinaryWriter stream) { stream.Write(Reader[index]); }
    }
}