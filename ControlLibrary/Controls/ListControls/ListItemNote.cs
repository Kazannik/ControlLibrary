using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.ListControls
{
	public abstract class ListItemNote: IListItemNote
	{
		private Size _size;
		
		public event EventHandler<EventArgs> ClipSizeChanged;
		public event EventHandler<EventArgs> ContentChanged;
				
		public Size Size => _size; 

		protected ListItemNote()
		{
			_size = Size.Empty;
		}

		public void Draw(DrawItemEventArgs e) => OnDraw(e: e);
				
		public Size MeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight)
		{
			_size = OnMeasureBound(graphics: graphics, font: font, itemWidth: itemWidth, itemHeight: itemHeight);
			return Size;
		}
		
		protected abstract void OnDraw(DrawItemEventArgs e);

		protected abstract Size OnMeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight);

		private void OnClipSizeChanged(EventArgs e) =>
			ClipSizeChanged?.Invoke(this, e);
		
		protected void DoClipSizeChanged() =>
			OnClipSizeChanged(new EventArgs());
		
		private void OnContentChanged(EventArgs e) =>
			ContentChanged?.Invoke(this, e);

		protected void DoContentChanged() =>
			OnContentChanged(new EventArgs());

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
