using System;
using System.Runtime.CompilerServices;

namespace Toe.EntityComponentSystem
{
    internal class ArrayHelper<T>
    {
        public T[] Items;
        public int Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayHelper(int capacity)
        {
            Items = new T[capacity];
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (Items.Length == Count)
            {
                Array.Resize(ref Items, Items.Length << 1);
            }
            Items[Count++] = item;
        }
    }
}