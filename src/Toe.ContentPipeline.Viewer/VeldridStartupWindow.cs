using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Toe.ContentPipeline.Preview;
using Toe.VeldridRender;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Veldrid.Utilities;

namespace Toe.ContentPipeline.Viewer
{
    public class VeldridStartupWindow : IApplicationWindow
    {
        private readonly ViewerOptions _options;
        private readonly Sdl2Window _window;
        private bool _windowResized = true;
        private VeldridContext _veldrid;

        public VeldridStartupWindow(string title, ViewerOptions options)
        {
            _options = options;
            var wci = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = 1280,
                WindowHeight = 720,
                WindowTitle = title,
                WindowInitialState = _options.WindowState
            };
            _window = VeldridStartup.CreateWindow(ref wci);
            _window.Resized += () => { _windowResized = true; };
            _window.KeyDown += OnKeyDown;
        }

        public event Action<float> Rendering;
        public event Action<VeldridContext> GraphicsDeviceCreated;
        public event Action GraphicsDeviceDestroyed;
        public event Action Resized;

        public uint Width => (uint)_window.Width;
        public uint Height => (uint)_window.Height;

        public void Run()
        {
            var options = new GraphicsDeviceOptions(
                false,
                PixelFormat.R16_UNorm,
                true,
                ResourceBindingModel.Improved,
                true,
                true);

#if DEBUG
            options.Debug = true;
#endif
            GraphicsDevice gd;
            if (_options.GraphicsBackend.HasValue)
                gd = VeldridStartup.CreateGraphicsDevice(_window, options, _options.GraphicsBackend.Value);
            else
                gd = VeldridStartup.CreateGraphicsDevice(_window, options);
            var factory = new DisposeCollectorResourceFactory(gd.ResourceFactory);

            _veldrid = new VeldridContext(gd, factory, gd.MainSwapchain);
            GraphicsDeviceCreated?.Invoke(_veldrid);

            var sw = Stopwatch.StartNew();
            var previousElapsed = sw.Elapsed.TotalSeconds;

            while (_window.Exists)
            {
                var newElapsed = sw.Elapsed.TotalSeconds;
                var deltaSeconds = (float)(newElapsed - previousElapsed);

                var inputSnapshot = _window.PumpEvents();

                if (_window.Exists)
                {
                    previousElapsed = newElapsed;
                    if (_windowResized)
                    {
                        _windowResized = false;
                        gd.ResizeMainWindow((uint)_window.Width, (uint)_window.Height);
                        Resized?.Invoke();
                    }

                    Rendering?.Invoke(deltaSeconds);
                }
            }

            gd.WaitForIdle();
            factory.DisposeCollector.DisposeAll();
            gd.Dispose();
            GraphicsDeviceDestroyed?.Invoke();
        }

        public event Action<KeyEvent> KeyPressed;

        protected void OnKeyDown(KeyEvent keyEvent)
        {
            KeyPressed?.Invoke(keyEvent);
        }
    }
}
