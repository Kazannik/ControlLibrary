// Ignore Spelling: Utils Mso img

using ControlLibrary.Structures;
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
		private static readonly StringFormat CENTER_STRING_FORMAT = new StringFormat
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center
		};

		public static void SetGraphicsStyle(Graphics graphics)
		{
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
			graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
		}


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

		private static GraphicsPath CreateStarPath(Rectangle rect)
		{
			return CreateStarPath(location: rect.Location, size: rect.Size);
		}

		private static GraphicsPath CreateStarPath(SizeF size)
		{
			return CreateStarPath(location: PointF.Empty, size: size);
		}

		private static GraphicsPath CreateStarPath(PointF location, SizeF size)
		{
			double r = (double)size.Height / 3.95, R = (double)size.Height / 2;
			double x0 = (double)size.Width / 2, y0 = (double)size.Height / 2;
			PointF[] points = new PointF[10];
			for (int vertex = 0; vertex < 10; vertex++)
			{
				double l = vertex % 2 == 0 ? R : r;
				points[vertex] = GetVertexPoint(location.X + x0 + 1, location.Y + y0 + 1, l, 10, vertex);
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

		public static void DrawStar(Graphics graphics, Color borderColor, Rectangle rect)
		{
			DrawStar(graphics: graphics, borderColor: borderColor, backBrush: new SolidBrush(borderColor), percent: 0, rect: rect);
		}

		public static void DrawStar(Graphics graphics, Color borderColor, Brush backBrush, Rectangle rect)
		{
			DrawStar(graphics: graphics, borderColor: borderColor, backBrush: backBrush, percent: 100, rect: rect);
		}

		public static void DrawStar(Graphics graphics, Color borderColor, Brush backBrush, int percent, Rectangle rect)
		{
			GraphicsPath star = CreateStarPath(rect);
			RectangleF starRectangle = star.GetBounds();
			Region starRegion = new Region(star);
			int percentWidth = (int)((double)starRectangle.Width / 100 * Math.Abs(percent));

			if (Math.Abs(percent) >= 100) { }

			else if (percent < 0)
			{
				Region backRegion = new Region(new Rectangle((int)starRectangle.Left, (int)starRectangle.Top, (int)starRectangle.Width - percentWidth + 1, (int)starRectangle.Height));
				starRegion.Exclude(backRegion);
				backRegion.Dispose();
			}
			else if (percent > 0)
			{
				Region backRegion = new Region(new Rectangle((int)starRectangle.Left + percentWidth + 1, (int)starRectangle.Top, (int)starRectangle.Width - percentWidth, (int)starRectangle.Height));
				starRegion.Exclude(backRegion);
				backRegion.Dispose();
			}

			if (percent != 0)
			{
				graphics.FillRegion(backBrush, starRegion);
				starRegion.Dispose();
			}
			graphics.DrawPath(new Pen(borderColor), star);
			star.Dispose();
		}

		public static Rectangle[] DrawRating(Graphics graphics, Font font, Color borderColor, Brush backBrush, Color textColor, Rectangle rect, int starCount = 5)
		{
			return DrawRating(graphics: graphics, font: font, borderColor: borderColor, backBrush: backBrush, textColor: textColor, rect: rect, rating: starCount, starCount: starCount);
		}

		public static Rectangle[] DrawRating(Graphics graphics, Font font, Color borderColor, Brush backBrush, Color textColor, Rectangle rect, Rating rating, int starCount = 5)
		{
			return DrawRating(graphics: graphics, font: font, borderColor: borderColor, backBrush: backBrush, textColor: textColor, rect: rect, rating: rating.Value, starCount: starCount);
		}

		public static Rectangle[] DrawRating(Graphics graphics, Font font, Color borderColor, Brush backBrush, Color textColor, Rectangle rect, int rating, int starCount = 5)
		{
			Rectangle[] rectagles = new Rectangle[starCount + 1];
			for (int i = 0; i < starCount; i++)
			{
				int percent = Rating.GetPercent(value: rating, index: i, count: starCount);
				rectagles[i] = new Rectangle(rect.X + ((rect.Height + 1) * i), rect.Y, rect.Height, rect.Height);
				DrawStar(graphics: graphics, borderColor: borderColor, backBrush: backBrush, percent: percent, rect: rectagles[i]);
			}
			int left = rectagles[starCount - 1].X + rectagles[starCount - 1].Width;
			rectagles[starCount] = new Rectangle(left, rect.Y, rect.Width - rect.Height * starCount, rect.Height);
			graphics.DrawString(s: rating.ToString(), font: font, brush: new SolidBrush(textColor), layoutRectangle: rectagles[starCount], format: CENTER_STRING_FORMAT);
			return rectagles;
		}

		public static Size MeasureRating(Graphics graphics, Font font, int starCount = 5)
		{
			SizeF measure = graphics.MeasureString("000", font, new Point(0, 0), CENTER_STRING_FORMAT);
			return new Size(((int)measure.Height + 1) * starCount + (int)measure.Width + 1, (int)measure.Height + 1);
		}

		public static void DrawOkIcon(Graphics graphics, Color foreColor, Color backColor, Rectangle rect)
		{
			double r = (double)rect.Height / 4, R = rect.Height / 2.5;
			double x0 = (double)rect.Width / 2, y0 = (double)rect.Height / 2;
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
			double R = (double)rect.Height / 4;
			double x0 = (double)rect.Width / 2, y0 = (double)rect.Height / 2;
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

		public static void DrawCheckedIcon(Graphics graphics, Color foreColor, Color backColor, Rectangle rect)
		{
			double r = (double)rect.Height / 4, R = rect.Height / 2.5;
			double x0 = (double)rect.Width / 2, y0 = (double)rect.Height / 2;
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

		public static void DrawUncheckedIcon(Graphics graphics, Color foreColor, Color backColor, Rectangle rect)
		{
			Pen pen = new Pen(foreColor, 2);
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

		public static void DrawText(Graphics graphics, string text, Font font, Color foreColor, Color backColor, Rectangle rect, StringFormat format)
		{
			Region[] regions = graphics.MeasureCharacterRanges(text, font, rect, format);
			for (int i = 0; i < regions.Length; i++)
			{
				//graphics.DrawString(text[i].ToString(), font, SystemBrushes.Control, regions[i].);
			}
		}
	}
}
