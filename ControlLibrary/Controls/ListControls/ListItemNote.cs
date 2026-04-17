using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.ListControls
{
	public abstract class ListItemNote : IListItemNote
	{
		public event EventHandler<EventArgs> ContentChanged;

		public Size Size { get; protected set; }

		public Rectangle Bounds { get; protected set; }

		protected ListItemNote()
		{
			Bounds = Rectangle.Empty;
			Size = Size.Empty;
		}

		public void Draw(DrawItemEventArgs e)
		{
			Bounds = e.Bounds;
			OnDraw(e: e);
		}

		public Size MeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight)
		{
			return Size = OnMeasureBound(graphics: graphics, font: font, itemWidth: itemWidth, itemHeight: itemHeight);
		}

		protected abstract void OnDraw(DrawItemEventArgs e);

		protected abstract Size OnMeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight);

		private void OnContentChanged(EventArgs e) => ContentChanged?.Invoke(this, e);

		protected void DoContentChanged() => OnContentChanged(new EventArgs());

		protected Size GetTextSize(Graphics graphics, string text, Font font, int width, StringFormat stringFormat)
		{
			if (!string.IsNullOrEmpty(text))
			{
				SizeF measure = graphics.MeasureString(text, font, width, stringFormat);
				return new Size(width, (int)measure.Height);
			}
			else
			{
				return Size.Empty;
			}
		}
	}
}
