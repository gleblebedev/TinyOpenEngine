using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Toe.ContentPipeline.VeldridMesh;
using Toe.SceneGraph;
using Toe.VeldridRender;
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
        private Dictionary<IMesh, VeldridGeometry> _meshes = new Dictionary<IMesh, VeldridGeometry>();

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
            _cl = _veldrid.ResourceFactory.CreateCommandList();

            foreach (var imageAsset in _content.Images)
            {
                CreateTexture(imageAsset);
            }
            foreach (var materialAsset in _content.Materials)
            {
                CreateMaterial(materialAsset);
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
            _meshes.Add(mesh, VeldridGeometry.Create(mesh));
        }

        private void CreateTexture(IImageAsset imageAsset)
        {
            var image = new Veldrid.ImageSharp.ImageSharpTexture(imageAsset.OpenAsync().Result);
            var texture = image.CreateDeviceTexture(_veldrid.GraphicsDevice, _veldrid.ResourceFactory);
            _textures[imageAsset] = texture;
        }
        private void CreateMaterial(IMaterialAsset materialAsset)
        {
            
        }


        private void Draw(float deltaSeconds)
        {
            VeldridGeometry mesh = new VeldridGeometry();
   
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
            get { return _veldrid.GraphicsDevice; }
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
