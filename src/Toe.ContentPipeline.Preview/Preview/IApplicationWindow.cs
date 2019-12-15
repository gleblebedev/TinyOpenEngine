using System;
using Veldrid;

namespace Toe.ContentPipeline.Preview
{
    public interface IApplicationWindow
    {
        
        uint Width { get; }
        uint Height { get; }

        event Action<float> Rendering;
        event Action<VeldridContext> GraphicsDeviceCreated;
        event Action GraphicsDeviceDestroyed;
        event Action Resized;

        void Run();
    }
}