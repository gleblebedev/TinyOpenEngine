using System;
using System.Text;

namespace Toe.ContentPipeline
{
    public interface IContentTransformation
    {
        IContentContainer Apply(IContentContainer content);
    }
}
