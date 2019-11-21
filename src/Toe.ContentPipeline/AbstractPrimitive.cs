namespace Toe.ContentPipeline
{
    public abstract class AbstractPrimitive
    {
        public PrimitiveTopology Topology { get; set; } = PrimitiveTopology.TriangleList;
    }
}