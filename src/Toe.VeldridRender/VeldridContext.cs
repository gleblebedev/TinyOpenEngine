using Veldrid;

namespace Toe.VeldridRender
{
    public class VeldridContext
    {
        public VeldridContext(GraphicsDevice graphicsDevice) : this(graphicsDevice, graphicsDevice.ResourceFactory, graphicsDevice.MainSwapchain)
        {

        }
        public VeldridContext(GraphicsDevice graphicsDevice, Swapchain swapchain): this(graphicsDevice, graphicsDevice.ResourceFactory, swapchain)
        {

        }
        public VeldridContext(GraphicsDevice graphicsDevice, ResourceFactory resourceFactory, Swapchain swapchain)
        {
            GraphicsDevice = graphicsDevice;
            ResourceFactory = resourceFactory;
            Swapchain = swapchain;
        }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public ResourceFactory ResourceFactory { get; private set; }
        public Swapchain Swapchain { get; private set; }
    }
}