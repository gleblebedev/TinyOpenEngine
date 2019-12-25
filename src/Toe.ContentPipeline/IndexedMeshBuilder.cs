using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Toe.ContentPipeline
{
    public class IndexedMeshBuilder
    {
        IndexedMesh _mesh = new IndexedMesh();
        MeshBufferView _currentBuffer;
        IndexMeshPrimitive _currentPrimitive;
        private DictionaryMeshStream<Vector3> _positions;
        private List<int> _positionIndices;
        private DictionaryMeshStream<Vector3> _normals;
        private List<int> _normalIndices;
        private DictionaryMeshStream<Vector4> _colors;
        private List<int> _colorIndices;

        public IndexedMeshBuilder()
        {
        }

        public void BeginPrimitive(PrimitiveTopology topology)
        {
            if (_currentBuffer == null)
                throw new InvalidOperationException("Call BeginBuffer() before BeginPrimitive()");
            _currentPrimitive = new IndexMeshPrimitive(_currentBuffer) {Topology = topology };
            _positionIndices = null;
            _normalIndices = null;
            _colorIndices = null;
            _mesh.Primitives.Add(_currentPrimitive);
        }

        public void BeginBuffer()
        {
            _currentBuffer = new MeshBufferView();
            _currentPrimitive = null;
            _positions = null;
            _positionIndices = null;
            _normals = null;
            _normalIndices = null;
            _colors = null;
            _colorIndices = null;
        }

        public void Position(Vector3 position)
        {
            AddVertexComponent(position, StreamKey.Position, ref _positions, ref _positionIndices);

        }

        public void Normal(Vector3 normal)
        {
            AddVertexComponent(normal, StreamKey.Normal, ref _normals, ref _normalIndices);
        }

        public void Color(Vector4 color)
        {
            AddVertexComponent(color, StreamKey.Color, ref _colors, ref _colorIndices);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddVertexComponent<T>(T value, StreamKey key, ref DictionaryMeshStream<T> values, ref List<int> indices)
        {
            if (_currentPrimitive == null)
            {
                throw new InvalidOperationException("Call BeginPrimitive() to start a new primitive first");
            }
            if (values == null)
            {
                values = new DictionaryMeshStream<T>(StreamConverterFactory.Default);
                _currentBuffer.SetStream(key, values);
            }

            if (indices == null)
            {
                indices = new List<int>();
                _currentPrimitive.SetIndexStream(key, indices);
            }
            indices.Add(values.Add(value));
        }

        public IndexedMesh Complete()
        {
            var indexedMesh = _mesh;
            return indexedMesh;
        }
    }
}