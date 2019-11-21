using System;
using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline
{
    public class ListMeshStream<T> : List<T>, IMeshStream<T>
    {
        public ListMeshStream(IStreamConverterFactory converterFactory = null, IStreamMetaInfo metaInfo = null)
        {
            ConverterFactory = converterFactory ?? StreamConverterFactory.Default;
            MetaInfo = metaInfo ?? ConverterFactory.GetMetaInfo(typeof(T));
        }

        public ListMeshStream(int capacity, IStreamConverterFactory converterFactory = null, IStreamMetaInfo metaInfo = null)
            : base(capacity)
        {
            ConverterFactory = converterFactory ?? StreamConverterFactory.Default;
            MetaInfo = metaInfo ?? ConverterFactory.GetMetaInfo(typeof(T));
        }
        public ListMeshStream(IEnumerable<T> data, IStreamConverterFactory converterFactory = null, IStreamMetaInfo metaInfo = null)
            : base()
        {
            ConverterFactory = converterFactory ?? StreamConverterFactory.Default;
            MetaInfo = metaInfo ?? ConverterFactory.GetMetaInfo(typeof(T));
            AddRange(data);
        }
        /// <summary>
        ///     Compare to items in stream.
        /// </summary>
        /// <param name="index0">First item to compare.</param>
        /// <param name="index1">Second item to compare.</param>
        /// <returns>True if items are equal</returns>
        public bool AreEqual(int index0, int index1)
        {
            return this[index0].Equals(this[index1]);
        }

        /// <summary>
        ///     Get item hash code.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns>Item hash code.</returns>
        public int GetItemHashCode(int index)
        {
            return this[index].GetHashCode();
        }

        /// <summary>
        ///     Create a list mesh stream of the same type.
        /// </summary>
        /// <returns>New empty list mesh stream.</returns>
        public IMeshStream CreateListMeshStreamOfTheSameType()
        {
            return new ListMeshStream<T>(ConverterFactory, MetaInfo);
        }

        /// <summary>
        ///     Create a dictionary mesh stream of the same type.
        /// </summary>
        /// <returns>New empty dictionary mesh stream.</returns>
        public IMeshStream CreateDictionaryMeshStreamOfTheSameType()
        {
            return new DictionaryMeshStream<T>(ConverterFactory, MetaInfo);
        }

        public IMeshStream Clone()
        {
            return new ListMeshStream<T>(this, ConverterFactory, MetaInfo);
        }

        public IStreamConverterFactory ConverterFactory { get; set; } = StreamConverterFactory.Default;

        public IStreamMetaInfo MetaInfo { get; }

        #region Public Methods and Operators

        public void EnsureAt(int index)
        {
            while (Count <= index)
            {
                Add(default(T));
            }
        }

        public override string ToString()
        {
            if (Count > 0)
                return string.Format("{0}[{1}] {{ {2} ... }}", typeof(T).Name, Count, this.First());
            return string.Format("{0}[{1}]", typeof(T).Name, Count);
        }

        //public void ModifyAt(int index, ModifyAtFunc<T> func)
        //{
        //	var v = this[index];
        //	func(ref v);
        //	this[index] = v;
        //}

        #endregion

        #region Implementation of IMeshStream

        public new IList<int> AddRange(IEnumerable<T> items)
        {
            var data = items.ToArray();
            var lookup = new int[data.Length];
            for (var index = 0; index < data.Length; index++)
            {
                lookup[index] = index + this.Count;
            }
            ((List<T>)this).AddRange(data);
            return lookup;
        }
        public new int Add(T item)
        {
            var v = Count;
            ((IList<T>)this).Add(item);
            return v;
        }

        public IList<TValue> GetReader<TValue>()
        {
            if (typeof(TValue) == typeof(T))
                return (StreamConverter<TValue>)(object)new StreamConverterListAdapter<T>(this);
            if (ConverterFactory != null)
            {
                var resolveConverter = ConverterFactory.ResolveConverter<T, TValue>(this);
                if (resolveConverter != null)
                    return resolveConverter;
            }
            throw new NotImplementedException(string.Format("{0} to {1} converter is not defined", typeof(T).FullName,
                typeof(TValue).FullName));
        }

        #endregion
    }

    public static class ListMeshStream
    {
        public static IMeshStream Create(Type t, IStreamConverterFactory converterFactory, IStreamMetaInfo metaInfo)
        {
            return
                (IMeshStream)
                Activator.CreateInstance(typeof(ListMeshStream<>).MakeGenericType(t), converterFactory, metaInfo);
        }
    }
}