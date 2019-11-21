using System;

namespace Toe.ContentPipeline
{
    public class StreamMetaInfo : IStreamMetaInfo, IEquatable<StreamMetaInfo>
    {
        public StreamMetaInfo(Type valueType, Type baseType, int componentsPerSet, int numberOfSets)
        {
            if (valueType == null)
                throw new ArgumentNullException(nameof(valueType));
            if (baseType == null)
                throw new ArgumentNullException(nameof(baseType));
            ValueType = valueType;
            BaseType = baseType;
            ComponentsPerSet = componentsPerSet;
            NumberOfSets = numberOfSets;
        }

        bool IEquatable<StreamMetaInfo>.Equals(StreamMetaInfo other)
        {
            return Equals((IStreamMetaInfo)other);
        }
        public bool Equals(IStreamMetaInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ValueType.Equals(other.ValueType) && ComponentsPerSet == other.ComponentsPerSet &&
                   NumberOfSets == other.NumberOfSets && BaseType.Equals(other.BaseType);
        }

        public Type ValueType { get; }

        public int ComponentsPerSet { get; }

        public int NumberOfSets { get; }

        public Type BaseType { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as IStreamMetaInfo;
            return other != null && this.Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ValueType.GetHashCode();
                hashCode = (hashCode * 397) ^ ComponentsPerSet;
                hashCode = (hashCode * 397) ^ NumberOfSets;
                hashCode = (hashCode * 397) ^ BaseType.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(StreamMetaInfo left, StreamMetaInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StreamMetaInfo left, StreamMetaInfo right)
        {
            return !Equals(left, right);
        }
    }
}