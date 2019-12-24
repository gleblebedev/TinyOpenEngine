using System.Collections.Generic;
using Veldrid;

namespace Toe.ContentPipeline.VeldridMesh
{
    internal class StreamAccessor<T>
    {
        private readonly IMeshStream _meshStream;
        protected IReadOnlyList<T> Reader { get; }
        public VertexElementSemantic VertexElementSemantic { get; }
        public VertexElementFormat VertexElementFormat
        {
            get;
        }
        public int Count
        {
            get { return _meshStream.Count; }
        }

        public StreamAccessor(StreamKey key, IMeshStream meshStream, VertexElementFormat elementFormat)
        {
            _meshStream = meshStream;
            Reader = meshStream.GetReader<T>();
            VertexElementFormat = elementFormat;
            switch (key.Key)
            {
                case Streams.Position:
                    VertexElementSemantic = VertexElementSemantic.Position;
                    break;
                case Streams.Normal:
                    VertexElementSemantic = VertexElementSemantic.Normal;
                    break;
                case Streams.Color:
                    VertexElementSemantic = VertexElementSemantic.Color;
                    break;
                default:
                    VertexElementSemantic = VertexElementSemantic.TextureCoordinate;
                    break;
            }
        }
    }
}