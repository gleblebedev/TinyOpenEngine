using System;
using System.Text;
using NUnit.Framework;

namespace Toe.ContentPipeline.Tokenizer.UnitTests
{
    [TestFixture]
    public class Utf8TokenEncodingTestFixture
    {
        [Test]
        [TestCase(0b00100100, 0b00100100, 0b00100100, 0b00100100)]
        [TestCase(0b11000010, 0b10100010, 0b11000010, 0b10100010)]
        [TestCase(0b11100000, 0b10100100, 0b10111001, 0b00100100)]
        [TestCase(0b11100010, 0b10000010, 0b10101100, 0b00100100)]
        [TestCase(0b11101101, 0b10010101, 0b10011100, 0b00100100)]
        [TestCase(0b11101111, 0b10111111, 0b10111111, 0b00100100)]
        [TestCase(0b11110000, 0b10010000, 0b10001101, 0b10001000)]
        public void A(byte b0, byte b1, byte b2, byte b3)
        {
            var array = new[] {b0, b1, b2, b3};
            var str = Encoding.UTF8.GetString(array);
            var encoding = new Utf8TokenEncoding();

            for (int i = 1; i < array.Length; ++i)
            {
                for (int j = 1; j < array.Length-i; ++j)
                {
                    var span0 = new Span<byte>(array, 0, i);
                    var span1 = new Span<byte>(array, i, j);
                    var span2 = new Span<byte>(array, i+j, array.Length - i-j);
                    var output = new char[5];

                    var estimatedLength0 = encoding.EstimateCharCount(span0);
                    var res0Len = encoding.GetString(span0, new Span<char>(output, 0, estimatedLength0));
                    var res0 = new Span<char>(output, 0, res0Len).ToString();

                    var estimatedLength1 = encoding.EstimateCharCount(span1);
                    var res1Len = encoding.GetString(span1, new Span<char>(output, res0Len, estimatedLength1));
                    var res1 = new Span<char>(output, res0Len, res1Len).ToString();


                    var estimatedLength2 = encoding.EstimateCharCount(span2);
                    var res2Len = encoding.GetString(span2, new Span<char>(output, res0Len+ res1Len, estimatedLength2));
                    var res2 = new Span<char>(output, res0Len+ res1Len, res2Len).ToString();

                    Assert.AreEqual(str, res0 + res1+ res2);
                }
            }
        }
    }
}