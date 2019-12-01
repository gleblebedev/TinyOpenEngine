using System.Collections.Generic;
using System.Numerics;
using SharpGLTF.Schema2;

namespace Toe.ContentPipeline.GLTFSharp
{
    public class BufferViewReader
    {
        public MeshBufferView BufferView { get; set; }

        public List<StreamReader> StreamReaders { get; } = new List<StreamReader>();

        public abstract class StreamReader
        {
            public abstract IMeshStream GetMeshStream();

            public abstract void Append(Accessor accessor, IEnumerable<int> map);
        }

        public class ScalarBufferStreamReader : StreamReader
        {
            private readonly ListMeshStream<float> _stream = new ListMeshStream<float>(StreamConverterFactory.Default);

            public override IMeshStream GetMeshStream()
            {
                return _stream;
            }

            public override void Append(Accessor accessor, IEnumerable<int> map)
            {
                var scalar = accessor.AsScalarArray();
                foreach (var index in map) _stream.Add(scalar[index]);
            }
        }

        public class Vec2BufferStreamReader : StreamReader
        {
            private readonly ListMeshStream<Vector2> _stream =
                new ListMeshStream<Vector2>(StreamConverterFactory.Default);

            public override IMeshStream GetMeshStream()
            {
                return _stream;
            }

            public override void Append(Accessor accessor, IEnumerable<int> map)
            {
                var scalar = accessor.AsVector2Array();
                foreach (var index in map) _stream.Add(scalar[index]);
            }
        }

        public class Vec3BufferStreamReader : StreamReader
        {
            private readonly ListMeshStream<Vector3> _stream =
                new ListMeshStream<Vector3>(StreamConverterFactory.Default);

            public override IMeshStream GetMeshStream()
            {
                return _stream;
            }

            public override void Append(Accessor accessor, IEnumerable<int> map)
            {
                var scalar = accessor.AsVector3Array();
                foreach (var index in map) _stream.Add(scalar[index]);
            }
        }

        public class Vec4BufferStreamReader : StreamReader
        {
            private readonly ListMeshStream<Vector4> _stream =
                new ListMeshStream<Vector4>(StreamConverterFactory.Default);

            public override IMeshStream GetMeshStream()
            {
                return _stream;
            }

            public override void Append(Accessor accessor, IEnumerable<int> map)
            {
                var scalar = accessor.AsVector4Array();
                foreach (var index in map) _stream.Add(scalar[index]);
            }
        }
    }
}