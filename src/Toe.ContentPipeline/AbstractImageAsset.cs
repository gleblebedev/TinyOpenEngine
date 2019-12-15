using System;
using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public abstract class AbstractImageAsset : AbstractAsset, IImageAsset
    {
        public string Path { get; set; }

        public string FileExtension { get; set; }

        public virtual async ValueTask<ArraySegment<byte>> GetContentAsync()
        {
            using (var stream = await GetStreamAsync())
            {
                var count = stream.Length;
                var buffer = new byte[count];
                var offset = 0;
                while (count > 0)
                {
                    var toRead = count > int.MaxValue ? int.MaxValue : (int) count;
                    var n = await stream.ReadAsync(buffer, offset, toRead);
                    if (n == 0) throw new InvalidOperationException($"Unexpected end of file {Path}");
                    offset += n;
                    count -= n;
                }

                return new ArraySegment<byte>();
            }
        }

        public async ValueTask<Stream> OpenAsync()
        {
            return await GetStreamAsync();
        }

        protected abstract ValueTask<Stream> GetStreamAsync();

        public override string ToString()
        {
            if (Path != null)
                return Path;
            return $"{Id}.{FileExtension}";
        }
    }
}