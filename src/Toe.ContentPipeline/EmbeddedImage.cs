using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public class EmbeddedImage : AbstractImageAsset
    {
        public EmbeddedImage(ArraySegment<byte> rawData)
        {
            RawData = rawData;
        }

        public ArraySegment<byte> RawData { get; set; }

        public override ValueTask<ArraySegment<byte>> GetContentAsync()
        {
            return new ValueTask<ArraySegment<byte>>(RawData);
        }

        protected override ValueTask<Stream> GetStreamAsync()
        {
            return new ValueTask<Stream>(new MemoryStream(RawData.ToArray()));
        }
    }
}