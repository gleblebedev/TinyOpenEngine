using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public class FileReferenceImage : AbstractImageAsset
    {
        protected override Task<Stream> GetStreamAsync()
        {
            return Task.FromResult((Stream) File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }
    }
}