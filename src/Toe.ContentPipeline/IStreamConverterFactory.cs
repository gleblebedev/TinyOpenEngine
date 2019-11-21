using System;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IStreamConverterFactory
    {
        #region Public Methods and Operators

        IStreamMetaInfo GetMetaInfo(Type type);

        StreamConverter<TRes> ResolveConverter<T, TRes>(IList<T> arrayMeshStream);

        #endregion
    }
}