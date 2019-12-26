using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Toe.ContentPipeline
{
    public class IndexedMeshBuilder
    {
        private StreamData<Vector4> _colors;
        private MeshBufferView _currentBuffer;
        private IndexMeshPrimitive _currentPrimitive;
        private StreamData<Vector4i> _joints;
        private readonly IndexedMesh _mesh = new IndexedMesh();
        private StreamData<Vector3> _normals;
        private StreamData<Vector3> _positions;
        private StreamData<Vector4> _tangents;
        private StreamData<Vector2> _texCoord0;
        private StreamData<Vector2> _texCoord1;
        private StreamData<Vector4> _weights;

        public void BeginPrimitive(PrimitiveTopology topology)
        {
            if (_currentBuffer == null)
                throw new InvalidOperationException("Call BeginBuffer() before BeginPrimitive()");
            _currentPrimitive = new IndexMeshPrimitive(_currentBuffer) {Topology = topology};
            _positions?.ResetIndices();
            _normals?.ResetIndices();
            _colors?.ResetIndices();
            _tangents?.ResetIndices();
            _texCoord0?.ResetIndices();
            _texCoord1?.ResetIndices();
            _joints?.ResetIndices();
            _weights?.ResetIndices();
            _mesh.Primitives.Add(_currentPrimitive);
        }

        public void BeginBuffer()
        {
            _currentBuffer = new MeshBufferView();
            _currentPrimitive = null;
            _positions = null;
            _normals = null;
            _colors = null;
            _tangents = null;
            _texCoord0 = null;
            _texCoord1 = null;
            _joints = null;
            _weights = null;
        }

        /// <summary>
        ///     Add position value to a current primitive.
        /// </summary>
        /// <param name="position">Position of the vertex.</param>
        public void Position(Vector3 position)
        {
            AddVertexComponent(position, StreamKey.Position, ref _positions);
        }

        /// <summary>
        ///     Add position value to a current primitive and complete the vertex.
        ///     If some components of the vector weren't specified the last know component value will be copied over to the vertex.
        ///     Basically this is the same behaviour as glVertex3f has.
        /// </summary>
        /// <param name="position">Position of the vertex.</param>
        public void Vertex(Vector3 position)
        {
            AddVertexComponent(position, StreamKey.Position, ref _positions);
            var expectedCount = _positions.Indices.Count;
            foreach (var indices in GetActiveIndexStreams())
                while (indices.Indices.Count < expectedCount)
                    indices.Indices.Add(indices.LastIndex);
        }

        private IEnumerable<StreamData> GetActiveIndexStreams()
        {
            if (_positions?.Indices != null)
                yield return _positions;
            if (_normals?.Indices != null)
                yield return _normals;
            if (_colors?.Indices != null)
                yield return _colors;
            if (_tangents?.Indices != null)
                yield return _tangents;
            if (_texCoord0?.Indices != null)
                yield return _texCoord0;
            if (_texCoord1?.Indices != null)
                yield return _texCoord1;
            if (_joints?.Indices != null)
                yield return _joints;
            if (_weights?.Indices != null)
                yield return _weights;
        }

        public void Normal(Vector3 normal)
        {
            AddVertexComponent(normal, StreamKey.Normal, ref _normals);
        }

        public void TangentAndHandedness(Vector4 tangent)
        {
            AddVertexComponent(tangent, StreamKey.Tangent, ref _tangents);
        }

        public void TexCoord0(Vector2 uv)
        {
            AddVertexComponent(uv, StreamKey.TexCoord0, ref _texCoord0);
        }

        public void TexCoord1(Vector2 uv)
        {
            AddVertexComponent(uv, StreamKey.TexCoord1, ref _texCoord1);
        }

        public void Joints(Vector4i joints)
        {
            AddVertexComponent(joints, StreamKey.Joints, ref _joints);
        }

        public void Weights(Vector4 weights)
        {
            AddVertexComponent(weights, StreamKey.Weights, ref _weights);
        }

        public void Color(Vector4 color)
        {
            AddVertexComponent(color, StreamKey.Color, ref _colors);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddVertexComponent<T>(T value, StreamKey key, ref StreamData<T> data)
        {
            if (_currentPrimitive == null)
                throw new InvalidOperationException("Call BeginPrimitive() to start a new primitive first");
            if (data == null)
            {
                data = new StreamData<T> {Key = key};
                data.Values = new DictionaryMeshStream<T>(StreamConverterFactory.Default);
                _currentBuffer.SetStream(key, data.Values);
            }

            if (data.Indices == null)
            {
                data.Indices = new List<int>();
                _currentPrimitive.SetIndexStream(key, data.Indices);
            }

            data.LastIndex = data.Values.Add(value);
            data.Indices.Add(data.LastIndex);
        }

        public IndexedMesh Complete()
        {
            var indexedMesh = _mesh;
            return indexedMesh;
        }

        private class StreamData
        {
            public List<int> Indices;
            public int LastIndex;
        }

        private class StreamData<T> : StreamData
        {
            public StreamKey Key;
            public DictionaryMeshStream<T> Values;

            public void ResetIndices()
            {
                if (Values != null)
                    Indices = new List<int>();
                else
                    Indices = null;
            }
        }
    }
}