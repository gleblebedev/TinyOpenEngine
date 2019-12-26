using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    internal class IndexSetReader
    {
        private readonly List<int> _buffer;
        private readonly IReadOnlyList<int>[] _readers;

        public IndexSetReader(List<int> buffer, IReadOnlyCollection<StreamKey> keys, IndexMeshPrimitive primitive)
        {
            _buffer = buffer;
            _readers = new IReadOnlyList<int>[keys.Count];
            var i = 0;
            foreach (var streamKey in keys)
            {
                _readers[i] = primitive.GetIndexReader(streamKey);
                ++i;
            }
        }

        public int Position { get; set; }

        public IndexSet Read(int index)
        {
            var res = new IndexSet(_buffer, Position, _readers.Length);

            foreach (var readOnlyList in _readers)
            {
                if (_buffer.Count == Position)
                    _buffer.Add(readOnlyList[index]);
                else
                    _buffer[Position] = readOnlyList[index];
                ++Position;
            }

            return res;
        }
    }
}