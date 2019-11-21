using System.Collections;
using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    public class ListProxy<T1, T2> : IList<T1> where T2 : T1
    {
        private readonly List<T2> _list;

        public ListProxy(List<T2> list)
        {
            _list = list;
        }

        public IEnumerator<T1> GetEnumerator()
        {
            foreach (var item in _list) yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _list).GetEnumerator();
        }

        public void Add(T1 item)
        {
            _list.Add((T2) item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T1 item)
        {
            return _list.Contains((T2) item);
        }

        public void CopyTo(T1[] array, int arrayIndex)
        {
            foreach (var item in _list)
            {
                array[arrayIndex] = item;
                ++arrayIndex;
            }
        }

        public bool Remove(T1 item)
        {
            return _list.Remove((T2) item);
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public int IndexOf(T1 item)
        {
            return _list.IndexOf((T2) item);
        }

        public void Insert(int index, T1 item)
        {
            _list.Insert(index, (T2) item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public T1 this[int index]
        {
            get => _list[index];
            set => _list[index] = (T2) value;
        }
    }
}