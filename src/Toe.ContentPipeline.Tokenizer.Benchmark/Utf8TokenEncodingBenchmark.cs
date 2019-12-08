using System;
using System.IO;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Toe.ContentPipeline.Tokenizer.Benchmark
{
    public class Utf8TokenEncodingBenchmark
    {
        private byte[] _binaryFile;
        private byte[] _twoByteChars;

        public Utf8TokenEncodingBenchmark()
        {
            _binaryFile = File.ReadAllBytes("Ase.ase");
            _twoByteChars = Encoding.UTF8.GetBytes(Enumerable.Range(0x80,0x07FF-0x80).Select(_=>(char)_).ToArray());
        }

        [Benchmark]
        public void AseFile()
        {
            TestEncoding(_binaryFile);
        }

        [Benchmark]
        public void TwoByteChars()
        {
            TestEncoding(_twoByteChars);
        }

        private void TestEncoding(byte[] source)
        {
            var encoding = new Utf8TokenEncoding();
            var blockSize = 1024;
            var blockOutput = new char[1024];
            for (int i = 0; i < source.Length; i += blockSize)
            {
                encoding.GetString(new ReadOnlySpan<byte>(source, i, Math.Min(blockSize, source.Length - i)),
                    new Span<char>(blockOutput));
            }
        }
    }
}