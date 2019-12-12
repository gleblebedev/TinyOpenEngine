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

        public SimpleTokenizer(ITokenObserver<TokenType> observer, ITokenEncoding encoding) : base(observer, encoding)
        {
        }

        public SimpleTokenizer(ITokenObserver<TokenType> observer) : base(observer)
        {
        }

        public SimpleTokenizer(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy) : base(
            observer, allocationStrategy)
        {
        }

        public SimpleTokenizer(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy,
            ITokenEncoding encoding) : base(observer, allocationStrategy, encoding)
        {
        }

        protected override int TryParseToken(in ReadOnlySpan<char> textSpan, int offset)
        {
            int res;

            //End of file
            if (textSpan[0] == 3)
                return 1;

            res = TryParseWhitespace(textSpan, offset);
            if (IsMatchOrInconclusive(res))
                return res;

            res = TryParseNewLine(textSpan, offset);
            if (IsMatch(res)) return res;

            return TryParseId(textSpan, offset);
        }

        private int TryParseId(in ReadOnlySpan<char> textSpan, int offset)
        {
            var start = offset;
            while (offset < textSpan.Length)
            {
                if (char.IsWhiteSpace(textSpan[offset]))
                    break;
                if (textSpan[offset] == '\r' || textSpan[offset] == '\n')
                    break;
                ++offset;
            }

            if (start > 0)
            {
                Send(TokenType.Id, textSpan.Slice(0, offset));
                return start;
            }

            return Mismatch;
        }

        private int TryParseNewLine(in ReadOnlySpan<char> textSpan, int offset)
        {
            if (textSpan.Length - offset < 2)
                return Inconslusive;
            if (textSpan[offset] == '\n')
            {
                if (textSpan[offset + 1] == '\r')
                {
                    Send(TokenType.NewLine, textSpan.Slice(0, 2));
                    return 2;
                }

                Send(TokenType.NewLine, textSpan.Slice(0, 1));
                return 1;
            }

            if (textSpan[offset] == '\r')
            {
                if (textSpan[offset + 1] == '\n')
                {
                    Send(TokenType.NewLine, textSpan.Slice(0, 2));
                    return 2;
                }

                Send(TokenType.NewLine, textSpan.Slice(0, 1));
                return 1;
            }

            return Mismatch;
        }


        private int TryParseWhitespace(in ReadOnlySpan<char> textSpan, int offset)
        {
            if (!char.IsWhiteSpace(textSpan[offset])) return Mismatch;
            while (offset < textSpan.Length)
            {
                if (!char.IsWhiteSpace(textSpan[offset]))
                {
                    Send(TokenType.Whitespace, textSpan.Slice(0, offset));
                    return offset;
                }

                ++offset;
            }

            return Inconslusive;
        }
    }
}