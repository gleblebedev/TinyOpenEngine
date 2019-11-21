namespace Toe.ContentPipeline
{
    public abstract class AbstractAsset : IAsset
    {
        public AbstractAsset(string id)
        {
            Id = id ?? string.Empty;
        }

        public string Id { get; }

        public override string ToString()
        {
            return $"({GetType().Name}){Id}";
        }
    }
}