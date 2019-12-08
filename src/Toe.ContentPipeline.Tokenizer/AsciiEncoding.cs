using System;

namespace Toe.ContentPipeline.Tokenizer
{

    public class AsciiTokenEncoding : ITokenEncoding
    {
        public int EstimateCharCount(in ReadOnlySpan<byte> source)
        {
            return source.Length;
        }

        public unsafe int GetString(in ReadOnlySpan<byte> source, Span<char> destination)
        {
            fixed (byte* bytePtr = &source.GetPinnableReference())
            {
                fixed (char* charPtr = &destination.GetPinnableReference())
                {
                    var sourceLength = source.Length;
                    int dest = 0;
                    int index = 0;
                    while (sourceLength - index > 4)
                    {
                        uint* uintPtr = (uint*)(bytePtr + index);
                        ulong val = *uintPtr;
                        ulong* dst = (ulong*)(charPtr + dest);
                        dst[0] =
                            ((val << (48 - 24)) & 0x00FF000000000000ul)
                            | ((val << (32 - 16)) & 0x00FF00000000ul)
                            | ((val << (16 - 8)) & 0x00FF0000ul)
                            | (val & 0x0FF)
                            ;
                        dest += 4;
                        index += 4;
                    }

                    while (sourceLength - index > 0)
                    {
                        charPtr[dest] = (char)bytePtr[index];
                        ++dest;
                        ++index;
                    }
                    return dest;
                }
            }
        }
    }
}