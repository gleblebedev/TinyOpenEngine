using System.IO;
using System.Threading.Tasks;
using SharpGLTF.Schema2;
using Toe.ContentPipeline;

namespace Toe.ConentPipeline.GLTFSharp
{
    public class GltfSharpWriter : IFileWriter
    {
        public Task WriteAsync(Stream stream)
        {
            return Task.Run(() =>
            {
                var root = ModelRoot.CreateModel();
                root.WriteGLB(stream);
            });
        }
    }
}