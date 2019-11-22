using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Toe.ContentPipeline;

namespace Toe.ConentPipeline.GLTFSharp.UnitTests
{
    [TestFixture]
    public class GltfSharpWriterTestFixture
    {
        [Test]
        public async Task Write()
        {
            var mesh = new GpuMesh("geometry");
            var scene = new SceneAsset("main");
            var container = new ContentContainer();
            container.Scenes.Add(scene);
            container.Meshes.Add(mesh);

            var writer = new GltfSharpWriter();
            var memoryStream = new MemoryStream();
            await writer.WriteAsync(memoryStream, container);
        }
    }
}