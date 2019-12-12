namespace Toe.ContentPipeline.Tokenizer
{
    public struct TokenValue<T>
    {
        public TokenValue(Token<T> token)
        {
            Type = token.Type;
            Value = token.ToString();
        }

        public string Value { get; set; }

        public T Type { get; }

        public override string ToString()
        {
            return Value;
        }
    }
}