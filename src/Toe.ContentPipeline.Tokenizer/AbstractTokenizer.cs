using System;
using System.Runtime.CompilerServices;

namespace Toe.ContentPipeline.Tokenizer
{
    public abstract class AbstractTokenizer<T> : ITokenizer
    {
        public const char EndOfText = (char) 3;

        protected const int Mismatch = -1;
        protected const int Inconslusive = 0;
        public static readonly ITokenEncoding DefaultEncoding = new AsciiTokenEncoding();
        private readonly IAllocationStrategy _allocationStrategy;
        private readonly ITokenEncoding _encoding;
        private readonly ITokenObserver<T> _observer;
        private char[] _buffer;
        private int _end;
        private readonly byte[] _endOfTextArray = {3};
        private int _position;

        public AbstractTokenizer(ITokenObserver<T> observer) : this(observer, new ArrayPoolAllocationStrategy(),
            DefaultEncoding)
        {
        }

        public AbstractTokenizer(ITokenObserver<T> observer, ITokenEncoding encoding) : this(observer,
            new ArrayPoolAllocationStrategy(), encoding)
        {
        }

        public AbstractTokenizer(ITokenObserver<T> observer, IAllocationStrategy allocationStrategy) : this(observer,
            allocationStrategy, DefaultEncoding)
        {
        }

        public AbstractTokenizer(ITokenObserver<T> observer, IAllocationStrategy allocationStrategy,
            ITokenEncoding encoding)
        {
            _observer = observer;
            _allocationStrategy = allocationStrategy;
            _encoding = encoding;
        }

        public void OnCompleted()
        {
            Process(_endOfTextArray);
            _observer.OnCompleted();
        }

        public void OnError(Exception error)
        {
            _observer.OnError(error);
        }

        public void OnNext(in ReadOnlySpan<byte> value)
        {
            if (value.Length > 0)
                Process(value);
        }

        private void Process(in ReadOnlySpan<byte> value)
        {
            var estimatedCount = _encoding.EstimateCharCount(value);
            if (_buffer == null)
            {
                _buffer = _allocationStrategy.Rent(estimatedCount * 2);
            }
            else if (_end + estimatedCount > _buffer.Length)
            {
                if (estimatedCount < _position && _position > 32)
                {
                    new Span<char>(_buffer, _position, _end - _position).CopyTo(new Span<char>(_buffer, 0,
                        _end - _position));
                    _end -= _position;
                    _position = 0;
                }
                else
                {
                    var buffer = _allocationStrategy.Rent(Math.Max(_buffer.Length * 2, _end + estimatedCount));
                    new Span<char>(_buffer, _position, _end - _position).CopyTo(buffer);
                    _end = _end - _position;
                    _position = 0;
                    _allocationStrategy.Return(_buffer);
                    _buffer = buffer;
                }
            }

            _end += _encoding.GetString(value, new Span<char>(_buffer, _end, estimatedCount));

            for (; _position < _end;)
            {
                var maxLength = _end - _position;
                var readOnlySpan = new ReadOnlySpan<char>(_buffer, _position, maxLength);
                var tokenLen = TryParseToken(readOnlySpan, 0);
                if (IsMismatch(tokenLen))
                    throw new FormatException("No matching token found at \"" +
                                              new string(_buffer, _position, Math.Min(32, maxLength)) + "\"");
                if (IsInconclusive(tokenLen))
                    return;
                _position += tokenLen;
            }
        }

        /// <summary>
        ///     Try parse token starting from the start of textSpan.
        /// </summary>
        /// <param name="textSpan">Span of chars to analyze.</param>
        /// <param name="offset">Offset in span</param>
        /// <returns>
        ///     Length of recognized token if detected, 0 if the input string doesn't match any token or -1 if the data set
        ///     doesn't have enough character to recognize a token.
        /// </returns>
        protected abstract int TryParseToken(in ReadOnlySpan<char> textSpan, int offset);

        protected void Send(T type, in ReadOnlySpan<char> text)
        {
            _observer.OnNext(new Token<T>(type, text));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool IsInconclusive(int result)
        {
            return result == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool IsMismatch(int result)
        {
            return result < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool IsMatch(int result)
        {
            return result > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool IsMatchOrInconclusive(int result)
        {
            return result >= 0;
        }
    }
}