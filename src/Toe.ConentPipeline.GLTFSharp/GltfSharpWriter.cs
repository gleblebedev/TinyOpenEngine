using System.IO;
using System.Threading.Tasks;
using SharpGLTF.Scenes;
using Toe.ContentPipeline;

namespace Toe.ConentPipeline.GLTFSharp
{
    public class GltfSharpWriter : IFileWriter
    {
        public Task WriteAsync(Stream stream, IContentContainer content)
        {
            return Task.Run(() =>
            {
                var scene = new SceneBuilder();
                foreach (var sceneAsset in content.Scenes)
                {
                }

                var modelRoot = scene.ToSchema2();
                modelRoot.WriteGLB(stream);
            });
        }
    }
}