using System.IO;
using System.Numerics;
using Veldrid;

namespace Toe.ContentPipeline.VeldridMesh
{
    internal class Float3StreamAccessor : StreamAccessor<Vector3>, IStreamAccessor
    {
        public Float3StreamAccessor(StreamKey key, IMeshStream meshStream) : base(key, meshStream, VertexElementFormat.Float3) { }
        public void Write(int index, BinaryWriter stream) { stream.Write(Reader[index]); }
    }
}