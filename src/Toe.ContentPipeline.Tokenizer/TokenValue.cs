namespace Toe.ContentPipeline.Tokenizer
{
    public struct TokenValue<T>
    {
        public TokenValue(Token<T> token)
        {
            this.Type = token.Type;
            this.Value = token.ToString();
        }

        public string Value { get; set; }

        public T Type { get; }

        public override string ToString()
        {
            return Value;
        }
    }
}