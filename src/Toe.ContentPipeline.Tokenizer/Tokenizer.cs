using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public class SimpleTokenizer : AbstractTokenizer<TokenType>
    {
        public SimpleTokenizer(ITokenObserver<TokenType> observer) : this(observer, DefaultEncoding)
        {
        }

        public SimpleTokenizer(ITokenObserver<TokenType> observer, ITokenEncoding encoding) : base(observer, encoding)
        {
        }

        protected override int TryParseToken(in ReadOnlySpan<char> textSpan)
        {
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
    }
}