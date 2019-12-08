using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public class AsciiTokenEncoding : ITokenEncoding
    {
        public int EstimateCharCount(in ReadOnlySpan<byte> source)
        {
            return source.Length;
        }

        public Span<char> GetString(in ReadOnlySpan<byte> source, Span<char> destination)
        {
            for (var index = 0; index < source.Length; index++)
            {
                destination[index] = (char) source[index];
            }

            return destination.Slice(0, source.Length);
        }
    }
}