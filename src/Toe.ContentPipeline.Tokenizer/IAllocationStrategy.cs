namespace Toe.ContentPipeline.Tokenizer
{
    public interface IAllocationStrategy
    {
        char[] Rent(int minSize);
        void Return(char[] buffer);
    }
}