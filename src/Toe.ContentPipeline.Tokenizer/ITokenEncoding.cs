using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public interface ITokenEncoding
    {
        int EstimateCharCount(in ReadOnlySpan<byte> source);

        int GetString(in ReadOnlySpan<byte> source, Span<char> destination);
    }
}