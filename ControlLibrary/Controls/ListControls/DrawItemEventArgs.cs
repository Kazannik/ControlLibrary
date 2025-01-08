using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.ListControls
{
	public class DrawItemEventArgs : System.Windows.Forms.DrawItemEventArgs
	{
		public DrawItemEventArgs(Graphics graphics, Font font, Rectangle rect, int index, DrawItemState state) 
			: base(graphics: graphics, font: font, rect: rect, index: index, state: state)
		{
			RatingStarColor = ForeColor;
		}

		public DrawItemEventArgs(Graphics graphics, Font font, Rectangle rect, int index, DrawItemState state, Color foreColor, Color backColor)
			: base(graphics: graphics, font: font, rect: rect, index: index, state: state, foreColor: foreColor, backColor: backColor)
		{
			RatingStarColor = foreColor;
		}

		public DrawItemEventArgs(Graphics graphics, Font font, Rectangle rect, int index, DrawItemState state, Color foreColor, Color backColor, Color ratingStarColor) 
			: this (graphics: graphics, font: font, rect: rect, index: index, state: state, foreColor: foreColor, backColor: backColor)
		{
			RatingStarColor = ratingStarColor;
		}

		public DrawItemEventArgs(System.Windows.Forms.DrawItemEventArgs arg) 
			: this(graphics: arg.Graphics, font: arg.Font, rect: arg.Bounds, index: arg.Index, state: arg.State, foreColor: arg.ForeColor, backColor: arg.BackColor)
		{
			RatingStarColor = arg.ForeColor;
		}

		public DrawItemEventArgs(System.Windows.Forms.DrawItemEventArgs arg, Color ratingStarColor) 
			: this(arg: arg)
		{
			RatingStarColor = ratingStarColor;
		}

		public Color RatingStarColor { get; }
	}
}
