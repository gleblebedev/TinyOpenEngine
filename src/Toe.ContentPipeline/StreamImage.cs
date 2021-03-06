﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace Toe.ContentPipeline
{
    public class StreamImage : AbstractImageAsset
    {
        private readonly Func<Stream> _streamFactory;

        public StreamImage(Func<Stream> streamFactory)
        {
            _streamFactory = streamFactory;
        }

        protected override ValueTask<Stream> GetStreamAsync()
        {
            return new ValueTask<Stream>(_streamFactory());
        }
    }
}