using System;
using Veldrid;

namespace Toe.ContentPipeline.Preview
{
    public interface IApplicationWindow
    {
        
        uint Width { get; }
        uint Height { get; }

        event Action<float> Rendering;
        event Action<GraphicsDevice, ResourceFactory, Swapchain> GraphicsDeviceCreated;
        event Action GraphicsDeviceDestroyed;
        event Action Resized;

        void Run();
    }
}