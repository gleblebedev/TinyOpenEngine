using System.Collections;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public abstract class StreamConverter<TRes> : IReadOnlyList<TRes>
    {
        #region Public Indexers

        /// <summary>
        ///     Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        ///     The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="index" /> is not a valid index in the
        ///     <see cref="T:System.Collections.Generic.IList`1" />.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The property is set and the
        ///     <see cref="T:System.Collections.Generic.IList`1" /> is read-only.
        /// </exception>
        public abstract TRes this[int index] { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///     The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public abstract int Count { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        public class Enumerator : IEnumerator<TRes>
        {
            #region Constructors and Destructors

            public Enumerator(StreamConverter<TRes> converter)
            {
                this.converter = converter;
            }

            #endregion

            #region Public Properties

            public TRes Current => converter[position];

            #endregion

            #region Explicit Interface Properties

            object IEnumerator.Current => Current;

            #endregion

            #region Constants and Fields

            private readonly StreamConverter<TRes> converter;

            private int position = -1;

            #endregion

            #region Public Methods and Operators

            public virtual void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (position + 1 >= converter.Count) return false;
                ++position;
                return true;
            }

            public void Reset()
            {
                position = -1;
            }

            #endregion
        }

        #region Explicit Interface Methods

        IEnumerator<TRes> IEnumerable<TRes>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}