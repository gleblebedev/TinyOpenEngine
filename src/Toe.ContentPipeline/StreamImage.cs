using System;
using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public class StreamImage : AbstractImageAsset
    {
        private readonly Func<Task<Stream>> _streamFactory;

        public StreamImage(Func<Task<Stream>> streamFactory)
        {
            _streamFactory = streamFactory;
        }

        protected override Task<Stream> GetStreamAsync()
        {
            return _streamFactory();
        }
    }
}