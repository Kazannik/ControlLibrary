using System;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	public class LineCountChangeEventArgs : EventArgs
	{
		private IDocument document;
		private int start;
		private int moved;

		/// <returns>
		/// always a valid Document which is related to the Event.
		/// </returns>
		public IDocument Document => document;

		/// <returns>
		/// -1 if no offset was specified for this event
		/// </returns>
		public int LineStart => start;

		/// <returns>
		/// -1 if no length was specified for this event
		/// </returns>
		public int LinesMoved => moved;

		public LineCountChangeEventArgs(IDocument document, int lineStart, int linesMoved)
		{
			this.document = document;
			this.start = lineStart;
			this.moved = linesMoved;
		}
	}

	public class LineEventArgs : EventArgs
	{
		private IDocument document;
		private LineSegment lineSegment;

		public IDocument Document => document;

		public LineSegment LineSegment => lineSegment;

		public LineEventArgs(IDocument document, LineSegment lineSegment)
		{
			this.document = document;
			this.lineSegment = lineSegment;
		}

		public override string ToString()
		{
			return string.Format("[LineEventArgs Document={0} LineSegment={1}]", this.document, this.lineSegment);
		}
	}

	public class LineLengthChangeEventArgs : LineEventArgs
	{
		private int lengthDelta;

		public int LengthDelta => lengthDelta;

		public LineLengthChangeEventArgs(IDocument document, LineSegment lineSegment, int moved)
			: base(document, lineSegment)
		{
			this.lengthDelta = moved;
		}

		public override string ToString()
		{
			return string.Format("[LineLengthEventArgs Document={0} LineSegment={1} LengthDelta={2}]", this.Document, this.LineSegment, this.lengthDelta);
		}
	}
}
