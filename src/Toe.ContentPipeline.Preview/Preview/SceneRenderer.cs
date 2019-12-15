using System;
using System.Collections.Generic;
using System.Linq;
using Veldrid;

namespace Toe.ContentPipeline.Preview
{
    public class SceneRenderer : IDisposable
    {
        private readonly IContentContainer _content;
        private readonly ISceneAsset _scene;
        private VeldridContext _veldrid;
        private CommandList _cl;

        private Dictionary<IImageAsset, Texture> _textures = new Dictionary<IImageAsset, Texture>();

        public SceneRenderer(IApplicationWindow window, IContentContainer content, ISceneAsset scene = null)
        {
            _content = content;
            _scene = scene ?? _content.Scenes.First();
            Window = window;
            Window.GraphicsDeviceCreated += OnGraphicsDeviceCreated;
            Window.GraphicsDeviceDestroyed += OnGraphicsDeviceDestroyed;
            Window.Rendering += PreDraw;
            Window.Rendering += Draw;
        }

        private void OnGraphicsDeviceDestroyed()
        {
            _cl.Dispose();
            _cl = null;
        }

        private void OnGraphicsDeviceCreated(VeldridContext veldrid)
        {
            _veldrid = veldrid;
            _cl = _veldrid.Resources.CreateCommandList();

            foreach (var contentImage in _content.Images)
            {
                CreateTexture(contentImage);
            }
            foreach (var mesh in _content.Meshes)
            {
                CreateMesh(mesh);
            }
            foreach (var node in _scene.ChildNodes)
            {
                CreateNode(null, node);
            }
        }

        private void CreateNode(INodeAsset parentNode, INodeAsset node)
        {
            
        }

        private void CreateMesh(IMesh mesh)
        {
            var gpuMesh = mesh.ToGpuMesh();
            var bufferViews = new Dictionary<IBufferView, object>();
            foreach (var bufferView in mesh.BufferViews)
            {
                bufferViews.Add(bufferView, null);
            }
        }

        private void CreateTexture(IImageAsset imageAsset)
        {
            var image = new Veldrid.ImageSharp.ImageSharpTexture(imageAsset.OpenAsync().Result);
            var texture = image.CreateDeviceTexture(_veldrid.Graphics, _veldrid.Resources);
            _textures[imageAsset] = texture;
        }

        private void Draw(float deltaSeconds)
        {
            _cl.Begin();
            _cl.SetFramebuffer(MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, RgbaFloat.LightGrey);
            _cl.ClearDepthStencil(1f);
            _cl.End();
            GraphicsDevice.SubmitCommands(_cl);
            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
        GraphicsDevice GraphicsDevice
        {
            get { return _veldrid.Graphics; }
        }
        Swapchain MainSwapchain
        {
            get { return _veldrid.Swapchain; }
        }
        private void PreDraw(float deltaSeconds)
        {
        }

        public IApplicationWindow Window { get; set; }

        public void Dispose()
        {
        }
    }
}
