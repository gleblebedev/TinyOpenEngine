using System.IO;
using Veldrid;

namespace Toe.ContentPipeline.VeldridMesh
{
    internal interface IStreamAccessor
    {
        VertexElementFormat VertexElementFormat { get; }
        VertexElementSemantic VertexElementSemantic { get; }
        int Count { get; }
        void Write(int index, BinaryWriter stream);
    }
}