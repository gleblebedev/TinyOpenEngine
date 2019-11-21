using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Toe.ContentPipeline
{
    public class DictionaryMeshStream<Key, Value> : IMeshStream<Value>, IDictionaryMeshStream
    {
        private readonly Func<Value, Key> _getKey;
        private readonly IEqualityComparer<Key> comparer;
        private readonly Dictionary<Key, int> data;

        private readonly List<Value> dataList;

        public DictionaryMeshStream(Func<Value, Key> getKey, IStreamConverterFactory converterFactory)
            : this(getKey, new Value[0], converterFactory, EqualityComparer<Key>.Default)
        {
        }

        public DictionaryMeshStream(Func<Value, Key> getKey, IStreamConverterFactory converterFactory, IStreamMetaInfo metaInfo)
            : this(getKey, new Value[0], converterFactory, EqualityComparer<Key>.Default, metaInfo)
        {
        }

        public DictionaryMeshStream(Func<Value, Key> getKey, IEnumerable<Value> data, IStreamConverterFactory converterFactory)
            : this(getKey, data, converterFactory, EqualityComparer<Key>.Default)
        {
        }

        public DictionaryMeshStream(Func<Value, Key> getKey, IEnumerable<Value> data, IStreamConverterFactory converterFactory,
            IEqualityComparer<Key> comparer)
            : this(getKey, data, converterFactory, comparer, converterFactory.GetMetaInfo(typeof(Value)))
        {
        }

        public DictionaryMeshStream(Func<Value, Key> getKey, IEnumerable<Value> data, IStreamConverterFactory converterFactory,
            IEqualityComparer<Key> comparer, IStreamMetaInfo metaInfo)
        {
            _getKey = getKey;
            this.data = new Dictionary<Key, int>(comparer);
            dataList = new List<Value>();
            foreach (var item in data)
            {
                Add(item);
            }
            ConverterFactory = converterFactory ?? StreamConverterFactory.Default;
            this.comparer = comparer;
            MetaInfo = metaInfo ?? ConverterFactory.GetMetaInfo(typeof(Value));
        }

        int IDictionaryMeshStream.Add(object item)
        {
            return Add((Value)item);
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return dataList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<Value>.Add(Value item)
        {
            Add(item);
        }

        public IList<int> AddRange(IEnumerable<Value> items)
        {
            var lookup = new List<int>();
            foreach (var item in items)
            {
                lookup.Add(Add(item));
            }
            return lookup;
        }

        public int Add(Value item)
        {
            Key key = _getKey(item);
            int v;
            if (data.TryGetValue(key, out v))
            {
                return v;
            }
            v = dataList.Count;
            dataList.Add(item);
            data.Add(key, v);
            return v;
        }

        int IList.Add(object value)
        {
            return Add((Value)value);
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Compare to items in stream.
        /// </summary>
        /// <param name="index0">First item to compare.</param>
        /// <param name="index1">Second item to compare.</param>
        /// <returns>True if items are equal</returns>
        public bool AreEqual(int index0, int index1)
        {
            return dataList[index0].Equals(dataList[index1]);
        }

        /// <summary>
        ///     Get item hash code.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns>Item hash code.</returns>
        public int GetItemHashCode(int index)
        {
            return dataList[index].GetHashCode();
        }

        /// <summary>
        ///     Create a list mesh stream of the same type.
        /// </summary>
        /// <returns>New empty list mesh stream.</returns>
        public IMeshStream CreateListMeshStreamOfTheSameType()
        {
            return new ListMeshStream<Value>(ConverterFactory, MetaInfo);
        }

        /// <summary>
        ///     Create a dictionary mesh stream of the same type.
        /// </summary>
        /// <returns>New empty list mesh stream.</returns>
        public IMeshStream CreateDictionaryMeshStreamOfTheSameType()
        {
            return new DictionaryMeshStream<Key, Value>(_getKey, Enumerable.Empty<Value>(), ConverterFactory, comparer, MetaInfo);
        }

        public IMeshStream Clone()
        {
            var dictionaryMeshStream = new DictionaryMeshStream<Key, Value>(_getKey, Enumerable.Empty<Value>(), ConverterFactory, comparer, MetaInfo);
            foreach (var i in data)
            {
                dictionaryMeshStream.data[i.Key] = i.Value;
            }
            dictionaryMeshStream.dataList.AddRange(this.dataList);
            return dictionaryMeshStream;
        }

        public void Clear()
        {
            data.Clear();
            dataList.Clear();
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((Value)value);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Value item)
        {
            return data.ContainsKey(_getKey(item));
        }

        public void CopyTo(Value[] array, int arrayIndex)
        {
            dataList.CopyTo(array, arrayIndex);
        }

        public bool Remove(Value item)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IList)dataList).CopyTo(array, index);
        }

        public int Count
        {
            get { return dataList.Count; }
        }

        object ICollection.SyncRoot
        {
            get { return ((IList)dataList).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((IList)dataList).IsSynchronized; }
        }

        public bool IsReadOnly
        {
            get { return ((IList<Value>)dataList).IsReadOnly; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        public int IndexOf(Value item)
        {
            int v;
            if (data.TryGetValue(_getKey(item), out v))
            {
                return v;
            }
            return -1;
        }

        public void Insert(int index, Value item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        object IList.this[int index]
        {
            get { return dataList[index]; }
            set { throw new NotImplementedException("Not supported. Please use Add method."); }
        }

        public Value this[int index]
        {
            get { return dataList[index]; }
            set { throw new NotImplementedException("Not supported. Please use Add method."); }
        }

        public IList<TValue> GetReader<TValue>()
        {
            if (typeof(TValue) == typeof(Value))
                return (StreamConverter<TValue>)(object)new StreamConverterListAdapter<Value>(dataList);
            if (ConverterFactory != null)
            {
                var resolveConverter = ConverterFactory.ResolveConverter<Value, TValue>(this);
                if (resolveConverter != null)
                    return resolveConverter;
            }
            throw new NotImplementedException(string.Format("{0} to {1} converter is not defined", typeof(Value).FullName,
                typeof(TValue).FullName));
        }

        public IStreamMetaInfo MetaInfo { get; }

        public IStreamConverterFactory ConverterFactory { get; set; }
    }

    public class DictionaryMeshStream<T> : IMeshStream<T>, IDictionaryMeshStream
    {
        private readonly IEqualityComparer<T> comparer;
        private readonly Dictionary<T, int> data;

        private readonly List<T> dataList;

        public DictionaryMeshStream(IStreamConverterFactory converterFactory)
            : this(new T[0], converterFactory, EqualityComparer<T>.Default)
        {
        }

        public DictionaryMeshStream(IStreamConverterFactory converterFactory, IStreamMetaInfo metaInfo)
            : this(new T[0], converterFactory, EqualityComparer<T>.Default, metaInfo)
        {
        }

        public DictionaryMeshStream(IEnumerable<T> data, IStreamConverterFactory converterFactory)
            : this(data, converterFactory, EqualityComparer<T>.Default)
        {
        }

        public DictionaryMeshStream(IEnumerable<T> data, IStreamConverterFactory converterFactory,
            IEqualityComparer<T> comparer)
            : this(data, converterFactory, comparer, converterFactory.GetMetaInfo(typeof(T)))
        {
        }

        public DictionaryMeshStream(IEnumerable<T> data, IStreamConverterFactory converterFactory,
            IEqualityComparer<T> comparer, IStreamMetaInfo metaInfo)
        {
            this.data = new Dictionary<T, int>(comparer);
            dataList = new List<T>();
            foreach (var item in data)
            {
                Add(item);
            }
            ConverterFactory = converterFactory ?? StreamConverterFactory.Default;
            this.comparer = comparer;
            MetaInfo = metaInfo ?? ConverterFactory.GetMetaInfo(typeof(T));
        }

        int IDictionaryMeshStream.Add(object item)
        {
            return Add((T)item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return dataList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public IList<int> AddRange(IEnumerable<T> items)
        {
            var lookup = new List<int>();
            foreach (var item in items)
            {
                lookup.Add(Add(item));
            }
            return lookup;
        }

        public int Add(T item)
        {
            int v;
            if (data.TryGetValue(item, out v))
            {
                return v;
            }
            v = dataList.Count;
            dataList.Add(item);
            data.Add(item, v);
            return v;
        }

        int IList.Add(object value)
        {
            return Add((T)value);
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Compare to items in stream.
        /// </summary>
        /// <param name="index0">First item to compare.</param>
        /// <param name="index1">Second item to compare.</param>
        /// <returns>True if items are equal</returns>
        public bool AreEqual(int index0, int index1)
        {
            return dataList[index0].Equals(dataList[index1]);
        }

        /// <summary>
        ///     Get item hash code.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns>Item hash code.</returns>
        public int GetItemHashCode(int index)
        {
            return dataList[index].GetHashCode();
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
        /// <returns>New empty list mesh stream.</returns>
        public IMeshStream CreateDictionaryMeshStreamOfTheSameType()
        {
            return new DictionaryMeshStream<T>(Enumerable.Empty<T>(), ConverterFactory, comparer, MetaInfo);
        }

        public IMeshStream Clone()
        {
            var dictionaryMeshStream = new DictionaryMeshStream<T>(Enumerable.Empty<T>(), ConverterFactory, comparer, MetaInfo);
            foreach (var i in data)
            {
                dictionaryMeshStream.data[i.Key] = i.Value;
            }
            dictionaryMeshStream.dataList.AddRange(this.dataList);
            return dictionaryMeshStream;
        }

        public void Clear()
        {
            data.Clear();
            dataList.Clear();
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            return data.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            dataList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IList)dataList).CopyTo(array, index);
        }

        public int Count
        {
            get { return dataList.Count; }
        }

        object ICollection.SyncRoot
        {
            get { return ((IList)dataList).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((IList)dataList).IsSynchronized; }
        }

        public bool IsReadOnly
        {
            get { return ((IList<T>)dataList).IsReadOnly; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            int v;
            if (data.TryGetValue(item, out v))
            {
                return v;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        object IList.this[int index]
        {
            get { return dataList[index]; }
            set { throw new NotImplementedException("Not supported. Please use Add method."); }
        }

        public T this[int index]
        {
            get { return dataList[index]; }
            set { throw new NotImplementedException("Not supported. Please use Add method."); }
        }

        public IList<TValue> GetReader<TValue>()
        {
            if (typeof(TValue) == typeof(T))
                return (StreamConverter<TValue>)(object)new StreamConverterListAdapter<T>(dataList);
            if (ConverterFactory != null)
            {
                var resolveConverter = ConverterFactory.ResolveConverter<T, TValue>(this);
                if (resolveConverter != null)
                    return resolveConverter;
            }
            throw new NotImplementedException(string.Format("{0} to {1} converter is not defined", typeof(T).FullName,
                typeof(TValue).FullName));
        }

        public IStreamMetaInfo MetaInfo { get; }

        public IStreamConverterFactory ConverterFactory { get; set; }
    }
}