using Veldrid;

namespace Toe.ContentPipeline.Preview
{
    public class VeldridContext
    {
        private readonly GraphicsDevice _graphics;
        private readonly ResourceFactory _resources;
        private readonly Swapchain _swapchain;

        public VeldridContext(GraphicsDevice graphics, ResourceFactory resources, Swapchain swapchain)
        {
            _graphics = graphics;
            _resources = resources;
            _swapchain = swapchain;
        }

        public GraphicsDevice Graphics
        {
            get => _graphics;
        }
        public ResourceFactory Resources
        {
            get => _resources;
        }
        public Swapchain Swapchain
        {
            get => _swapchain;
        }
    }
}