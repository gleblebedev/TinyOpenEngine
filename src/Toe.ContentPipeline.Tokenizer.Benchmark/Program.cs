using System;
using System.IO;
using System.Net;
using BenchmarkDotNet.Running;

namespace Toe.ContentPipeline.Tokenizer.Benchmark
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var buf = new byte[4049];
                using (var fileStream = File.Open(args[0], FileMode.Open, FileAccess.Read, FileShare.Write))
                {
                    var tokenizer = new SimpleTokenizer(new AbstractTokenObserver<SimpleTokenizer.TokenType>(),
                        new Utf8TokenEncoding());
                    for (; ; )
                    {
                        var len = fileStream.Read(buf, 0, buf.Length);
                        if (len <= 0)
                            break;
                        tokenizer.OnNext(new ReadOnlySpan<byte>(buf, 0, len));
                    }
                }
            }
            else
            {
#if DEBUG
            
                new Utf8TokenEncodingBenchmark().AseFile();
                new Utf8TokenEncodingBenchmark().TwoByteChars();
#else
                var summary = BenchmarkRunner.Run<Utf8TokenEncodingBenchmark>();
                Console.WriteLine(summary);
#endif
            }
        }
    }
}