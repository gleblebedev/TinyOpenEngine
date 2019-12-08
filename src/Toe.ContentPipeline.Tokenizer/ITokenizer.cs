using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public interface ITokenizer
    {
        void OnNext(in ReadOnlySpan<byte> input);
        void OnCompleted();
        void OnError(Exception exception);
    }
}