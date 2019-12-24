using System.IO;
using System.Numerics;
using Veldrid;

namespace Toe.ContentPipeline.VeldridMesh
{
    internal class Float4StreamAccessor : StreamAccessor<Vector4>, IStreamAccessor
    {
        public Float4StreamAccessor(StreamKey key, IMeshStream meshStream) : base(key, meshStream, VertexElementFormat.Float4) { }
        public void Write(int index, BinaryWriter stream) { stream.Write(Reader[index]); }
    }
}