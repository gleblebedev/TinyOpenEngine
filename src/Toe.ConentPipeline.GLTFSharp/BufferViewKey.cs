using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharpGLTF.Schema2;

namespace Toe.ConentPipeline.GLTFSharp
{
    public class BufferViewKey : IEquatable<BufferViewKey>, IEnumerable<BufferStreamKey>
    {
        private readonly BufferStreamKey[] _streams;

        public BufferViewKey(IReadOnlyDictionary<string, Accessor> accessors)
        {
            _streams = accessors.Select(_ => new BufferStreamKey(_.Key, _.Value)).OrderBy(_ => _).ToArray();
        }

        public IEnumerator<BufferStreamKey> GetEnumerator()
        {
            return ((IList<BufferStreamKey>) _streams).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(BufferViewKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (_streams.Length != other._streams.Length) return false;
            for (var index = 0; index < _streams.Length; index++)
                if (_streams[index] != other._streams[index])
                    return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BufferViewKey) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            for (var index = 0; index < _streams.Length; index++)
                hashCode = hashCode * 397 + _streams[index].GetHashCode();

            return hashCode;
        }

        public static bool operator ==(BufferViewKey left, BufferViewKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BufferViewKey left, BufferViewKey right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Join(", ", _streams.Select(_ => _.ToString()));
        }
    }
}