namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	public class ColumnRange
	{
		public static readonly ColumnRange NoColumn = new ColumnRange(-2, -2);
		public static readonly ColumnRange WholeColumn = new ColumnRange(-1, -1);
		private int startColumn;
		private int endColumn;

		public int StartColumn
		{
			get => startColumn;
			set => startColumn = value;
		}

		public int EndColumn
		{
			get => endColumn;
			set => endColumn = value;
		}

		public ColumnRange(int startColumn, int endColumn)
		{
			this.startColumn = startColumn;
			this.endColumn = endColumn;

		}

		public override int GetHashCode()
		{
			return startColumn + (endColumn << 16);
		}

		public override bool Equals(object obj)
		{
			return obj is ColumnRange range && range.startColumn == startColumn &&
					   range.endColumn == endColumn;
		}

		public override string ToString()
		{
			return string.Format("[ColumnRange: StartColumn={0}, EndColumn={1}]", startColumn, endColumn);
		}
	}
}
