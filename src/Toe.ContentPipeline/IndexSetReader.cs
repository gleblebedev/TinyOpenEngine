using System.Collections.Generic;

namespace Toe.ContentPipeline
{
    internal class IndexSetReader 
    {
        private readonly List<int> _buffer;
        private IReadOnlyList<int>[] _readers;
        private int _position = 0;

        public IndexSetReader(List<int> buffer, IReadOnlyCollection<StreamKey> keys, IndexMeshPrimitive primitive)
        {
            _buffer = buffer;
            _readers = new IReadOnlyList<int>[keys.Count];
            int i = 0;
            foreach (var streamKey in keys)
            {
                _readers[i] = primitive.GetIndexReader(streamKey);
                ++i;
            }
        }

        public int Position
        {
            get { return _position;}
            set { _position = value; }
        }

        public IndexSet Read(int index)
        {
            var res = new IndexSet(_buffer, _position, _readers.Length);

            foreach (var readOnlyList in _readers)
            {
                if (_buffer.Count == _position)
                    _buffer.Add(readOnlyList[index]);
                else
                    _buffer[_position] = readOnlyList[index];
                ++_position;
            }

            return res;
        }
    }
}