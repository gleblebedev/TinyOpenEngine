using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public abstract class AbstractTokenizer<T>: ITokenizer
    {
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
            Process(ReadOnlySpan<byte>.Empty);
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
                var buffer = _allocationStrategy.Rent(Math.Max(_buffer.Length * 2, _end + estimatedCount));
                new Span<char>(_buffer, _position, _end - _position).CopyTo(buffer);
                _end = _end - _position;
                _position = 0;
            }

            _end += _encoding.GetString(value, new Span<char>(_buffer,_end, estimatedCount)).Length;

            throw new NotImplementedException();
        }

        protected void Send(T type, in ReadOnlySpan<char> text)
        {
            _observer.OnNext(new Token<T>(type, text));
        }
    }
}