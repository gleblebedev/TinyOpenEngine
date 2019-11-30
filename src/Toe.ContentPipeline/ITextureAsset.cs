using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public interface ITextureAsset : IAsset
    {
        ValueTask<byte[]> GetContentAsync();
    }
}