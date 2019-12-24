using System;

namespace Toe.ContentPipeline
{
    public struct SortableStreamKey : IEquatable<SortableStreamKey>, IComparable<SortableStreamKey>
    {
        public int CompareTo(SortableStreamKey other)
        {
            var priorityComparison = _priority.CompareTo(other._priority);
            if (priorityComparison != 0) return priorityComparison;
            return Key.CompareTo(other.Key);
        }

        public bool Equals(SortableStreamKey other)
        {
            return Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            return obj is SortableStreamKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public static bool operator ==(SortableStreamKey left, SortableStreamKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SortableStreamKey left, SortableStreamKey right)
        {
            return !left.Equals(right);
        }

        private readonly int _priority;

        public SortableStreamKey(StreamKey key)
        {
            Key = key;
            switch (key.Key)
            {
                case Streams.Position:
                    _priority = 0;
                    break;
                case Streams.Normal:
                    _priority = 1;
                    break;
                case Streams.Tangent:
                    _priority = 2;
                    break;
                case Streams.Binormal:
                    _priority = 3;
                    break;
                case Streams.TexCoord:
                    _priority = 4;
                    break;
                case Streams.Color:
                    _priority = 5;
                    break;
                case Streams.Joints:
                    _priority = 6;
                    break;
                case Streams.Weights:
                    _priority = 7;
                    break;
                case Streams.Index:
                    _priority = 8;
                    break;
                default:
                    _priority = 9;
                    break;
            }
        }

        public StreamKey Key { get; }
    }
}