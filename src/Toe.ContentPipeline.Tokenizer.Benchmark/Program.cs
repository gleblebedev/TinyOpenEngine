namespace Toe.ContentPipeline.Tokenizer.Benchmark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
            //new Utf8TokenEncodingBenchmark().AseFile();
            new Utf8TokenEncodingBenchmark().TwoByteChars();
#else
            var summary = BenchmarkRunner.Run<Utf8TokenEncodingBenchmark>();
            Console.WriteLine(summary);
#endif
        }
    }
}