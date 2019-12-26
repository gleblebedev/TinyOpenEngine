using System.IO;

namespace Toe.ContentPipeline
{
    /// <summary>
    ///     File serializer interface.
    /// </summary>
    public interface IContentWriter
    {
        void Write(Stream fileStream, string fileName, IContentContainer content);
    }
}