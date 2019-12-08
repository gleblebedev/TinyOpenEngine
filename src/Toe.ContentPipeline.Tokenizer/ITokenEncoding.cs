using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public interface ITokenEncoding
    {
        int EstimateCharCount(in ReadOnlySpan<byte> source);

        Span<char> GetString(in ReadOnlySpan<byte> source, Span<char> destination);
    }
}