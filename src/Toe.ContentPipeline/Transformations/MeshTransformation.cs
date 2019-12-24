using System.Collections.Generic;

namespace Toe.ContentPipeline.Transformations
{
    public abstract class MeshTransformation : ContentVisitor
    {
        private readonly IMeshTransformation _meshTransformation;

        public MeshTransformation(IMeshTransformation meshTransformation)
        {
            _meshTransformation = meshTransformation;
        }

        public override IEnumerable<IMesh> Apply(IMesh geometry)
        {
            return _meshTransformation.Apply(geometry);
        }
    }
}