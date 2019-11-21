using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public interface IStreamReader
    {
        Task<IContentContainer> ReadAsync(Stream stream);
    }
}