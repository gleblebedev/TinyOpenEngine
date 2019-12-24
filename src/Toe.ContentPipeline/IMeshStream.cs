using System.Collections;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public interface IMeshStream<T> : IList<T>, IMeshStream
    {
        #region Public Methods and Operators

        new int Add(T value);

        IList<int> AddRange(IEnumerable<T> items);

        #endregion
    }

    public interface IMeshStream : IList
    {
        #region Public Properties

        IStreamConverterFactory ConverterFactory { get; set; }

        IStreamMetaInfo MetaInfo { get; }

        #endregion

        #region Public Methods and Operators

        void AddDefault(int count = 1);


        IReadOnlyList<TValue> GetReader<TValue>();

        /// <summary>
        ///     Compare to items in stream.
        /// </summary>
        /// <param name="index0">First item to compare.</param>
        /// <param name="index1">Second item to compare.</param>
        /// <returns>True if items are equal</returns>
        bool AreEqual(int index0, int index1);

        /// <summary>
        ///     Get item hash code.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns>Item hash code.</returns>
        int GetItemHashCode(int index);

        /// <summary>
        ///     Create a list mesh stream of the same type.
        /// </summary>
        /// <returns>New empty list mesh stream.</returns>
        IMeshStream CreateListMeshStreamOfTheSameType();

        /// <summary>
        ///     Create a dictionary mesh stream of the same type.
        /// </summary>
        /// <returns>New empty dictionary mesh stream.</returns>
        IMeshStream CreateDictionaryMeshStreamOfTheSameType();

        /// <summary>
        ///     Creates an exact copy of the stream
        /// </summary>
        /// <returns>Cloned stream</returns>
        IMeshStream Clone();

        #endregion
    }
}