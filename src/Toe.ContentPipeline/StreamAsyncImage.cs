using System;
using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public class StreamAsyncImage : AbstractImageAsset
    {
        private readonly Func<Task<Stream>> _streamFactory;

        public StreamAsyncImage(Func<Task<Stream>> streamFactory)
        {
            _streamFactory = streamFactory;
        }

        protected override ValueTask<Stream> GetStreamAsync()
        {
            return new ValueTask<Stream>(_streamFactory());
        }
    }
}