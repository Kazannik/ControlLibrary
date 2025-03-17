using System.Drawing;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	public enum TextMarkerType
	{
		Invisible,
		SolidBlock,
		Underlined,
		WaveLine
	}

	/// <summary>
	/// Marks a part of a document.
	/// </summary>
	public class TextMarker : AbstractSegment
	{
		private readonly TextMarkerType textMarkerType;
		private readonly Color color;
		private readonly Color foreColor;
		private string toolTip = null;
		private readonly bool overrideForeColor = false;

		public TextMarkerType TextMarkerType => textMarkerType;

		public Color Color => color;

		public Color ForeColor => foreColor;

		public bool OverrideForeColor => overrideForeColor;

		/// <summary>
		/// Marks the text segment as read-only.
		/// </summary>
		public bool IsReadOnly { get; set; }

		public string ToolTip
		{
			get => toolTip;
			set => toolTip = value;
		}

		/// <summary>
		/// Gets the last offset that is inside the marker region.
		/// </summary>
		public int EndOffset => Offset + Length - 1;

		public TextMarker(int offset, int length, TextMarkerType textMarkerType) : this(offset, length, textMarkerType, Color.Red)
		{
		}

		public TextMarker(int offset, int length, TextMarkerType textMarkerType, Color color)
		{
			if (length < 1) length = 1;
			this.offset = offset;
			this.length = length;
			this.textMarkerType = textMarkerType;
			this.color = color;
		}

		public TextMarker(int offset, int length, TextMarkerType textMarkerType, Color color, Color foreColor)
		{
			if (length < 1) length = 1;
			this.offset = offset;
			this.length = length;
			this.textMarkerType = textMarkerType;
			this.color = color;
			this.foreColor = foreColor;
			this.overrideForeColor = true;
		}
	}
}
