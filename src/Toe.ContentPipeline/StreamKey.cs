using System;

namespace Toe.ContentPipeline
{
    public struct StreamKey : IEquatable<StreamKey>, IComparable<StreamKey>
    {
        #region Constants and Fields

        public static readonly StreamKey Binormal = new StreamKey(Streams.Binormal, 0);

        public static readonly StreamKey Color = new StreamKey(Streams.Color, 0);

        public static readonly StreamKey Normal = new StreamKey(Streams.Normal, 0);

        public static readonly StreamKey Position = new StreamKey(Streams.Position, 0);

        public static readonly StreamKey Tangent = new StreamKey(Streams.Tangent, 0);

        public static readonly StreamKey TexCoord0 = new StreamKey(Streams.TexCoord, 0);

        public static readonly StreamKey TexCoord1 = new StreamKey(Streams.TexCoord, 1);

        public static readonly StreamKey Weight = new StreamKey(Streams.Weight, 0);

        public static readonly StreamKey Joint = new StreamKey(Streams.Joint, 0);

        public static readonly StreamKey Index = new StreamKey(Streams.Index, 0);

        #endregion

        #region Constructors and Destructors

        public StreamKey(string key, int channel)
            : this()
        {
            Key = key;
            Channel = channel;
        }

        public StreamKey(string key)
            : this()
        {
            Key = key;
            Channel = 0;
        }

        #endregion

        #region Public Properties

        public int Channel { get; }

        public string Key { get; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(StreamKey left, StreamKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StreamKey left, StreamKey right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(StreamKey other)
        {
            return string.Equals(Key, other.Key) && Channel == other.Channel;
        }

        /// <summary>
        ///     Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is StreamKey && Equals((StreamKey) obj);
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Key != null ? Key.GetHashCode() : 0) * 397) ^ Channel;
            }
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            return string.Format("{0}{1}", Key, Channel);
        }

        public int CompareTo(StreamKey other)
        {
            var keyComparison = string.Compare(Key, other.Key, StringComparison.Ordinal);
            if (keyComparison != 0) return keyComparison;
            return Channel.CompareTo(other.Channel);
        }

        #endregion
    }
}