using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.ListControls
{
	public interface IListItem : IEnumerable<IListItemNote> 
	{
		event EventHandler<EventArgs> ClipSizeChanged;
		event EventHandler<EventArgs> ContentChanged;

		Size MeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight);
		void Draw(DrawItemEventArgs e);
		Rectangle Bounds { get; }
		Size Size { get; }
		IListItemNote this[int index] { get; }
		int Count { get; }
		Rectangle GetSubitemRectangle(int index);		
	}	
}