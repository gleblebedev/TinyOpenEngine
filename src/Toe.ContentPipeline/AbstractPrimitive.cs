namespace Toe.ContentPipeline
{
    public abstract class AbstractPrimitive
    {
        public PrimitiveTopology Topology { get; set; } = PrimitiveTopology.TriangleList;

        public AbstractPrimitive(IBufferView bufferView)
        {
            BufferView = bufferView;
        }

        public IBufferView BufferView { get; }

        public virtual bool DeleteStream(StreamKey key)
        {
            return false;
        }

    }
}