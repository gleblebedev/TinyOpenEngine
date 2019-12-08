using System.Buffers;

namespace Toe.ContentPipeline.Tokenizer
{
    public class ArrayPoolAllocationStrategy: IAllocationStrategy
    {
        private readonly int _alignment;
        ArrayPool<char> _arrayPool;

        public ArrayPoolAllocationStrategy(int alignment = 1024) : this(ArrayPool<char>.Shared, alignment)
        {

        }
        public ArrayPoolAllocationStrategy(ArrayPool<char> pool, int alignment = 1024)
        {
            _alignment = alignment;
            _arrayPool = pool;
        }
        public char[] Rent(int minSize)
        {
            var count = (minSize + _alignment - 1) / _alignment;
            return _arrayPool.Rent(count * _alignment);
        }

        public void Return(char[] buffer)
        {
            _arrayPool.Return(buffer);
        }
    }
}