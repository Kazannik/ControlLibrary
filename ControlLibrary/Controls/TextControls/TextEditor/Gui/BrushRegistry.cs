using System.Collections.Generic;
using System.Drawing;

namespace ControlLibrary.Controls.TextControl.TextEditor
{
	/// <summary>
	/// Contains brushes/pens for the text editor to speed up drawing. Re-Creation of brushes and pens
	/// seems too costly.
	/// </summary>
	public class BrushRegistry
	{
		private static readonly Dictionary<Color, Brush> brushes = new Dictionary<Color, Brush>();
		private static readonly Dictionary<Color, Pen> pens = new Dictionary<Color, Pen>();
		private static readonly Dictionary<Color, Pen> dotPens = new Dictionary<Color, Pen>();

		public static Brush GetBrush(Color color)
		{
			lock (brushes)
			{
				if (!brushes.TryGetValue(color, out Brush brush))
				{
					brush = new SolidBrush(color);
					brushes.Add(color, brush);
				}
				return brush;
			}
		}

		public static Pen GetPen(Color color)
		{
			lock (pens)
			{
				if (!pens.TryGetValue(color, out Pen pen))
				{
					pen = new Pen(color);
					pens.Add(color, pen);
				}
				return pen;
			}
		}

		private static readonly float[] dotPattern = { 1, 1, 1, 1 };

		public static Pen GetDotPen(Color color)
		{
			lock (dotPens)
			{
				if (!dotPens.TryGetValue(color, out Pen pen))
				{
					pen = new Pen(color)
					{
						DashPattern = dotPattern
					};
					dotPens.Add(color, pen);
				}
				return pen;
			}
		}
	}
}
