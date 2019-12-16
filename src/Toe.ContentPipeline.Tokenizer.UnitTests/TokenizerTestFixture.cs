using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Toe.ContentPipeline.Tokenizer.UnitTests
{
    [TestFixture]
    public class TokenizerTestFixture
    {
        public class TokenObserver : ITokenObserver<SimpleTokenizer.TokenType>
        {
            public Exception Error;

            public List<TokenValue<SimpleTokenizer.TokenType>> Tokens =
                new List<TokenValue<SimpleTokenizer.TokenType>>();

            public bool IsCompleted { get; set; }

            public void OnNext(Token<SimpleTokenizer.TokenType> token)
            {
                Tokens.Add(token);
            }

            public void OnError(Exception exception)
            {
                Error = exception;
            }

            public void OnCompleted()
            {
                IsCompleted = true;
            }
        }

        [Test]
        public void OnCompleted_NoTokensNoErrorCompleted()
        {
            var observer = new TokenObserver();
            var tokenizer = new SimpleTokenizer(observer);

            tokenizer.OnCompleted();

            Assert.AreEqual(0, observer.Tokens.Count);
            Assert.IsNull(observer.Error);
            Assert.AreEqual(true, observer.IsCompleted);
        }

        [Test]
        [TestCase("id", SimpleTokenizer.TokenType.Id)]
        [TestCase("+", SimpleTokenizer.TokenType.Separator)]
        [TestCase("-", SimpleTokenizer.TokenType.Separator)]
        [TestCase(".", SimpleTokenizer.TokenType.Separator)]
        [TestCase("0", SimpleTokenizer.TokenType.Int)]
        [TestCase("+10", SimpleTokenizer.TokenType.Int)]
        [TestCase("-10", SimpleTokenizer.TokenType.Int)]
        [TestCase("2147483647", SimpleTokenizer.TokenType.Int)]
        [TestCase("-2147483648", SimpleTokenizer.TokenType.Int)]
        [TestCase("9223372036854775807", SimpleTokenizer.TokenType.Int)]
        [TestCase("-9223372036854775808", SimpleTokenizer.TokenType.Int)]
        [TestCase(".5", SimpleTokenizer.TokenType.Float)]
        [TestCase("+.5", SimpleTokenizer.TokenType.Float)]
        [TestCase("-.5", SimpleTokenizer.TokenType.Float)]
        [TestCase("5.", SimpleTokenizer.TokenType.Float)]
        [TestCase("+5.", SimpleTokenizer.TokenType.Float)]
        [TestCase("-5.", SimpleTokenizer.TokenType.Float)]
        [TestCase("1e-2", SimpleTokenizer.TokenType.Float)]
        [TestCase("+1e-2", SimpleTokenizer.TokenType.Float)]
        [TestCase("-1e-2", SimpleTokenizer.TokenType.Float)]
        [TestCase("1.3e-2", SimpleTokenizer.TokenType.Float)]
        [TestCase("+1.3e-2", SimpleTokenizer.TokenType.Float)]
        [TestCase("-1.3e-2", SimpleTokenizer.TokenType.Float)]
        [TestCase("3.40282347E+38", SimpleTokenizer.TokenType.Float)]
        [TestCase("-3.40282347E+38", SimpleTokenizer.TokenType.Float)]
        [TestCase("340282346638528859811704183484516925440.00", SimpleTokenizer.TokenType.Float)]
        [TestCase("-340282346638528859811704183484516925440.00", SimpleTokenizer.TokenType.Float)]
        public void OnNext_CorrectlyParsed(string text, SimpleTokenizer.TokenType type)
        {
            Console.WriteLine(float.MinValue.ToString("f", CultureInfo.InvariantCulture));
            Console.WriteLine(float.MaxValue.ToString("f", CultureInfo.InvariantCulture));
            var buf = Encoding.UTF8.GetBytes(text);
            for (var i = 0; i < text.Length; ++i)
            {
                var observer = new TokenObserver();
                var tokenizer = new SimpleTokenizer(observer);
                tokenizer.OnNext(new Span<byte>(buf, 0, i));
                tokenizer.OnNext(new Span<byte>(buf, i, buf.Length - i));
                tokenizer.OnCompleted();

                Assert.AreEqual(1, observer.Tokens.Count);
                Assert.AreEqual(text, observer.Tokens[0].Value);
                Assert.AreEqual(type, observer.Tokens[0].Type);
            }
        }

        [Test]
        public void OnNext_EmptyString_NoTokensNoErrorNotCompleted()
        {
            var observer = new TokenObserver();
            var tokenizer = new SimpleTokenizer(observer);

            tokenizer.OnNext(ReadOnlySpan<byte>.Empty);

            Assert.AreEqual(0, observer.Tokens.Count);
            Assert.IsNull(observer.Error);
            Assert.AreEqual(false, observer.IsCompleted);
        }

        [Test]
        public void OnNext_Id_IdCorrectlyParsed()
        {
            var observer = new TokenObserver();
            var tokenizer = new SimpleTokenizer(observer);

            tokenizer.OnNext(Encoding.UTF8.GetBytes("Bla"));
            tokenizer.OnCompleted();

            Assert.AreEqual(1, observer.Tokens.Count);
            Assert.AreEqual("Bla", observer.Tokens[0].Value);
            Assert.AreEqual(SimpleTokenizer.TokenType.Id, observer.Tokens[0].Type);
            Assert.IsNull(observer.Error);
            Assert.AreEqual(true, observer.IsCompleted);
        }

        [Test]
        public void OnNext_SplittedId_IdCorrectlyParsed()
        {
            var observer = new TokenObserver();
            var tokenizer = new SimpleTokenizer(observer);

            tokenizer.OnNext(Encoding.UTF8.GetBytes("iii"));
            tokenizer.OnNext(Encoding.UTF8.GetBytes("bbb"));
            tokenizer.OnCompleted();

            Assert.AreEqual(1, observer.Tokens.Count);
            Assert.AreEqual("iiibbb", observer.Tokens[0].Value);
            Assert.AreEqual(SimpleTokenizer.TokenType.Id, observer.Tokens[0].Type);
            Assert.IsNull(observer.Error);
            Assert.AreEqual(true, observer.IsCompleted);
        }


        [Test]
        [TestCase(2, 3)]
        [TestCase(1, 3)]
        [TestCase(1, 2)]
        [TestCase(1, 5)]
        [TestCase(3, 2)]
        public void OnNext_SplittedText_TextCorrectlyParsed(int size0, int size1)
        {
            var observer = new TokenObserver();
            var tokenizer = new SimpleTokenizer(observer);

            var buf = Encoding.UTF8.GetBytes("id \tblabla");
            var pos = 0;
            var blocks = new[] {size0, size1};
            foreach (var blockSize in blocks.Where(_ => _ != 0))
            {
                tokenizer.OnNext(new Span<byte>(buf, pos, blockSize));
                pos += blockSize;
            }

            if (pos < buf.Length)
                tokenizer.OnNext(new Span<byte>(buf, pos, buf.Length - pos));
            tokenizer.OnCompleted();
            Console.WriteLine(string.Join(Environment.NewLine, observer.Tokens));
            Assert.AreEqual(3, observer.Tokens.Count);
            Assert.AreEqual("id", observer.Tokens[0].Value);
            Assert.AreEqual(SimpleTokenizer.TokenType.Id, observer.Tokens[0].Type);
            Assert.AreEqual(" \t", observer.Tokens[1].Value);
            Assert.AreEqual(SimpleTokenizer.TokenType.Whitespace, observer.Tokens[1].Type);
            Assert.AreEqual("blabla", observer.Tokens[2].Value);
            Assert.AreEqual(SimpleTokenizer.TokenType.Id, observer.Tokens[2].Type);
            Assert.IsNull(observer.Error);
            Assert.AreEqual(true, observer.IsCompleted);
        }
    }
}