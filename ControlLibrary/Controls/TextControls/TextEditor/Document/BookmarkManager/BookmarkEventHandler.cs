using System;

namespace ControlLibrary.Controls.TextControl.TextEditor.Document
{
	public delegate void BookmarkEventHandler(object sender, BookmarkEventArgs e);

	/// <summary>
	/// Description of BookmarkEventHandler.
	/// </summary>
	public class BookmarkEventArgs : EventArgs
	{
		private readonly Bookmark bookmark;

		public Bookmark Bookmark
		{
			get
			{
				return bookmark;
			}
		}

		public BookmarkEventArgs(Bookmark bookmark)
		{
			this.bookmark = bookmark;
		}
	}
}
