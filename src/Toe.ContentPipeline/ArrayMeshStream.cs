using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline
{
    public abstract class ArrayMeshStream
    {
        public static IMeshStream Create<T>(T[] data, IStreamConverterFactory converterFactory,
            IStreamMetaInfo metaInfo)
        {
            return new ArrayMeshStream<T>(data, converterFactory, metaInfo);
        }
    }

    public class ArrayMeshStream<T> : ArrayMeshStream, IMeshStream<T>
    {
        public static readonly EqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;

        private readonly T[] _data;

        public ArrayMeshStream(T[] data, IStreamConverterFactory converterFactory = null,
            IStreamMetaInfo metaInfo = null)
        {
            ConverterFactory = converterFactory ?? StreamConverterFactory.Default;
            MetaInfo = metaInfo ?? ConverterFactory.GetMetaInfo(typeof(T));
            _data = data;
        }

        public ArrayMeshStream(int length, IStreamConverterFactory converterFactory = null,
            IStreamMetaInfo metaInfo = null)
        {
            ConverterFactory = converterFactory ?? StreamConverterFactory.Default;
            MetaInfo = metaInfo ?? ConverterFactory.GetMetaInfo(typeof(T));
            _data = new T[length];
        }

        public ArrayMeshStream(IEnumerable<T> data, IStreamConverterFactory converterFactory = null,
            IStreamMetaInfo metaInfo = null)
        {
            ConverterFactory = converterFactory ?? StreamConverterFactory.Default;
            MetaInfo = metaInfo ?? ConverterFactory.GetMetaInfo(typeof(T));
            _data = data.ToArray();
        }

        public IStreamConverterFactory ConverterFactory { get; set; } = StreamConverterFactory.Default;

        object IList.this[int index]
        {
            get => _data[index];
            set => _data[index] = (T) value;
        }

        void IList.RemoveAt(int index)
        {
            throw new InvalidOperationException("Can't modify array");
        }

        public bool IsFixedSize => true;

        public bool IsReadOnly => false;

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new InvalidOperationException("Can't modify array");
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }


        public IStreamMetaInfo MetaInfo { get; }

        /// <summary>
        ///     Compare to items in stream.
        /// </summary>
        /// <param name="index0">First item to compare.</param>
        /// <param name="index1">Second item to compare.</param>
        /// <returns>True if items are equal</returns>
        public bool AreEqual(int index0, int index1)
        {
            return EqualityComparer.Equals(_data[index0], _data[index1]);
        }

        /// <summary>
        ///     Get item hash code.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns>Item hash code.</returns>
        public int GetItemHashCode(int index)
        {
            return _data[index].GetHashCode();
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


        int IList<T>.IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        #region Public Methods and Operators

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>) _data).GetEnumerator();
        }

        public override string ToString()
        {
            if (_data.Length > 0)
                return string.Format("{0}[{1}] {{ {2} ... }}", typeof(T).Name, _data.Length, this.First());
            return string.Format("{0}[{1}]", typeof(T).Name, _data.Length);
        }

        //public void ModifyAt(int index, ModifyAtFunc<T> func)
        //{
        //    var v = this[index];
        //    func(ref v);
        //    this[index] = v;
        //}

        #endregion

        #region Implementation of IMeshStream

        public int Add(T item)
        {
            throw new InvalidOperationException("Can't add data to array");
        }

        IList<int> IMeshStream<T>.AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            throw new NotImplementedException();
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            _data.CopyTo(array, index);
        }


        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }


        public int Count => _data.Length;

        public bool IsSynchronized => _data.IsSynchronized;

        public object SyncRoot => _data.SyncRoot;

        public void AddDefault(int count = 1)
        {
            throw new InvalidOperationException("Can't add data to array");
        }

        public IReadOnlyList<TValue> GetReader<TValue>()
        {
            if (typeof(TValue) == typeof(T))
                return (StreamConverter<TValue>) (object) new StreamConverterListAdapter<T>(_data);
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
}