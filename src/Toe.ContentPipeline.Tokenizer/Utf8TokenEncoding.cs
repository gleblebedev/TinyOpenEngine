using System;
using System.Runtime.CompilerServices;

namespace Toe.ContentPipeline.Tokenizer
{
    public class Utf8TokenEncoding : ITokenEncoding
    {
        internal unsafe struct Utf8Char
        {
            public fixed byte Bytes[4];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public char AsOneByteChar()
            {
                return (char) Bytes[0];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public char AsTwoByteChar()
            {
                int b0 = Bytes[0] & 0b00011111;
                int b1 = Bytes[1] & 0b00111111;
                return (char)((b0 << 6) | b1);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public char AsThreeByteChar()
            {
                int b0 = Bytes[0] & 0b00001111;
                int b1 = Bytes[1] & 0b00111111;
                int b2 = Bytes[2] & 0b00111111;
                return (char)((b0 << 12) | (b1 << 6) | b2);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AsFourByteChar(in Span<char> dest)
            {
                int b0 = Bytes[0] & 0b00000111;
                int b1 = Bytes[1] & 0b00111111;
                int b2 = Bytes[2] & 0b00111111;
                int b3 = Bytes[3] & 0b00111111;
                var num = (char)((b0 << 18) | (b1 << 12) | (b2 << 6) | (b3));
                dest[0] = (char)(0xD800 + ((num >> 10) & 0b01111111111));
                dest[1] = (char)(0xDC00 + (num & 0b01111111111));
            }
        }
        public int Position;
        public int ExpectedLength;

        private Utf8Char _leftovers;


        public int EstimateCharCount(in ReadOnlySpan<byte> source)
        {
            if (ExpectedLength > 0)
                return source.Length + 1;
            return source.Length;
        }

        public unsafe Span<char> GetString(in ReadOnlySpan<byte> source, Span<char> destination)
        {
            int dest = 0;
            int index = 0;
            if (ExpectedLength > 0)
            {
                var toCopy = Math.Min(source.Length, ExpectedLength - Position);
                for (; index < toCopy; ++index, ++Position)
                {
                    _leftovers.Bytes[Position] = source[index];
                }

                if (ExpectedLength != Position)
                    return destination.Slice(0, 0);
                switch (ExpectedLength)
                {
                    case 2:
                        destination[dest] = _leftovers.AsTwoByteChar();
                        ++dest;
                        break;
                    case 3:
                        destination[dest] = _leftovers.AsThreeByteChar();
                        ++dest;
                        break;
                    case 4:
                        _leftovers.AsFourByteChar(destination);
                        dest += 2;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                ExpectedLength = 0;
            }
            Utf8Char currentChar;
            while (index < source.Length)
            {
                currentChar.Bytes[0] = source[index];
                if (0 == (currentChar.Bytes[0] & 0x80))
                {
                    destination[dest] = currentChar.AsOneByteChar();
                    ++dest;
                    ++index;
                    continue;
                }

                if ((currentChar.Bytes[0] & 0b11100000) == 0b11000000)
                {
                    if (source.Length - index < 2)
                    {
                        _leftovers.Bytes[0] = currentChar.Bytes[0];

                        Position = 1;
                        ExpectedLength = 2;
                        break;
                    }

                    currentChar.Bytes[1] = source[index + 1];
                    destination[dest] = currentChar.AsTwoByteChar();
                    ++dest;
                    index += 2;
                    continue;
                }
                if ((currentChar.Bytes[0] & 0b11110000) == 0b11100000)
                {
                    var presentBytes = source.Length - index;
                    if (presentBytes < 3)
                    {
                        _leftovers.Bytes[0] = currentChar.Bytes[0];
                        for (int i = 1; i < presentBytes; ++i)
                        {
                            _leftovers.Bytes[i] = source[index+i];
                        }
                        Position = presentBytes;
                        ExpectedLength = 3;
                        break;
                    }

                    currentChar.Bytes[1] = source[index + 1];
                    currentChar.Bytes[2] = source[index + 2];
                    destination[dest] = currentChar.AsThreeByteChar();
                    ++dest;
                    index += 3;
                    continue;
                }
                if ((currentChar.Bytes[0] & 0b11111000) == 0b11110000)
                {
                    var presentBytes = source.Length - index;
                    if (presentBytes < 4)
                    {
                        _leftovers.Bytes[0] = currentChar.Bytes[0];
                        for (int i = 1; i < presentBytes; ++i)
                        {
                            _leftovers.Bytes[i] = source[index + i];
                        }
                        Position = presentBytes;
                        ExpectedLength = 4;
                        break;
                    }

                    currentChar.Bytes[1] = source[index + 1];
                    currentChar.Bytes[2] = source[index + 2];
                    currentChar.Bytes[3] = source[index + 3];
                    currentChar.AsFourByteChar(destination.Slice(dest));
                    dest+=2;
                    index += 4;
                    continue;
                }
                // Handle invalid UTF8 as an ascii character
                {
                    destination[dest] = (char)currentChar.AsOneByteChar();
                    ++dest;
                    ++index;
                    continue;
                }
            }

            return destination.Slice(0, dest);
        }
    }
}