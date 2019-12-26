using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline
{
    public abstract class AbstractFileFormat : IFileFormat
    {
        private string[] _extensions;

        #region Methods

        private bool MatchExtensions(string fileName)
        {
            return Extensions.Any(_ => fileName.EndsWith(_, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Scene file format extensions.
        /// </summary>
        public virtual IReadOnlyCollection<string> Extensions
        {
            get { return _extensions ?? (_extensions = new[] {"." + Name.ToLower()}); }
        }

        /// <summary>
        ///     Scene file format name.
        /// </summary>
        public abstract string Name { get; }

        public virtual ReaderMode PreferableReadMode => ReaderMode.Proactive;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Check if there is a reader implemented for a file format.
        /// </summary>
        /// <returns>True if there is a reader available.</returns>
        public virtual bool CanLoad()
        {
            return false;
        }

        /// <summary>
        ///     Check file format could be readed from specific file.
        /// </summary>
        /// <returns>True if there is a reader available.</returns>
        public virtual bool CanLoadFrom(string fileName)
        {
            return CanLoad() && MatchExtensions(fileName);
        }

        /// <summary>
        ///     Check if there is a writer implemented for a file format.
        /// </summary>
        /// <returns>True if there is a writer available.</returns>
        public virtual bool CanSave()
        {
            return false;
        }

        /// <summary>
        ///     Check file format could be saved with specific file extension.
        /// </summary>
        /// <returns>True if there is a writer available.</returns>
        public virtual bool CanSaveAs(string fileName)
        {
            return CanSave() && MatchExtensions(fileName);
        }

        public virtual IStreamReader CreateProactiveReader(ReaderContext context)
        {
            var reactiveReader = CreateReactiveReader(context);
            if (reactiveReader is ProactiveToReactiveReaderAdapter)
                throw new NotImplementedException();
            return new ReactiveToProactiveReaderAdapter(reactiveReader);
        }

        /// <summary>
        ///     Creates the reader.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Scene reader.</returns>
        public virtual IReactiveReader CreateReactiveReader(ReaderContext context)
        {
            var proactiveReader = CreateProactiveReader(context);
            if (proactiveReader is ReactiveToProactiveReaderAdapter)
                throw new NotImplementedException();
            return new ProactiveToReactiveReaderAdapter(proactiveReader);
        }

        /// <summary>
        ///     Creates the writer.
        /// </summary>
        /// <returns>Scene writer.</returns>
        public virtual IContentWriter CreateWriter()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}