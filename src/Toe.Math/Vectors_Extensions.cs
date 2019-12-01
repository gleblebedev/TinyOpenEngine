using System.IO;
using System.Numerics;

namespace Toe
{
    public static partial class MathExtensionMethods
	{
        public static void Write(this BinaryWriter writer, in Vector2 vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
		}

        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            var X = reader.ReadSingle();
            var Y = reader.ReadSingle();
            return new Vector2(X, Y);
        }
        public static void Write(this BinaryWriter writer, in Vector3 vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
		}

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            var X = reader.ReadSingle();
            var Y = reader.ReadSingle();
            var Z = reader.ReadSingle();
            return new Vector3(X, Y, Z);
        }
        public static void Write(this BinaryWriter writer, in Vector4 vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
		}

        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            var X = reader.ReadSingle();
            var Y = reader.ReadSingle();
            var Z = reader.ReadSingle();
            var W = reader.ReadSingle();
            return new Vector4(X, Y, Z, W);
        }
        public static void Write(this BinaryWriter writer, in Vector2d vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
		}

        public static Vector2d ReadVector2d(this BinaryReader reader)
        {
            var X = reader.ReadDouble();
            var Y = reader.ReadDouble();
            return new Vector2d(X, Y);
        }
        public static void Write(this BinaryWriter writer, in Vector3d vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
		}

        public static Vector3d ReadVector3d(this BinaryReader reader)
        {
            var X = reader.ReadDouble();
            var Y = reader.ReadDouble();
            var Z = reader.ReadDouble();
            return new Vector3d(X, Y, Z);
        }
        public static void Write(this BinaryWriter writer, in Vector4d vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
		}

        public static Vector4d ReadVector4d(this BinaryReader reader)
        {
            var X = reader.ReadDouble();
            var Y = reader.ReadDouble();
            var Z = reader.ReadDouble();
            var W = reader.ReadDouble();
            return new Vector4d(X, Y, Z, W);
        }
        public static void Write(this BinaryWriter writer, in Vector2i vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
		}

        public static Vector2i ReadVector2i(this BinaryReader reader)
        {
            var X = reader.ReadInt32();
            var Y = reader.ReadInt32();
            return new Vector2i(X, Y);
        }
        public static void Write(this BinaryWriter writer, in Vector3i vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
		}

        public static Vector3i ReadVector3i(this BinaryReader reader)
        {
            var X = reader.ReadInt32();
            var Y = reader.ReadInt32();
            var Z = reader.ReadInt32();
            return new Vector3i(X, Y, Z);
        }
        public static void Write(this BinaryWriter writer, in Vector4i vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
		}

        public static Vector4i ReadVector4i(this BinaryReader reader)
        {
            var X = reader.ReadInt32();
            var Y = reader.ReadInt32();
            var Z = reader.ReadInt32();
            var W = reader.ReadInt32();
            return new Vector4i(X, Y, Z, W);
        }
        public static void Write(this BinaryWriter writer, in Vector4us vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
		}

        public static Vector4us ReadVector4us(this BinaryReader reader)
        {
            var X = reader.ReadUInt16();
            var Y = reader.ReadUInt16();
            var Z = reader.ReadUInt16();
            var W = reader.ReadUInt16();
            return new Vector4us(X, Y, Z, W);
        }
        public static void Write(this BinaryWriter writer, in Vector4ub vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
		}

        public static Vector4ub ReadVector4ub(this BinaryReader reader)
        {
            var X = reader.ReadByte();
            var Y = reader.ReadByte();
            var Z = reader.ReadByte();
            var W = reader.ReadByte();
            return new Vector4ub(X, Y, Z, W);
        }
        public static void Write(this BinaryWriter writer, in Vector2l vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
		}

        public static Vector2l ReadVector2l(this BinaryReader reader)
        {
            var X = reader.ReadInt64();
            var Y = reader.ReadInt64();
            return new Vector2l(X, Y);
        }
        public static void Write(this BinaryWriter writer, in Vector3l vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
		}

        public static Vector3l ReadVector3l(this BinaryReader reader)
        {
            var X = reader.ReadInt64();
            var Y = reader.ReadInt64();
            var Z = reader.ReadInt64();
            return new Vector3l(X, Y, Z);
        }
        public static void Write(this BinaryWriter writer, in Vector4l vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
		}

        public static Vector4l ReadVector4l(this BinaryReader reader)
        {
            var X = reader.ReadInt64();
            var Y = reader.ReadInt64();
            var Z = reader.ReadInt64();
            var W = reader.ReadInt64();
            return new Vector4l(X, Y, Z, W);
        }
        public static void Write(this BinaryWriter writer, in Vector2b vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
		}

        public static Vector2b ReadVector2b(this BinaryReader reader)
        {
            var X = reader.ReadBoolean();
            var Y = reader.ReadBoolean();
            return new Vector2b(X, Y);
        }
        public static void Write(this BinaryWriter writer, in Vector3b vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
		}

        public static Vector3b ReadVector3b(this BinaryReader reader)
        {
            var X = reader.ReadBoolean();
            var Y = reader.ReadBoolean();
            var Z = reader.ReadBoolean();
            return new Vector3b(X, Y, Z);
        }
        public static void Write(this BinaryWriter writer, in Vector4b vec)
		{
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
		}

        public static Vector4b ReadVector4b(this BinaryReader reader)
        {
            var X = reader.ReadBoolean();
            var Y = reader.ReadBoolean();
            var Z = reader.ReadBoolean();
            var W = reader.ReadBoolean();
            return new Vector4b(X, Y, Z, W);
        }
	}
}