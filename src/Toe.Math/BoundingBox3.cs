using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace Toe
{
    public struct BoundingBox3
    {
        #region Constants and Fields

        public static readonly BoundingBox3 Empty = new BoundingBox3(AllMaxValues, AllMinValues);

        private static readonly Vector3 AllMaxValues = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        private static readonly Vector3 AllMinValues = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        private Vector3 _max;

        private Vector3 _min;

        #endregion

        #region Constructors and Destructors

        public BoundingBox3(Vector3 min, Vector3 max)
        {
            _min = min;
            _max = max;
        }

        public BoundingBox3(IEnumerable<Vector3> items)
        {
            _min = AllMaxValues;
            _max = AllMinValues;
            foreach (var Vector3 in items)
            {
                if (Vector3.X > _max.X) _max.X = Vector3.X;
                if (Vector3.X < _min.X) _min.X = Vector3.X;
                if (Vector3.Y > _max.Y) _max.Y = Vector3.Y;
                if (Vector3.Y < _min.Y) _min.Y = Vector3.Y;
                if (Vector3.Z > _max.Z) _max.Z = Vector3.Z;
                if (Vector3.Z < _min.Z) _min.Z = Vector3.Z;
            }
        }

        #endregion

        #region Public Properties

        public Vector3 Center => (_min + _max) * 0.5f;

        public bool IsEmpty => _min.X > _max.X || _min.Y > _max.Y || _min.Z > _max.Z;

        public Vector3 Max => _max;

        public Vector3 Min => _min;

        public Vector3 Size
        {
            get
            {
                if (IsEmpty) return Vector3.Zero;
                return _max - _min;
            }
        }

        public float Volume
        {
            get
            {
                var size = Size;
                return size.X * size.Y * size.Z;
            }
        }

        #endregion

        #region Public Methods and Operators

        public BoundingBox3 Union(BoundingBox3 right)
        {
            if (IsEmpty)
                return right;
            if (right.IsEmpty)
                return this;
            return new BoundingBox3
            {
                _min = new Vector3(Math.Min(Min.X, right.Min.X), Math.Min(Min.Y, right.Min.Y),
                    Math.Min(Min.Z, right.Min.Z)),
                _max = new Vector3(Math.Max(Max.X, right.Max.X), Math.Max(Max.Y, right.Max.Y),
                    Math.Max(Max.Z, right.Max.Z))
            };
        }

        public void Union(ref BoundingBox3 right, out BoundingBox3 res)
        {
            if (IsEmpty)
            {
                res = right;
                return;
            }

            if (right.IsEmpty)
            {
                res = this;
                return;
            }

            res = new BoundingBox3
            {
                _min =
                    new Vector3(Math.Min(Min.X, right.Min.X), Math.Min(Min.Y, right.Min.Y),
                        Math.Min(Min.Z, right.Min.Z)),
                _max =
                    new Vector3(Math.Max(Max.X, right.Max.X), Math.Max(Max.Y, right.Max.Y),
                        Math.Max(Max.Z, right.Max.Z))
            };
        }

        public BoundingBox3 Union(Vector3 right)
        {
            if (IsEmpty)
                return new BoundingBox3 {_min = right, _max = right};
            return new BoundingBox3
            {
                _min = new Vector3(Math.Min(Min.X, right.X), Math.Min(Min.Y, right.Y),
                    Math.Min(Min.Z, right.Z)),
                _max = new Vector3(Math.Max(Max.X, right.X), Math.Max(Max.Y, right.Y),
                    Math.Max(Max.Z, right.Z))
            };
        }

        public static BoundingBox3 operator +(BoundingBox3 left, BoundingBox3 right)
        {
            return
                new BoundingBox3(
                    new Vector3(
                        Math.Min(left._min.X, right._min.X),
                        Math.Min(left._min.Y, right._min.Y),
                        Math.Min(left._min.Z, right._min.Z)),
                    new Vector3(
                        Math.Max(left._min.X, right._min.X),
                        Math.Max(left._min.Y, right._min.Y),
                        Math.Max(left._min.Z, right._min.Z)));
        }

        public static BoundingBox3 operator +(BoundingBox3 left, Vector3 right)
        {
            if (left.IsEmpty) return left;
            return new BoundingBox3(
                new Vector3(left._min.X + right.X, left._min.Y + right.Y, left._min.Z + right.Z),
                new Vector3(left._max.X + right.X, left._max.Y + right.Y, left._max.Z + right.Z));
        }

        public static bool operator ==(BoundingBox3 left, BoundingBox3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BoundingBox3 left, BoundingBox3 right)
        {
            return !left.Equals(right);
        }

        public bool Equals(BoundingBox3 other)
        {
            return _min.Equals(other._min) && _max.Equals(other._max);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BoundingBox3 && Equals((BoundingBox3) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_min.GetHashCode() * 397) ^ _max.GetHashCode();
            }
        }

        public bool Contains(Vector3 v)
        {
            return v.X >= _min.X && v.X <= _max.X
                                 && v.Y >= _min.Y && v.Y <= _max.Y
                                 && v.Z >= _min.Z && v.Z <= _max.Z;
        }

        public bool Contains(ref BoundingBox3 box)
        {
            return box._min.X >= _min.X && box._max.X <= _max.X
                                        && box._min.Y >= _min.Y && box._max.Y <= _max.Y
                                        && box._min.Z >= _min.Z && box._max.Z <= _max.Z;
        }

        public bool Contains(ref Vector3 v)
        {
            return v.X >= _min.X && v.X <= _max.X
                                 && v.Y >= _min.Y && v.Y <= _max.Y
                                 && v.Z >= _min.Z && v.Z <= _max.Z;
        }

        public bool Contains(ref Vector3 v, float eps)
        {
            return v.X + eps >= _min.X && v.X - eps <= _max.X
                                       && v.Y + eps >= _min.Y && v.Y - eps <= _max.Y
                                       && v.Z + eps >= _min.Z && v.Z - eps <= _max.Z;
        }

        #endregion

        public BoundingBox3 ExpandBy(float step)
        {
            return ExpandBy(new Vector3(step, step, step));
        }

        public BoundingBox3 ExpandBy(Vector3 step)
        {
            return new BoundingBox3(_min - step, _max + step);
        }

        public static BoundingBox3 FromCenterAndSize(Vector3 center, Vector3 size)
        {
            var halfSize = size * 0.5f;
            return new BoundingBox3(center - halfSize, center + halfSize);
        }

        public static BoundingBox3 FromCenterAndSize(Vector3 center, float size)
        {
            var halfSize = new Vector3(size, size, size) * 0.5f;
            return new BoundingBox3(center - halfSize, center + halfSize);
        }

        /// <summary>
        ///     Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scale.</param>
        /// <returns>The result of the calculation.</returns>
        public static BoundingBox3 operator *(BoundingBox3 vec, float scale)
        {
            return new BoundingBox3(vec.Min * scale, vec.Max * scale);
        }

        /// <summary>
        ///     Multiplies an instance by a scale.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scale vector.</param>
        /// <returns>The result of the calculation.</returns>
        public static BoundingBox3 operator *(BoundingBox3 vec, Vector3 scale)
        {
            return new BoundingBox3(vec.Min * scale, vec.Max * scale);
        }

        #region Overrides

        #region public override string ToString()

        private static readonly string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        /// <summary>
        ///     Returns a System.String that represents the current Vector2.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{{{0} .. {1}}}", _min, _max);
        }

        #endregion

        #endregion
    }
}