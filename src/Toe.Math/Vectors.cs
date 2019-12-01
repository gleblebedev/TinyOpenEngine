using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Toe
{
    public partial struct Vector2d: IEquatable<Vector2d>, IEnumerable<double>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public double X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public double Y;
        public Vector2d(Vector2 vec)
        {
            this.X = (double)vec.X;
            this.Y = (double)vec.Y;
        }

        public Vector2d(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<double> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2d))
                return false;
            return Equals((Vector2d)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2d left, Vector2d right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2d left, Vector2d right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector2d is equal to this Vector2d instance.
        /// </summary>
        /// <param name="other">The Vector2d to compare this instance to.</param>
        /// <returns>True if the other Vector2d is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2d other)
        {
            return 
                   X == other.X
                && Y == other.Y
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector3d: IEquatable<Vector3d>, IEnumerable<double>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public double X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public double Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public double Z;
        public Vector3d(Vector3 vec)
        {
            this.X = (double)vec.X;
            this.Y = (double)vec.Y;
            this.Z = (double)vec.Z;
        }

        public Vector3d(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<double> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3d))
                return false;
            return Equals((Vector3d)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3d left, Vector3d right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3d left, Vector3d right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector3d is equal to this Vector3d instance.
        /// </summary>
        /// <param name="other">The Vector3d to compare this instance to.</param>
        /// <returns>True if the other Vector3d is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3d other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector4d: IEquatable<Vector4d>, IEnumerable<double>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public double X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public double Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public double Z;
    
        /// <summary>
        /// The W component of the vector.
        /// </summary>
        public double W;
        public Vector4d(Vector4 vec)
        {
            this.X = (double)vec.X;
            this.Y = (double)vec.Y;
            this.Z = (double)vec.Z;
            this.W = (double)vec.W;
        }

        public Vector4d(double X, double Y, double Z, double W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<double> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
            yield return this.W;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector4d))
                return false;
            return Equals((Vector4d)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4d left, Vector4d right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                && left.W == right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4d left, Vector4d right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                && left.W != right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector4d is equal to this Vector4d instance.
        /// </summary>
        /// <param name="other">The Vector4d to compare this instance to.</param>
        /// <returns>True if the other Vector4d is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4d other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                && W == other.W
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector2i: IEquatable<Vector2i>, IEnumerable<int>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public int X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public int Y;
        public Vector2i(Vector2 vec)
        {
            this.X = (int)vec.X;
            this.Y = (int)vec.Y;
        }

        public Vector2i(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<int> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2i))
                return false;
            return Equals((Vector2i)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2i left, Vector2i right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2i left, Vector2i right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector2i is equal to this Vector2i instance.
        /// </summary>
        /// <param name="other">The Vector2i to compare this instance to.</param>
        /// <returns>True if the other Vector2i is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2i other)
        {
            return 
                   X == other.X
                && Y == other.Y
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector3i: IEquatable<Vector3i>, IEnumerable<int>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public int X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public int Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public int Z;
        public Vector3i(Vector3 vec)
        {
            this.X = (int)vec.X;
            this.Y = (int)vec.Y;
            this.Z = (int)vec.Z;
        }

        public Vector3i(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<int> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3i))
                return false;
            return Equals((Vector3i)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3i left, Vector3i right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3i left, Vector3i right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector3i is equal to this Vector3i instance.
        /// </summary>
        /// <param name="other">The Vector3i to compare this instance to.</param>
        /// <returns>True if the other Vector3i is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3i other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector4i: IEquatable<Vector4i>, IEnumerable<int>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public int X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public int Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public int Z;
    
        /// <summary>
        /// The W component of the vector.
        /// </summary>
        public int W;
        public Vector4i(Vector4 vec)
        {
            this.X = (int)vec.X;
            this.Y = (int)vec.Y;
            this.Z = (int)vec.Z;
            this.W = (int)vec.W;
        }

        public Vector4i(int X, int Y, int Z, int W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<int> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
            yield return this.W;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector4i))
                return false;
            return Equals((Vector4i)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4i left, Vector4i right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                && left.W == right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4i left, Vector4i right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                && left.W != right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector4i is equal to this Vector4i instance.
        /// </summary>
        /// <param name="other">The Vector4i to compare this instance to.</param>
        /// <returns>True if the other Vector4i is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4i other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                && W == other.W
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector4us: IEquatable<Vector4us>, IEnumerable<ushort>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public ushort X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public ushort Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public ushort Z;
    
        /// <summary>
        /// The W component of the vector.
        /// </summary>
        public ushort W;
        public Vector4us(Vector4 vec)
        {
            this.X = (ushort)vec.X;
            this.Y = (ushort)vec.Y;
            this.Z = (ushort)vec.Z;
            this.W = (ushort)vec.W;
        }

        public Vector4us(ushort X, ushort Y, ushort Z, ushort W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<ushort> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
            yield return this.W;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector4us))
                return false;
            return Equals((Vector4us)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4us left, Vector4us right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                && left.W == right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4us left, Vector4us right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                && left.W != right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector4us is equal to this Vector4us instance.
        /// </summary>
        /// <param name="other">The Vector4us to compare this instance to.</param>
        /// <returns>True if the other Vector4us is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4us other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                && W == other.W
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector4ub: IEquatable<Vector4ub>, IEnumerable<byte>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public byte X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public byte Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public byte Z;
    
        /// <summary>
        /// The W component of the vector.
        /// </summary>
        public byte W;
        public Vector4ub(Vector4 vec)
        {
            this.X = (byte)vec.X;
            this.Y = (byte)vec.Y;
            this.Z = (byte)vec.Z;
            this.W = (byte)vec.W;
        }

        public Vector4ub(byte X, byte Y, byte Z, byte W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
            yield return this.W;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector4ub))
                return false;
            return Equals((Vector4ub)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4ub left, Vector4ub right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                && left.W == right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4ub left, Vector4ub right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                && left.W != right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector4ub is equal to this Vector4ub instance.
        /// </summary>
        /// <param name="other">The Vector4ub to compare this instance to.</param>
        /// <returns>True if the other Vector4ub is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4ub other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                && W == other.W
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector2l: IEquatable<Vector2l>, IEnumerable<long>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public long X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public long Y;
        public Vector2l(Vector2 vec)
        {
            this.X = (long)vec.X;
            this.Y = (long)vec.Y;
        }

        public Vector2l(long X, long Y)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<long> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2l))
                return false;
            return Equals((Vector2l)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2l left, Vector2l right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2l left, Vector2l right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector2l is equal to this Vector2l instance.
        /// </summary>
        /// <param name="other">The Vector2l to compare this instance to.</param>
        /// <returns>True if the other Vector2l is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2l other)
        {
            return 
                   X == other.X
                && Y == other.Y
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector3l: IEquatable<Vector3l>, IEnumerable<long>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public long X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public long Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public long Z;
        public Vector3l(Vector3 vec)
        {
            this.X = (long)vec.X;
            this.Y = (long)vec.Y;
            this.Z = (long)vec.Z;
        }

        public Vector3l(long X, long Y, long Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<long> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3l))
                return false;
            return Equals((Vector3l)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3l left, Vector3l right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3l left, Vector3l right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector3l is equal to this Vector3l instance.
        /// </summary>
        /// <param name="other">The Vector3l to compare this instance to.</param>
        /// <returns>True if the other Vector3l is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3l other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector4l: IEquatable<Vector4l>, IEnumerable<long>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public long X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public long Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public long Z;
    
        /// <summary>
        /// The W component of the vector.
        /// </summary>
        public long W;
        public Vector4l(Vector4 vec)
        {
            this.X = (long)vec.X;
            this.Y = (long)vec.Y;
            this.Z = (long)vec.Z;
            this.W = (long)vec.W;
        }

        public Vector4l(long X, long Y, long Z, long W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<long> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
            yield return this.W;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector4l))
                return false;
            return Equals((Vector4l)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4l left, Vector4l right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                && left.W == right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4l left, Vector4l right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                && left.W != right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector4l is equal to this Vector4l instance.
        /// </summary>
        /// <param name="other">The Vector4l to compare this instance to.</param>
        /// <returns>True if the other Vector4l is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4l other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                && W == other.W
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector2b: IEquatable<Vector2b>, IEnumerable<bool>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public bool X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public bool Y;

        public Vector2b(bool X, bool Y)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<bool> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2b))
                return false;
            return Equals((Vector2b)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2b left, Vector2b right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2b left, Vector2b right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector2b is equal to this Vector2b instance.
        /// </summary>
        /// <param name="other">The Vector2b to compare this instance to.</param>
        /// <returns>True if the other Vector2b is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2b other)
        {
            return 
                   X == other.X
                && Y == other.Y
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector3b: IEquatable<Vector3b>, IEnumerable<bool>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public bool X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public bool Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public bool Z;

        public Vector3b(bool X, bool Y, bool Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<bool> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3b))
                return false;
            return Equals((Vector3b)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3b left, Vector3b right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3b left, Vector3b right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector3b is equal to this Vector3b instance.
        /// </summary>
        /// <param name="other">The Vector3b to compare this instance to.</param>
        /// <returns>True if the other Vector3b is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3b other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }

    public partial struct Vector4b: IEquatable<Vector4b>, IEnumerable<bool>, IEnumerable
    {
    
        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public bool X;
    
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public bool Y;
    
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public bool Z;
    
        /// <summary>
        /// The W component of the vector.
        /// </summary>
        public bool W;

        public Vector4b(bool X, bool Y, bool Z, bool W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<bool> GetEnumerator()
        {
            yield return this.X;
            yield return this.Y;
            yield return this.Z;
            yield return this.W;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Vector3 instance.
        /// </summary>
        /// <param name="obj">The Object to compare against.</param>
        /// <returns>True if the Object is equal to this Vector3; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Vector4b))
                return false;
            return Equals((Vector4b)obj);
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are equal; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4b left, Vector4b right)
        {
            return 
                   left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z
                && left.W == right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the two given vectors are not equal.
        /// </summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns>True if the vectors are not equal; False if they are equal.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4b left, Vector4b right)
        {
            return 
                   left.X != right.X
                && left.Y != right.Y
                && left.Z != right.Z
                && left.W != right.W
                ;
        }

        /// <summary>
        /// Returns a boolean indicating whether the given Vector4b is equal to this Vector4b instance.
        /// </summary>
        /// <param name="other">The Vector4b to compare this instance to.</param>
        /// <returns>True if the other Vector4b is equal to this instance; False otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4b other)
        {
            return 
                   X == other.X
                && Y == other.Y
                && Z == other.Z
                && W == other.W
                ;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }
    }

}