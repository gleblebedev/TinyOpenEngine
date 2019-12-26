using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IFileFormat
    {
        /// <summary>
        ///     Scene file format extensions.
        /// </summary>
        IReadOnlyCollection<string> Extensions { get; }

        /// <summary>
        ///     Scene file format name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Preferable read mode.
        /// </summary>
        ReaderMode PreferableReadMode { get; }

        /// <summary>
        ///     Check if there is a reader implemented for a file format.
        /// </summary>
        /// <returns>True if there is a reader available.</returns>
        bool CanLoad();

        /// <summary>
        ///     Check file format could be readed from specific file.
        /// </summary>
        /// <returns>True if there is a reader available.</returns>
        bool CanLoadFrom(string fileName);

        /// <summary>
        ///     Check if there is a writer implemented for a file format.
        /// </summary>
        /// <returns>True if there is a writer available.</returns>
        bool CanSave();

        /// <summary>
        ///     Check file format could be saved with specific file extension.
        /// </summary>
        /// <returns>True if there is a writer available.</returns>
        bool CanSaveAs(string fileName);

        /// <summary>
        ///     Creates the reader.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Scene reader.</returns>
        IReactiveReader CreateReactiveReader(ReaderContext context);

        /// <summary>
        ///     Creates the reader.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Scene reader.</returns>
        IStreamReader CreateProactiveReader(ReaderContext context);

        /// <summary>
        ///     Creates the writer.
        /// </summary>
        /// <returns>Scene writer.</returns>
        IContentWriter CreateWriter();
    }
}