using System;

namespace Toe.ContentPipeline
{
    public interface IStreamMetaInfo
    {
        #region Public Properties

        Type BaseType { get; }

        int ComponentsPerSet { get; }

        int NumberOfSets { get; }

        Type ValueType { get; }

        #endregion
    }
}