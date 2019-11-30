using System;
using System.Globalization;
using System.Text;

namespace Toe
{
	public partial struct Vector2d: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector2d instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector2d instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector3d: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector3d instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Z).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector3d instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector4d: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector4d instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Z).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.W).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector4d instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector2i: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector2i instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector2i instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector3i: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector3i instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Z).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector3i instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector4i: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector4i instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Z).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.W).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector4i instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector4us: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector4us instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Z).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.W).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector4us instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector4ub: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector4ub instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Z).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.W).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector4ub instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector2l: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector2l instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector2l instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector3l: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector3l instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Z).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector3l instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector4l: IFormattable
	{
        /// <summary>
        /// Returns a String representing this Vector4l instance, using the specified format to format individual elements 
        /// and the given IFormatProvider.
        /// </summary>
        /// <param name="format">The format of individual elements.</param>
        /// <param name="formatProvider">The format provider to use when formatting elements.</param>
        /// <returns>The string representation.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(((IFormattable)this.X).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Y).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.Z).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(((IFormattable)this.W).ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }

		/// <summary>
        /// Returns a String representing this Vector4l instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
	}

	public partial struct Vector2b
	{
		/// <summary>
        /// Returns a String representing this Vector2b instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string separator = ",";
            sb.Append('<');
            sb.Append(this.X);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.Y);
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }
	}

	public partial struct Vector3b
	{
		/// <summary>
        /// Returns a String representing this Vector3b instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string separator = ",";
            sb.Append('<');
            sb.Append(this.X);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.Y);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.Z);
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }
	}

	public partial struct Vector4b
	{
		/// <summary>
        /// Returns a String representing this Vector4b instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string separator = ",";
            sb.Append('<');
            sb.Append(this.X);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.Y);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.Z);
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(this.W);
            sb.Append(separator);
            sb.Append('>');
            return sb.ToString();
        }
	}

}