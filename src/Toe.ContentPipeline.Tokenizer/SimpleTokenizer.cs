using System;
using System.Runtime.CompilerServices;

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
            if (IsEndOfText(textSpan[0]))
                return 1;

            res = TryParseWhitespace(textSpan, offset);
            if (IsMatchOrInconclusive(res))
                return res;

            res = TryParseNewLine(textSpan, offset);
            if (IsMatchOrInconclusive(res)) return res;

            res = TryParseFloat(textSpan, offset);
            if (IsMatchOrInconclusive(res)) return res;

            res = TryParseSeparator(textSpan, offset);
            if (IsMatchOrInconclusive(res)) return res;

            return TryParseId(textSpan, offset);
        }

        private int TryParseSeparator(in ReadOnlySpan<char> textSpan, int offset)
        {
            if (IsSeparator(textSpan[0]))
            {
                Send(TokenType.Separator, textSpan.Slice(0,1));
                return 1;
            }
            return Mismatch;
        }

        private bool IsSeparator(char value)
        {
            switch (value)
            {
                case '!':
                    return true;
                case '#':
                    return true;
                case '$':
                    return true;
                case '%':
                    return true;
                case '&':
                    return true;
                case '(':
                    return true;
                case ')':
                    return true;
                case '*':
                    return true;
                case '+':
                    return true;
                case ',':
                    return true;
                case '-':
                    return true;
                case '.':
                    return true;
                case '/':
                    return true;
                case ':':
                    return true;
                case ';':
                    return true;
                case '<':
                    return true;
                case '=':
                    return true;
                case '>':
                    return true;
                case '?':
                    return true;
                case '@':
                    return true;
                case '[':
                    return true;
                case '\\':
                    return true;
                case ']':
                    return true;
                case '{':
                    return true;
                case '}':
                    return true;
                case '^':
                    return true;
                case '`':
                    return true;
            }
            return false;
        }

        private int TryParseId(in ReadOnlySpan<char> textSpan, int offset)
        {
            var start = offset;
            while (offset < textSpan.Length)
            {
                var c = textSpan[offset];
                if (char.IsWhiteSpace(c) || IsSeparator(c) || IsEndOfText(c) || c == '\r' || c == '\n')
                {
                    Send(TokenType.Id, textSpan.Slice(start, offset - start));
                    return offset-start;
                }
                ++offset;
            }

            return Inconslusive;
        }

        private int TryParseFloat(in ReadOnlySpan<char> textSpan, int offset)
        {
            var start = offset;

            if (textSpan[offset] == '+' || textSpan[offset] == '-')
            {
                ++offset;
            }

            if (offset >= textSpan.Length) return Inconslusive;

            bool hasInt = false;
            while (offset < textSpan.Length && char.IsDigit(textSpan[offset]))
            {
                hasInt = true;
                ++offset;
            }

            if (offset >= textSpan.Length) return Inconslusive;

            bool hasDot = false;
            bool hasExp = false;
            if (textSpan[offset] == '.')
            {
                hasDot = true;
                ++offset;
                if (offset >= textSpan.Length) return Inconslusive;
                if (!hasInt && !char.IsDigit(textSpan[offset]))
                {
                    return Mismatch;
                }
                while (offset < textSpan.Length && char.IsDigit(textSpan[offset]))
                {
                    ++offset;
                }
            }
            if (offset >= textSpan.Length) return Inconslusive;
            if (textSpan[offset] == 'e' || textSpan[offset] == 'E')
            {
                ++offset;
                if (offset >= textSpan.Length) return Inconslusive;
                if (textSpan[offset] == '+' || textSpan[offset] == '-')
                {
                    ++offset;
                }
                if (offset >= textSpan.Length) return Inconslusive;
                if (!char.IsDigit(textSpan[offset]))
                    return Mismatch;
                while (offset < textSpan.Length && char.IsDigit(textSpan[offset]))
                {
                    hasExp = true;
                    ++offset;
                }
            }
            if (offset >= textSpan.Length) return Inconslusive;

            if ((hasInt && hasExp) || hasDot)
            {
                Send(TokenType.Float, textSpan.Slice(start, offset - start));
                return offset - start;
            }
            if (hasInt && !hasExp && !hasDot)
            {
                Send(TokenType.Int, textSpan.Slice(start, offset - start));
                return offset - start;
            }

            return Mismatch;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsEndOfText(char c)
        {
            return c == 3;
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