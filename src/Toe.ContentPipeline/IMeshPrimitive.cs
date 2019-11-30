namespace Toe.ContentPipeline
{
    public interface IMeshPrimitive
    {
        PrimitiveTopology Topology { get; }

        IBufferView BufferView { get; }

        bool DeleteStream(StreamKey key);
    }
}