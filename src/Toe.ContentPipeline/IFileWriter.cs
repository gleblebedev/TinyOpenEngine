using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public interface IFileWriter
    {
        Task WriteAsync(Stream stream, IContentContainer content);
    }
}