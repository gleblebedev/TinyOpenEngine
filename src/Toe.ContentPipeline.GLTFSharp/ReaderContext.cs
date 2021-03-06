﻿using System.Collections.Generic;
using SharpGLTF.Schema2;

namespace Toe.ContentPipeline.GLTFSharp
{
    public class ReaderContext
    {
        public ModelRoot ModelRoot { get; set; }
        public ContentContainer Container { get; set; }
        public IReadOnlyList<IImageAsset> Images { get; set; }
        public IReadOnlyList<IMesh> Meshes { get; set; }
        public IReadOnlyList<IMaterialAsset> Materials { get; set; }
        public IReadOnlyList<INodeAsset> Nodes { get; set; }
        public IReadOnlyList<ICameraAsset> Cameras { get; set; }
        public IReadOnlyList<ILightAsset> Lights { get; set; }
    }
}