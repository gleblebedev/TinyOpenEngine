using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public abstract class AbstractTokenizer<T>: ITokenizer
    {
        public const char EndOfText = (char) 3;
        private byte[] _endOfTextArray = new byte[]{3};
        public static readonly ITokenEncoding DefaultEncoding = new AsciiTokenEncoding();
        private readonly ITokenEncoding _encoding;
        private readonly ITokenObserver<T> _observer;
        private readonly IAllocationStrategy _allocationStrategy;
        private char[] _buffer = null;
        private int _end;
        private int _position;

        public AbstractTokenizer(ITokenObserver<T> observer) : this(observer, new ArrayPoolAllocationStrategy(), DefaultEncoding)
        {
        }

        public AbstractTokenizer(ITokenObserver<T> observer, ITokenEncoding encoding) : this(observer, new ArrayPoolAllocationStrategy(), encoding)
        {
        }

        public AbstractTokenizer(ITokenObserver<T> observer, IAllocationStrategy allocationStrategy) : this(observer, allocationStrategy, DefaultEncoding)
        {
        }

        public AbstractTokenizer(ITokenObserver<T> observer, IAllocationStrategy allocationStrategy, ITokenEncoding encoding)
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
                _buffer = _allocationStrategy.Rent(estimatedCount*2);
            }
            else if (_end + estimatedCount > _buffer.Length)
            {
                if (estimatedCount < _position && _position > 32)
                {
                    new Span<char>(_buffer, _position, _end-_position).CopyTo(new Span<char>(_buffer, 0, _end - _position));
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

            _end += _encoding.GetString(value, new Span<char>(_buffer,_end, estimatedCount));

            for (;_position < _end;)
            {
                var textSpan = new ReadOnlySpan<char>(_buffer, _position, _end - _position);
                var tokenLen = TryParseToken(textSpan);
                if (tokenLen < 0)
                    return;
                _position += tokenLen;
            }
        }

        protected abstract int TryParseToken(in ReadOnlySpan<char> textSpan);

        protected void Send(T type, in ReadOnlySpan<char> text)
        {
            _observer.OnNext(new Token<T>(type, text));
        }
    }
}