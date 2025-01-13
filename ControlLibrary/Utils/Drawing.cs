using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static ControlLibrary.Utils.Gdi32;

namespace ControlLibrary.Utils
{
	public static class Drawing
	{
		private const TextFormatFlags CENTER_FORMAT_FLAGS = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;

		public static void DrawTextIcon(Graphics graphics, string text, Font font, Color foreColor, Color backColor, Rectangle rect)
		{
			Brush BackColorBrush = new SolidBrush(foreColor);
			Pen BackColorPen = new Pen(foreColor, 1);
			FillRoundedRectangle(graphics, BackColorBrush, rect, (rect.Height / 3) == 0 ? 1 : (rect.Height / 3));
			TextRenderer.DrawText(graphics, text, font, rect, backColor, foreColor, CENTER_FORMAT_FLAGS);
			DrawRoundedRectangle(graphics, BackColorPen, rect, (rect.Height / 3) == 0 ? 1 : (rect.Height / 3));
			BackColorBrush.Dispose();
			BackColorPen.Dispose();
		}

		public static void DrawRoundedRectangle(Graphics graphics, Pen pen, Rectangle rect, int radius)
		{
			GraphicsPath path = new GraphicsPath();

			path.AddArc(new Rectangle(rect.X, rect.Y, radius, radius), 180, 90);
			path.AddArc(new Rectangle(rect.X + rect.Width - radius, rect.Y, radius, radius), 270, 90);
			path.AddArc(new Rectangle(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius), 0, 90);
			path.AddArc(new Rectangle(rect.X, rect.Y + rect.Height - radius, radius, radius), 90, 90);
			path.CloseFigure();

			graphics.DrawPath(pen, path);
		}

		public static void FillRoundedRectangle(Graphics graphics, Brush brush, Rectangle rect, int radius)
		{
			GraphicsPath path = new GraphicsPath();

			path.AddArc(new Rectangle(rect.X, rect.Y, radius, radius), 180, 90);
			path.AddArc(new Rectangle(rect.X + rect.Width - radius, rect.Y, radius, radius), 270, 90);
			path.AddArc(new Rectangle(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius), 0, 90);
			path.AddArc(new Rectangle(rect.X, rect.Y + rect.Height - radius, radius, radius), 90, 90);
			path.CloseFigure();

			graphics.FillPath(brush, path);
		}

		public static GraphicsPath CreateStarPath(Rectangle rect)
		{
			return CreateStarPath(location: rect.Location, size: rect.Size);
		}

		public static GraphicsPath CreateStarPath(SizeF size)
		{
			return CreateStarPath(location: PointF.Empty, size: size);
		}

		public static GraphicsPath CreateStarPath(PointF location, SizeF size)
		{
			double r = (double)size.Height / (double)3.5, R = (double)size.Height / (double)2;
			double x0 = (double)size.Width / (double)2, y0 = (double)size.Height / (double)2;
			PointF[] points = new PointF[10];
			for (int vertex = 0; vertex < 10; vertex++)
			{
				double l = vertex % 2 == 0 ? R : r;
				points[vertex] = GetVertexPoint(location.X + x0, location.Y + y0, l, 10, vertex);
			}
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);
			path.CloseFigure();
			return path;
		}

		private static Point GetVertexPoint(double x, double y, double radius, double count, double part)
		{
			return new Point(
				(int)(x + (radius * Math.Sin(2 * Math.PI / count * part))),
				(int)(y - (radius * Math.Cos(2 * Math.PI / count * part))));
		}

		public static void DrawStar(Graphics graphics, Color foreColor, Rectangle rect)
		{
			DrawStar(graphics: graphics, foreColor: foreColor, backColor: foreColor, percent: 0, rect: rect);
		}

		public static void DrawStar(Graphics graphics, Color foreColor, Color backColor, Rectangle rect)
		{
			DrawStar(graphics: graphics, foreColor: foreColor, backColor: backColor, percent: 100, rect: rect);
		}

		public static void DrawStar(Graphics graphics, Color foreColor, Color backColor, int percent, Rectangle rect)
		{
			GraphicsPath star = CreateStarPath(rect);
			RectangleF starRectagle = star.GetBounds();
			Region starRegion = new Region(star);
			int percentWidth = (int)((double)starRectagle.Width / (double)100 * (double)Math.Abs(percent));

			if (Math.Abs(percent) >= 100) { }
			else if (percent < 0)
			{
				Region backRegion = new Region(new Rectangle((int)starRectagle.Left, (int)starRectagle.Top, (int)starRectagle.Width - percentWidth + 1, (int)starRectagle.Height));
				starRegion.Exclude(backRegion);
				backRegion.Dispose();
			}
			else if (percent > 0)
			{
				Region backRegion = new Region(new Rectangle((int)starRectagle.Left + percentWidth + 1, (int)starRectagle.Top, (int)starRectagle.Width - percentWidth, (int)starRectagle.Height));
				starRegion.Exclude(backRegion);
				backRegion.Dispose();
			}
			if (percent != 0)
			{
				graphics.FillRegion(new SolidBrush(backColor), starRegion);
				starRegion.Dispose();
			}
			graphics.DrawPath(new Pen(foreColor), star);
			star.Dispose();
		}

		public static void DrawOkIcon(Graphics graphics, Color foreColor, Color backColor, Rectangle rect)
		{
			double r = (double)rect.Height / (double)4, R = (double)rect.Height / (double)2.5;
			double x0 = (double)rect.Width / (double)2, y0 = (double)rect.Height / (double)2;
			Point[] points = new Point[3];
			points[0] = GetVertexPoint(rect.X + x0, rect.Y + y0, R, 8, 1);
			points[1] = GetVertexPoint(rect.X + x0, rect.Y + y0, R, 8, 4);
			points[2] = GetVertexPoint(rect.X + x0, rect.Y + y0, r, 8, 6);
			GraphicsPath path = new GraphicsPath();
			path.AddLines(points);
			Pen pen = new Pen(foreColor, 2);

			graphics.DrawPath(pen, path);
			path.Dispose();
			graphics.DrawEllipse(pen, rect);
			pen.Dispose();
		}

		public static void DrawCancelIcon(Graphics graphics, Color foreColor, Color backColor, Rectangle rect)
		{
			double R = (double)rect.Height / (double)4;
			double x0 = (double)rect.Width / (double)2, y0 = (double)rect.Height / (double)2;
			Point[] points = new Point[4];
			points[0] = GetVertexPoint(rect.X + x0 + 1, rect.Y + y0, R, 8, 1);
			points[1] = GetVertexPoint(rect.X + x0 + 1, rect.Y + y0 + 1, R, 8, 3);
			points[2] = GetVertexPoint(rect.X + x0, rect.Y + y0 + 1, R, 8, 5);
			points[3] = GetVertexPoint(rect.X + x0, rect.Y + y0, R, 8, 7);

			Pen pen = new Pen(foreColor, 2);
			graphics.DrawLine(pen, points[0], points[2]);
			graphics.DrawLine(pen, points[3], points[1]);
			graphics.DrawEllipse(pen, rect);
			pen.Dispose();
		}

		public static Bitmap GetImageMso(stdole.IPictureDisp img)
		{
			// stdole.IPictureDisp img;
			// Microsoft.Office.Interop.Excel.Application application = Globals.ThisAddIn.Application;
			// img = application.CommandBars.GetImageMso("SelectAll", 16, 16);
			return ConvertPixelByPixel(img);
		}
	}
}
