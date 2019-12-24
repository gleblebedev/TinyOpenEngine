using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IMeshTransformation
    {
        IEnumerable<IMesh> Apply(IMesh mesh);
    }
}