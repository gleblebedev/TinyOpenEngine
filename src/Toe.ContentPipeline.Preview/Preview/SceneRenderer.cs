using System;

namespace Toe.ContentPipeline.Preview
{
    public class SceneRenderer : IDisposable
    {
        public SceneRenderer(IApplicationWindow window, string file)
        {
            Window = window;

            Window.Rendering += PreDraw;
            Window.Rendering += Draw;
        }

        private void Draw(float deltaSeconds)
        {
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
