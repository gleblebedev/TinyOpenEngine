using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Toe.ContentPipeline
{
    internal struct IndexSet : IEquatable<IndexSet>
    {
        public bool Equals(IndexSet other)
        {
            if (_count != other._count)
                return false;
            for (var i = 0; i < _count; i++)
                if (this[i] != other[i])
                    return false;

            return true;
        }

        public int this[int streamIndex]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _indices[Offset + streamIndex];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _indices[Offset + streamIndex] = value;
        }

        public override bool Equals(object obj)
        {
            return obj is IndexSet other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 0;
                for (var i = 0; i < _count; i++) hashCode = (hashCode * 397) ^ this[i];
                return hashCode;
            }
        }

        public static bool operator ==(IndexSet left, IndexSet right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IndexSet left, IndexSet right)
        {
            return !left.Equals(right);
        }

        private readonly List<int> _indices;
        private readonly int _count;

        public IndexSet(List<int> indices, int start, int count)
        {
            _indices = indices;
            Offset = start;
            _count = count;
        }

        public int Offset { get; }
    }
}