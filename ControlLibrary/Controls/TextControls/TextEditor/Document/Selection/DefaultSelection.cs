using System;
using System.Diagnostics;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	/// <summary>
	/// Default implementation of the <see cref="TextEditor.Document.ISelection"/> interface.
	/// </summary>
	public class DefaultSelection : ISelection
	{
		private readonly IDocument document;
		private bool isRectangularSelection;
		private TextLocation startPosition;
		private TextLocation endPosition;

		public TextLocation StartPosition
		{
			get => startPosition;
			set
			{
				DefaultDocument.ValidatePosition(document, value);
				startPosition = value;
			}
		}

		public TextLocation EndPosition
		{
			get => endPosition;
			set
			{
				DefaultDocument.ValidatePosition(document, value);
				endPosition = value;
			}
		}

		public int Offset => document.PositionToOffset(startPosition);

		public int EndOffset => document.PositionToOffset(endPosition);

		public int Length => EndOffset - Offset;

		/// <value>
		/// Returns true, if the selection is empty
		/// </value>
		public bool IsEmpty => startPosition == endPosition;

		/// <value>
		/// Returns true, if the selection is rectangular
		/// </value>
		// TODO : make this unused property used.
		public bool IsRectangularSelection
		{
			get => isRectangularSelection;
			set => isRectangularSelection = value;
		}

		/// <value>
		/// The text which is selected by this selection.
		/// </value>
		public string SelectedText => document != null ? Length < 0 ? null : document.GetText(Offset, Length) : null;

		/// <summary>
		/// Creates a new instance of <see cref="DefaultSelection"/>
		/// </summary>
		public DefaultSelection(IDocument document, TextLocation startPosition, TextLocation endPosition)
		{
			DefaultDocument.ValidatePosition(document, startPosition);
			DefaultDocument.ValidatePosition(document, endPosition);
			Debug.Assert(startPosition <= endPosition);
			this.document = document;
			this.startPosition = startPosition;
			this.endPosition = endPosition;
		}

		/// <summary>
		/// Converts a <see cref="DefaultSelection"/> instance to string (for debug purposes)
		/// </summary>
		public override string ToString()
		{
			return string.Format("[DefaultSelection : StartPosition={0}, EndPosition={1}]", startPosition, endPosition);
		}
		public bool ContainsPosition(TextLocation position)
		{
			return !IsEmpty && ((startPosition.Y < position.Y && position.Y < endPosition.Y) ||
				(startPosition.Y == position.Y && startPosition.X <= position.X && (startPosition.Y != endPosition.Y || position.X <= endPosition.X)) ||
				(endPosition.Y == position.Y && startPosition.Y != endPosition.Y && position.X <= endPosition.X));
		}

		public bool ContainsOffset(int offset)
		{
			return Offset <= offset && offset <= EndOffset;
		}
	}
}
