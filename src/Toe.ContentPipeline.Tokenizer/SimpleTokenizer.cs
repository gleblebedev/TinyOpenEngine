using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public class SimpleTokenizer : AbstractTokenizer<SimpleTokenizer.TokenType>
    {
        public enum TokenType
        {
            Whitespace,
            NewLine,
            Int,
            Float,
            Id,
            String,
            Separator,

            StringConstant,

            CharConstant
        }

        public SimpleTokenizer(ITokenObserver<TokenType> observer, ITokenEncoding encoding) : base(observer, encoding) { }
        public SimpleTokenizer(ITokenObserver<TokenType> observer) : base(observer) { }

        public SimpleTokenizer(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy) : base(observer, allocationStrategy) { }

        public SimpleTokenizer(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy, ITokenEncoding encoding):base(observer, allocationStrategy, encoding) { }

        protected override int TryParseToken(in ReadOnlySpan<char> textSpan, int offset)
        {
            switch (textSpan[offset])
            {
                case ' ':
                    return TryParseWhitespace(textSpan, offset + 1);
                default:
                    return -1;
            }
            if (char.IsWhiteSpace(textSpan[0]))
            {
                for (int i = 1; i < textSpan.Length; ++i)
                {
                    if (!char.IsWhiteSpace(textSpan[i]) || textSpan[i] == EndOfText)
                    {
                        Send(TokenType.Whitespace, textSpan.Slice(0,i));
                        return i;
                    }
                }

                return -1;
            }
            else
            {
                if (textSpan[0] == EndOfText)
                    return 1;
                for (int i = 1; i < textSpan.Length; ++i)
                {
                    if (char.IsWhiteSpace(textSpan[i]) || textSpan[i] == EndOfText)
                    {
                        Send(TokenType.Id, textSpan.Slice(0, i));
                        return i;
                    }
                }
                return -1;
            }
        }

        private int TryParseWhitespace(in ReadOnlySpan<char> textSpan, int offset)
        {
            while (offset < textSpan.Length)
            {
                switch (textSpan[offset])
                {
                    case ' ':
                        ++offset;
                        break;
                    default:
                        return offset;
                }
            }

            return offset;
        }
    }
}