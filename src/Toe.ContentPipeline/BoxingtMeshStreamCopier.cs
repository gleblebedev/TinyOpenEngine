namespace Toe.ContentPipeline
{
    public sealed class BoxingtMeshStreamCopier: AbstractMeshStreamCopier
    {
        private readonly IMeshStream _source;
        private readonly IMeshStream _destination;

        public BoxingtMeshStreamCopier(IMeshStream source, IMeshStream destination)
        {
            _source = source;
            _destination = destination;
        }
        public override int Copy(int sourceIndex)
        {
            return _destination.Add(_source[sourceIndex]);
        }
    }
}