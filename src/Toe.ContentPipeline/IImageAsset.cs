using System;
using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public interface IImageAsset : IAsset
    {
        string Path { get; }

        string FileExtension { get; }

        ValueTask<ArraySegment<byte>> GetContentAsync();

        ValueTask<Stream> OpenAsync();
    }
}