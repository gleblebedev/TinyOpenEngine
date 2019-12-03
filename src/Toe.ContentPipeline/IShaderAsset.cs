using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IShaderAsset
    {
        IEnumerable<IShaderParameter> Parameters { get; }
        void Set(IShaderParameter shaderParameter);
    }
}