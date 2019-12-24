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

    }
}