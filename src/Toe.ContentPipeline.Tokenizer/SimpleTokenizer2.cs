using System;

namespace Toe.ContentPipeline.Tokenizer
{
    public partial class SimpleTokenizer2 : AbstractTokenizer<SimpleTokenizer2.TokenType>
    {
        public enum TokenType
        {
            Integer,
            Float,
            StringConstant,
            Whitespace,
            NewLine,
        }

        public SimpleTokenizer2(ITokenObserver<TokenType> observer, ITokenEncoding encoding) : base(observer, encoding) { }
        public SimpleTokenizer2(ITokenObserver<TokenType> observer) : base(observer) { }
        public SimpleTokenizer2(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy) : base(observer, allocationStrategy) { }
        public SimpleTokenizer2(ITokenObserver<TokenType> observer, IAllocationStrategy allocationStrategy, ITokenEncoding encoding):base(observer, allocationStrategy, encoding) { }

        protected override int TryParseToken(in ReadOnlySpan<char> textSpan, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
