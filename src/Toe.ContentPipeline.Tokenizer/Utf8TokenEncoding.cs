using System;
using System.Runtime.CompilerServices;

namespace Toe.ContentPipeline.Tokenizer
{
    public class Utf8TokenEncoding : ITokenEncoding
    {
        private Callback _callback;
        private uint _leftovers;
        private int _shift;

        public Utf8TokenEncoding()
        {
            _callback = GetStringImpl;
        }

        public int EstimateCharCount(in ReadOnlySpan<byte> source)
        {
            if (_callback != GetStringImpl)
                return source.Length + 1;
            return source.Length;
        }

        public int GetString(in ReadOnlySpan<byte> source, Span<char> destination)
        {
            return _callback(source, destination);
        }

        public unsafe int GetStringImpl(in ReadOnlySpan<byte> source, Span<char> destination)
        {
            var dest = 0;

            fixed (byte* bytePtr = &source.GetPinnableReference())
            {
                fixed (char* charPtr = &destination.GetPinnableReference())
                {
                    var sourceLength = source.Length;
                    var index = 0;
                    while (sourceLength - index > 4)
                    {
                        var uintPtr = (uint*) (bytePtr + index);
                        if ((*uintPtr & 0x80808080) == 0)
                        {
                            ulong val = *uintPtr;
                            var dst = (ulong*) (charPtr + dest);
                            dst[0] =
                                ((val << (48 - 24)) & 0x00FF000000000000ul)
                                | ((val << (32 - 16)) & 0x00FF00000000ul)
                                | ((val << (16 - 8)) & 0x00FF0000ul)
                                | (val & 0x0FF)
                                ;
                            dest += 4;
                            index += 4;
                        }
                        else
                        {
                            ParseIndividualSymbols(sourceLength, ref index, bytePtr, charPtr, ref dest, 4);
                        }
                    }

                    ParseIndividualSymbols(sourceLength, ref index, bytePtr, charPtr, ref dest,
                        destination.Length - dest);
                }
            }

            return dest;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void ParseIndividualSymbols(int lastIndex, ref int index, byte* bytePtr, char* charPtr,
            ref int dest, int takeCount)
        {
            while (index < lastIndex && takeCount > 0)
            {
                var firstByte = (uint) bytePtr[index];
                if (firstByte < 0b1000_0000)
                {
                    charPtr[dest] = (char) bytePtr[index];
                    ++dest;
                    ++index;
                    --takeCount;
                }
                else if (firstByte < 0b1110_0000)
                {
                    if (index + 1 < lastIndex)
                    {
                        var secondByte = (uint) bytePtr[index + 1];
                        charPtr[dest] = (char) (((firstByte & 0b011111) << 6) | (secondByte & 0b00111111));
                        ++dest;
                        index += 2;
                        --takeCount;
                    }
                    else
                    {
                        _leftovers = (firstByte & 0b011111) << 6;
                        _callback = ContinueMultibyteChar;
                        _shift = 0;
                        return;
                    }
                }
                else if (firstByte < 0b1111_0000)
                {
                    if (index + 2 < lastIndex)
                    {
                        var secondByte = (uint) bytePtr[index + 1] & 0b00111111;
                        var thirdByte = (uint) bytePtr[index + 2] & 0b00111111;
                        charPtr[dest] = (char) (((firstByte & 0b01111) << 12) | (secondByte << 6) | thirdByte);
                        ++dest;
                        index += 3;
                        --takeCount;
                    }
                    else
                    {
                        _leftovers = (firstByte & 0b01111) << 12;
                        ++index;
                        _shift = 6;
                        _callback = ContinueMultibyteChar;
                        ContinueMultibyteChar(new ReadOnlySpan<byte>(bytePtr + 1, lastIndex - index),
                            new Span<char>(charPtr, takeCount));
                        return;
                    }
                }
                else
                {
                    if (index + 3 < lastIndex)
                    {
                        var secondByte = (uint) bytePtr[index + 1] & 0b00111111;
                        var thirdByte = (uint) bytePtr[index + 2] & 0b00111111;
                        var fourthByte = (uint) bytePtr[index + 3] & 0b00111111;
                        var val = ((firstByte & 0b0111) << 18) | (secondByte << 12) | (thirdByte << 6) | fourthByte;
                        charPtr[dest] = (char) (0xD800 | ((val >> 10) & 0b01111111111));
                        charPtr[dest + 1] = (char) (0xDC00 | (val & 0b01111111111));
                        dest += 2;
                        index += 4;
                        --takeCount;
                    }
                    else
                    {
                        _leftovers = (firstByte & 0b0111) << 18;
                        ++index;
                        _shift = 12;
                        _callback = ContinueMultibyteChar;
                        ContinueMultibyteChar(new ReadOnlySpan<byte>(bytePtr + 1, lastIndex - index),
                            new Span<char>(charPtr, takeCount));
                        return;
                    }
                }
            }
        }

        private int ContinueMultibyteChar(in ReadOnlySpan<byte> source, Span<char> destination)
        {
            if (source.Length == 0)
                return 0;
            var index = 0;
            for (;;)
            {
                var nextByte = source[index];
                if ((nextByte & 0b11000000) != 0b10000000) break;
                _leftovers |= ((uint) nextByte & 0b00111111) << _shift;
                ++index;
                _shift -= 6;
                if (_shift < 0)
                    break;
                if (index >= source.Length) return 0;
            }

            _callback = GetStringImpl;
            if (_leftovers < 0x010000)
            {
                destination[0] = (char) _leftovers;
                return GetStringImpl(source.Slice(index), destination.Slice(1)) + 1;
            }

            destination[0] = (char) (0xD800 + (((_leftovers - 0x10000) >> 10) & 0b011_1111_1111));
            destination[1] = (char) (0xDC00 + (_leftovers & 0b011_1111_1111));
            return GetStringImpl(source.Slice(index), destination.Slice(2)) + 2;
        }

        private delegate int Callback(in ReadOnlySpan<byte> source, Span<char> destination);
    }
}