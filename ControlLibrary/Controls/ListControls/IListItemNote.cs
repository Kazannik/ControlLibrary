using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.ListControls
{
	public interface IListItemNote
	{
		event EventHandler<EventArgs> ContentChanged;
		Size MeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight);
		void Draw(DrawItemEventArgs e);
		Rectangle Bounds { get; }
		Size Size { get; }
	}
}
