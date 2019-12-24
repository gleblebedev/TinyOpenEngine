using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Toe.ContentPipeline
{
    public class AssetContainer<T> : IAssetContainer<T> where T : IAsset
    {
        private readonly List<T> _list;
        private readonly Dictionary<string, T> _map;

        public AssetContainer() : this(0)
        {
        }

        public AssetContainer(int capacity)
        {
            _list = new List<T>(capacity);
            _map = new Dictionary<string, T>(capacity);
        }

        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list.Capacity;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _list.Capacity = value;
        }

        public bool IsReadOnly => false;

        public void Add(T asset)
        {
            if (asset.Id == null) asset.Id = $"{typeof(T).Name}{_list.Count}";
            _map.Add(asset.Id, asset);
            _list.Add(asset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) _list).GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _list).GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return _map.ContainsKey(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            return _map.TryGetValue(key, out value);
        }

        public T this[string key]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _map[key];
        }

        public IEnumerable<string> Keys => _map.Keys;

        public IEnumerable<T> Values => _map.Values;


        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list.Count;
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list[index];
        }
    }
}