using ControlLibrary.Controls.TextControl.TextEditor.Document;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.TextControl.TextEditor
{
	public delegate void MarginMouseEventHandler(AbstractMargin sender, Point mousepos, MouseButtons mouseButtons);
	public delegate void MarginPaintEventHandler(AbstractMargin sender, Graphics g, Rectangle rect);

	/// <summary>
	/// This class views the line numbers and folding markers.
	/// </summary>
	public abstract class AbstractMargin
	{
		private Cursor cursor = Cursors.Default;

		//[CLSCompliant(false)]
		protected Rectangle drawingPosition = new Rectangle(0, 0, 0, 0);
		//[CLSCompliant(false)]
		protected TextArea textArea;

		public Rectangle DrawingPosition
		{
			get => drawingPosition;
			set => drawingPosition = value;
		}

		public TextArea TextArea => textArea;

		public IDocument Document => textArea.Document;

		public ITextEditorProperties TextEditorProperties => textArea.Document.TextEditorProperties;

		public virtual Cursor Cursor
		{
			get => cursor;
			set => cursor = value;
		}

		public virtual Size Size => new Size(-1, -1);

		public virtual bool IsVisible => true;

		protected AbstractMargin(TextArea textArea)
		{
			this.textArea = textArea;
		}

		public virtual void HandleMouseDown(Point mousepos, MouseButtons mouseButtons)
		{
			MouseDown?.Invoke(this, mousepos, mouseButtons);
		}
		public virtual void HandleMouseMove(Point mousepos, MouseButtons mouseButtons)
		{
			MouseMove?.Invoke(this, mousepos, mouseButtons);
		}
		public virtual void HandleMouseLeave(EventArgs e)
		{
			MouseLeave?.Invoke(this, e);
		}

		public virtual void Paint(Graphics g, Rectangle rect)
		{
			Painted?.Invoke(this, g, rect);
		}

		public event MarginPaintEventHandler Painted;
		public event MarginMouseEventHandler MouseDown;
		public event MarginMouseEventHandler MouseMove;
		public event EventHandler MouseLeave;
	}
}
