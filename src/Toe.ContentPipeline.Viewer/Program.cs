using System;
using System.IO;
using CommandLine;
using Toe.ContentPipeline.Preview;

namespace Toe.ContentPipeline.Viewer
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<ViewerOptions>(args) as Parsed<ViewerOptions>;

            if (!File.Exists(options.Value.FileName))
            {
                throw new FileNotFoundException("File not found "+options.Value.FileName, options.Value.FileName);
            }

            VeldridStartupWindow window = new VeldridStartupWindow("glTF Viewer", options?.Value ?? new ViewerOptions());

            SceneRenderer sceneRenderer = new SceneRenderer(window, options.Value.FileName);
            window.Run();
        }
    }
}
