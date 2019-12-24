using System.IO;
using Veldrid;

namespace Toe.ContentPipeline.VeldridMesh
{
    internal class Float1StreamAccessor : StreamAccessor<float>, IStreamAccessor
    {
        public Float1StreamAccessor(StreamKey key, IMeshStream meshStream) : base(key, meshStream, VertexElementFormat.Float1) { }
        public void Write(int index, BinaryWriter stream) { stream.Write(Reader[index]); }
    }
}