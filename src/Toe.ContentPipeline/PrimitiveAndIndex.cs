namespace Toe.ContentPipeline
{
    public struct PrimitiveAndIndex<T>
    {
        public PrimitiveAndIndex(T primitive, int index)
        {
            Primitive = primitive;
            Index = index;
        }

        public static implicit operator int(PrimitiveAndIndex<T> primitive)
        {
            return primitive.Index;
        }

        public T Primitive { get; }

        public int Index { get; }
    }
}