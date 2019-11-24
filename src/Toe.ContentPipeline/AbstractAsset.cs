namespace Toe.ContentPipeline
{
    public abstract class AbstractAsset : IAsset
    {
        private ImmutableId _id;

        public AbstractAsset(string id)
        {
            _id = new ImmutableId(id);
        }

        public AbstractAsset()
        {
        }

        public string Id
        {
            get => _id.Id;
            set => _id.Id = value;
        }

        public override string ToString()
        {
            return $"({GetType().Name}){Id}";
        }
    }
}