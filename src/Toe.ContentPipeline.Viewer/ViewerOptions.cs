using CommandLine;
using Veldrid;

namespace Toe.ContentPipeline.Viewer
{
    public class ViewerOptions
    {
        [Option('g', "graphics")] public GraphicsBackend? GraphicsBackend { get; set; }

        [Option('w', "windowstate")] public WindowState WindowState { get; set; } = WindowState.Normal;

        [Option('i', "input", Required = true)] public string FileName { get; set; }

        [Option('r', "renderdoc")] public bool RenderDoc { get; set; }
    }
}