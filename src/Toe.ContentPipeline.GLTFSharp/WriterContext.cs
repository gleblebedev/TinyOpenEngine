using System.Collections.Generic;
using SharpGLTF.Schema2;

namespace Toe.ContentPipeline.GLTFSharp
{
    public class WriterContext
    {
        public IContentContainer Container { get; set; }
        public ModelRoot ModelRoot { get; set; }
        public Dictionary<IImageAsset, Image> Textures { get; } = new Dictionary<IImageAsset, Image>();
        public Dictionary<IMaterialAsset, Material> Materials { get; } = new Dictionary<IMaterialAsset, Material>();
        public Dictionary<IMesh, Mesh> Meshes { get; } = new Dictionary<IMesh, Mesh>();

        public Dictionary<IMesh, IList<IMaterialAsset>> MeshInstances { get; } =
            new Dictionary<IMesh, IList<IMaterialAsset>>();
    }
}