using System.Drawing;

namespace ControlLibrary.Controls.TextControl.TextEditor
{
	public delegate void ToolTipRequestEventHandler(object sender, ToolTipRequestEventArgs e);

	public class ToolTipRequestEventArgs
	{
		private Point mousePosition;
		private TextLocation logicalPosition;
		private readonly bool inDocument;

		public Point MousePosition => mousePosition;

		public TextLocation LogicalPosition => logicalPosition;

		public bool InDocument => inDocument;

		/// <summary>
		/// Gets if some client handling the event has already shown a tool tip.
		/// </summary>
		public bool ToolTipShown => toolTipText != null;

		internal string toolTipText;

		public void ShowToolTip(string text)
		{
			toolTipText = text;
		}

		public ToolTipRequestEventArgs(Point mousePosition, TextLocation logicalPosition, bool inDocument)
		{
			this.mousePosition = mousePosition;
			this.logicalPosition = logicalPosition;
			this.inDocument = inDocument;
		}
	}
}
