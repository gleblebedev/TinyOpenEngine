namespace Toe.ContentPipeline.Tokenizer
{
    public class DefaultTokenizer : AbstractTokenizer<DefaultTokenType>
    {
        public DefaultTokenizer(ITokenObserver<DefaultTokenType> observer) : this(observer, DefaultEncoding)
        {
        }

        public DefaultTokenizer(ITokenObserver<DefaultTokenType> observer, ITokenEncoding encoding) : base(observer, encoding)
        {
        }

      
    }
}