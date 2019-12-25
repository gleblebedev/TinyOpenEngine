using System.Numerics;
using NUnit.Framework;

namespace Toe.ContentPipeline
{
    [TestFixture]
    public class GpuMeshTestFixture
    {
        [Test]
        public void Optimize_EmptyMesh_DoesntThrow()
        {
            var index = new IndexedMesh();

            Assert.DoesNotThrow(()=>GpuMesh.Optimize(index));
        }

        [Test]
        public void Optimize_IdIsSet_KeepsId()
        {
            var index = new IndexedMesh("id1");

            var gpuMesh = GpuMesh.Optimize(index);

            Assert.AreEqual(index.Id, gpuMesh.Id);
        }

        [Test]
        public void Optimize_TwoTriangles_4VerticesAnd6IndicesCreated()
        {
            var builder = new IndexedMeshBuilder();
            builder.BeginBuffer();
            builder.BeginPrimitive(PrimitiveTopology.TriangleList);
            builder.Position(new Vector3(0,0,0));
            builder.Position(new Vector3(1, 0, 0));
            builder.Position(new Vector3(0, 1, 0));
            builder.Position(new Vector3(1, 0, 0));
            builder.Position(new Vector3(1, 1, 0));
            builder.Position(new Vector3(0, 1, 0));

            var mesh = GpuMesh.Optimize(builder.Complete());

            Assert.AreEqual(1, mesh.Primitives.Count);
            Assert.AreEqual(6, mesh.Primitives[0].Count);
            Assert.AreEqual(4, mesh.Primitives[0].BufferView.GetStreamReader<Vector3>(StreamKey.Position).Count);
        }

        [Test]
        public void Optimize_TwoTrianglesOfDifferentColor_6VerticesAnd6IndicesCreated()
        {
            var builder = new IndexedMeshBuilder();
            builder.BeginBuffer();
            builder.BeginPrimitive(PrimitiveTopology.TriangleList);
            builder.Color(Vector4.Zero);
            builder.Position(new Vector3(0, 0, 0));
            builder.Color(Vector4.Zero);
            builder.Position(new Vector3(1, 0, 0));
            builder.Color(Vector4.Zero);
            builder.Position(new Vector3(0, 1, 0));
            builder.Color(Vector4.One);
            builder.Position(new Vector3(1, 0, 0));
            builder.Color(Vector4.One);
            builder.Position(new Vector3(1, 1, 0));
            builder.Color(Vector4.One);
            builder.Position(new Vector3(0, 1, 0));

            var mesh = GpuMesh.Optimize(builder.Complete());

            Assert.AreEqual(1, mesh.Primitives.Count);
            Assert.AreEqual(6, mesh.Primitives[0].Count);
            Assert.AreEqual(6, mesh.Primitives[0].BufferView.GetStreamReader<Vector3>(StreamKey.Position).Count);
            Assert.AreEqual(6, mesh.Primitives[0].BufferView.GetStreamReader<Vector3>(StreamKey.Color).Count);
        }
    }
}