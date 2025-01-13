using System;
using System.Xml;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	/// <summary>
	/// Used for mark previous token
	/// </summary>
	public class PrevMarker
	{
		private string what;
		private HighlightColor color;
		private bool markMarker = false;

		/// <value>
		/// String value to indicate to mark previous token
		/// </value>
		public string What => what;

		/// <value>
		/// Color for marking previous token
		/// </value>
		public HighlightColor Color => color;

		/// <value>
		/// If true the indication text will be marked with the same color
		/// too
		/// </value>
		public bool MarkMarker => markMarker;

		/// <summary>
		/// Creates a new instance of <see cref="PrevMarker"/>
		/// </summary>
		public PrevMarker(XmlElement mark)
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
