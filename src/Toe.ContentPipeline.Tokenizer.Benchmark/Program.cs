using System;
using BenchmarkDotNet.Running;

namespace Toe.ContentPipeline.Tokenizer.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Utf8TokenEncodingBenchmark>();
            Console.WriteLine(summary);
        }
    }
}
