using System;
using System.Drawing;

namespace ControlLibrary.Controls.ListControls
{
	public interface IListItem
	{
		Size MeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight);
		void Draw(DrawItemEventArgs e);
		Size Size { get; }
		Rectangle ApplyButton { get; }
		Rectangle CancelButton { get; }
		bool IsAppled { get; }

		event EventHandler<EventArgs> ContentChanged;
	}
}