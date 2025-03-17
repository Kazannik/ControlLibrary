using System;

namespace ControlLibrary.Controls.TextControl.TextEditor
{
	/// <summary>
	/// A line/column position.
	/// Text editor lines/columns are counting from zero.
	/// </summary>
	public struct TextLocation : IComparable<TextLocation>, IEquatable<TextLocation>
	{
		/// <summary>
		/// Represents no text location (-1, -1).
		/// </summary>
		public static readonly TextLocation Empty = new TextLocation(-1, -1);

		public TextLocation(int column, int line)
		{
			x = column;
			y = line;
		}

		private int x, y;

		public int X
		{
			get => x;
			set => x = value;
		}

		public int Y
		{
			get => y;
			set => y = value;
		}

		public int Line
		{
			get => y;
			set => y = value;
		}

		public int Column
		{
			get => x;
			set => x = value;
		}

		public bool IsEmpty => x <= 0 && y <= 0;

		public override string ToString()
		{
			return string.Format("(Line {1}, Col {0})", x, y);
		}

		public override int GetHashCode()
		{
			return unchecked((87 * x.GetHashCode()) ^ y.GetHashCode());
		}

		public override bool Equals(object obj)
		{
			return obj is TextLocation location && location == this;
		}

		public bool Equals(TextLocation other)
		{
			return this == other;
		}

		public static bool operator ==(TextLocation a, TextLocation b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(TextLocation a, TextLocation b)
		{
			return a.x != b.x || a.y != b.y;
		}

		public static bool operator <(TextLocation a, TextLocation b)
		{
			return a.y < b.y || (a.y == b.y && a.x < b.x);
		}

		public static bool operator >(TextLocation a, TextLocation b)
		{
			return a.y > b.y || (a.y == b.y && a.x > b.x);
		}

		public static bool operator <=(TextLocation a, TextLocation b)
		{
			return !(a > b);
		}

		public static bool operator >=(TextLocation a, TextLocation b)
		{
			return !(a < b);
		}

		public int CompareTo(TextLocation other)
		{
			return this == other ? 0 : this < other ? -1 : 1;
		}
	}
}
