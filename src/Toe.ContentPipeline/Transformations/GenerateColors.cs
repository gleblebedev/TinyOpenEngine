using System;
using System.Collections.Generic;
using System.Numerics;

namespace Toe.ContentPipeline.Transformations
{
    public class GenerateColors : IMeshTransformation
    {
        private readonly Vector4 _color;

        public GenerateColors(Vector4 color)
        {
            _color = color;
        }

        public GenerateColors() : this(Vector4.One)
        {
        }

        public IEnumerable<IMesh> Apply(IMesh geometry)
        {
            var indexedMesh = geometry as IndexedMesh;
            if (indexedMesh != null) return PaintSeparateStreamMesh(indexedMesh);

            var gpuMesh = geometry as GpuMesh;
            if (gpuMesh != null) return PaintSingleStreamMesh(gpuMesh);
            return PaintSingleStreamMesh(geometry.ToGpuMesh());
        }

        private IEnumerable<IMesh> PaintSingleStreamMesh(GpuMesh mesh)
        {
            foreach (var bufferAndPrimitives in mesh.GroupPrimitives())
            {
                var bufferView = bufferAndPrimitives.BufferView;
                if (bufferView.GetStream(StreamKey.Color) != null) continue;
                var meshStream = bufferView.GetStream(StreamKey.Position);
                var streamConverterFactory = meshStream.ConverterFactory;
                var colors = new Vector4[meshStream.Count];
                new Span<Vector4>(colors).Fill(_color);
                var arrayMeshStream = new ArrayMeshStream<Vector4>(colors, streamConverterFactory);
                bufferView.SetStream(StreamKey.Color, arrayMeshStream);
            }

            yield return mesh;
        }

        private IEnumerable<IMesh> PaintSeparateStreamMesh(IndexedMesh mesh)
        {
            foreach (var bufferAndPrimitives in mesh.GroupPrimitives())
            {
                var bufferView = bufferAndPrimitives.BufferView;
                if (bufferView.GetStream(StreamKey.Color) != null) continue;
                var streamConverterFactory = bufferView.GetStream(StreamKey.Position).ConverterFactory;
                bufferView.SetStream(StreamKey.Color,
                    new ArrayMeshStream<Vector4>(new[] {_color}, streamConverterFactory));
                foreach (var primitiveAndIndex in bufferAndPrimitives.Primitives)
                {
                    var submesh = primitiveAndIndex.Primitive;
                    var positionIndices = submesh.GetIndexReader(StreamKey.Position);
                    var indices = new int[positionIndices.Count];
                    submesh.SetIndexStream(StreamKey.Color, indices);
                }
            }

            yield return mesh;
        }
    }
}