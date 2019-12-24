using NUnit.Framework;

namespace Toe.ContentPipeline
{
    [TestFixture]
    public class IndexedMeshTestFixture
    {
        [Test]
        public void Optimize_EmptyMesh_DoesntThrow()
        {
            var index = new GpuMesh();

            Assert.DoesNotThrow(() => IndexedMesh.Optimize(index));
        }

        [Test]
        public void Optimize_IdIsSet_KeepsId()
        {
            var index = new GpuMesh("id1");

            var gpuMesh = IndexedMesh.Optimize(index);

            Assert.AreEqual(index.Id, gpuMesh.Id);
        }
    }
}