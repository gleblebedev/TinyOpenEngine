using System;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class StreamConverterImpl<T, TRes> : StreamConverter<TRes>
    {
        private readonly Func<T, TRes> converter;

        private readonly IList<T> source;

        public StreamConverterImpl(Func<T, TRes> converter, IList<T> source)
        {
            this.converter = converter;
            this.source = source;
        }

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///     The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public override int Count
        {
            get { return source.Count; }
        }

        public override TRes this[int index]
        {
            get { return converter(source[index]); }
            set { throw new NotImplementedException(); }
        }
    }
}