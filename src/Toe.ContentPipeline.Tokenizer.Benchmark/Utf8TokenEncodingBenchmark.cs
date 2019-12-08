using System;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Toe.ContentPipeline.Tokenizer.Benchmark
{
    public class Utf8TokenEncodingBenchmark
    {
        private byte[] _binaryFile;

        public Utf8TokenEncodingBenchmark()
        {
            _binaryFile = File.ReadAllBytes("Ase.ase");
        }

        [Benchmark]
        public void AseFile()
        {
            var encoding = new Utf8TokenEncoding();
            var blockSize = 1024;
            var blockOutput = new char[1024];
            for (int i = 0; i < _binaryFile.Length; i+=blockSize)
            {
                encoding.GetString(new ReadOnlySpan<byte>(_binaryFile, i, Math.Min(blockSize, _binaryFile.Length - i)), new Span<char>(blockOutput));
            }
        }
    }
}