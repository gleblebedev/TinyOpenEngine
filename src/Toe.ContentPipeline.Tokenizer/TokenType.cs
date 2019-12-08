namespace Toe.ContentPipeline.Tokenizer
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
}