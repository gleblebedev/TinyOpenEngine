using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public ref struct Token<T>
    {
        public static implicit operator TokenValue<T>(in Token<T> rhs)
        {
            return new TokenValue<T>(rhs);
        }

        public bool Equals(Token<T> other)
        {
            return Type.Equals(other) && _text.SequenceCompareTo(other._text) == 0;
        }

        public override bool Equals(object other)
        {
            throw new NotImplementedException("This method should never be executed");
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type.GetHashCode();
                for (var index = 0; index < _text.Length; index++)
                {
                    var c = _text[index];
                    hashCode = (hashCode * 397) ^ c.GetHashCode();
                }

                return hashCode;
            }
        }

        public static bool operator ==(Token<T> left, Token<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Token<T> left, Token<T> right)
        {
            return !left.Equals(right);
        }

        private readonly ReadOnlySpan<char> _text;

        public Token(T type, ReadOnlySpan<char> text)
        {
            Type = type;
            _text = text;
        }

        public override string ToString()
        {
            return _text.ToString();
        }

        public T Type { get; }

        public int Length => _text.Length;

        public char this[int index] => _text[index];
    }
}