using System;
using System.Globalization;
using SharpGLTF.Schema2;
using Toe.ContentPipeline;

namespace Toe.ContentPipeline.GLTFSharp
{
    public class BufferStreamKey : IEquatable<BufferStreamKey>, IComparable<BufferStreamKey>
    {
        public BufferStreamKey(string key, Accessor accessor)
        {
            AcessorKey = key;
            Key = GetStreamKey(key);
            Dimensions = accessor.Dimensions;
            Encoding = accessor.Encoding;
        }

        public string AcessorKey { get; }
        public StreamKey Key { get; }

        public EncodingType Encoding { get; set; }

        public DimensionType Dimensions { get; set; }

        public int CompareTo(BufferStreamKey other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var keyComparison = Key.CompareTo(other.Key);
            if (keyComparison != 0) return keyComparison;
            var encodingComparison = Encoding.CompareTo(other.Encoding);
            if (encodingComparison != 0) return encodingComparison;
            return Dimensions.CompareTo(other.Dimensions);
        }

        public bool Equals(BufferStreamKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Key.Equals(other.Key) && Encoding == other.Encoding && Dimensions == other.Dimensions;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BufferStreamKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Key.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Encoding;
                hashCode = (hashCode * 397) ^ (int) Dimensions;
                return hashCode;
            }
        }

        public static bool operator ==(BufferStreamKey left, BufferStreamKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BufferStreamKey left, BufferStreamKey right)
        {
            return !Equals(left, right);
        }

        public static StreamKey GetStreamKey(string key)
        {
            if (key.Length > 2 && key[key.Length - 2] == '_' && char.IsDigit(key[key.Length - 1]))
                return new StreamKey(key.Substring(0, key.Length - 2),
                    int.Parse(key.Substring(key.Length - 1), CultureInfo.InvariantCulture));
            return new StreamKey(key, 0);
        }

        public override string ToString()
        {
            return $"{Key} {Dimensions} {Encoding}";
        }
    }
}