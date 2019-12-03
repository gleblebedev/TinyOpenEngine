namespace Toe.ContentPipeline
{
    public abstract class AbstractPrimitive
    {
        public AbstractPrimitive(IBufferView bufferView)
        {
            BufferView = bufferView;
        }

        public PrimitiveTopology Topology { get; set; } = PrimitiveTopology.TriangleList;

        public IBufferView BufferView { get; }

        public virtual bool DeleteStream(StreamKey key)
        {
            return false;
        }
    }
}