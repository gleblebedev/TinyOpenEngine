using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class MeshInstance: IMeshInstance
    {
        public MeshInstance(IMesh mesh, IList<IMaterialAsset> materials)
        {
            Mesh = mesh;
            Materials = materials;
        }
        public IMesh Mesh { get; }
        public IList<IMaterialAsset> Materials { get; }

        public override string ToString()
        {
            return Mesh?.ToString() ?? base.ToString();
        }
    }
}