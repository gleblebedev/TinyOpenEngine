namespace Toe.ContentPipeline
{
    public struct PrimitiveAndIndex<T>
    {
        private readonly T _primitive;
        private readonly int _index;

        public PrimitiveAndIndex(T primitive, int index)
        {
            _primitive = primitive;
            _index = index;
        }
        public static implicit operator int (PrimitiveAndIndex<T> primitive)
        {
            return primitive.Index;
        }

        public T Primitive
        {
            get => _primitive;
        }

        public int Index
        {
            get => _index;
        }
    }
}