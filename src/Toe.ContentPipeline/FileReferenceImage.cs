using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public class FileReferenceImage : AbstractImageAsset
    {
        protected override ValueTask<Stream> GetStreamAsync()
        {
            return new ValueTask<Stream>((Stream) File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }
    }
}