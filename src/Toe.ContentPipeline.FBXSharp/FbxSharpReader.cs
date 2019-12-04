using System.IO;
using System.Threading.Tasks;
using UkooLabs.FbxSharpie;

namespace Toe.ContentPipeline.FBXSharp
{
    public class FbxSharpReader : IStreamReader
    {
        public Task<IContentContainer> ReadAsync(Stream stream)
        {
            return Task.Run(() =>
            {
                var contentContainer = new ContentContainer();
                var fbxDocument = FbxIO.Read(stream, ErrorLevel.Permissive);
                return (IContentContainer)contentContainer;
            });
        }
    }
}