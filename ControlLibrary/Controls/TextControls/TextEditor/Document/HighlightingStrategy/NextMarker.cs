using System;
using System.Xml;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	/// <summary>
	/// Used for mark next token
	/// </summary>
	public class NextMarker
	{
		private readonly string what;
		private readonly HighlightColor color;
		private readonly bool markMarker = false;

		/// <value>
		/// String value to indicate to mark next token
		/// </value>
		public string What => what;

		/// <value>
		/// Color for marking next token
		/// </value>
		public HighlightColor Color => color;

		/// <value>
		/// If true the indication text will be marked with the same color
		/// too
		/// </value>
		public bool MarkMarker => markMarker;

		/// <summary>
		/// Creates a new instance of <see cref="NextMarker"/>
		/// </summary>
		public NextMarker(XmlElement mark)
		{
			color = new HighlightColor(mark);
			what = mark.InnerText;
			if (mark.Attributes["markmarker"] != null)
			{
				markMarker = Boolean.Parse(mark.Attributes["markmarker"].InnerText);
			}
		}
	}

}
