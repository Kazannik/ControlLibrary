using ControlLibrary.Structures;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ControlLibrary.Controls.RatingControls
{
	[ToolboxBitmap(typeof(ComboBox))]
	[ComVisible(false)]
	public class RatingControl : Control
	{
		private const int STAR_COUNT = 5;
		private readonly BufferedGraphicsContext context;

		protected static readonly StringFormat CENTER_STRING_FORMAT = new StringFormat
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center
		};

		protected Size ratingBoxSize;

		private Rating rating;
		private Rectangle[] rectangles;

		private Color starsColor1 = SystemColors.ActiveCaption;
		private Color starsColor2 = SystemColors.InactiveCaption;

		public Color StarsColor1
		{
			get => starsColor1;
			set
			{
				if (starsColor1 != value)
				{
					starsColor1 = value;
					Invalidate();
				}
			}
		}

		public Color StarsColor2
		{
			get => starsColor2;
			set
			{
				if (starsColor2 != value)
				{
					starsColor2 = value;
					Invalidate();
				}
			}
		}


		private IContainer components = null;

		public RatingControl()
		{
			context = BufferedGraphicsManager.Current;
			InitializeComponent();
		}

		public RatingControl(IContainer container)
		{
			context = BufferedGraphicsManager.Current;
			InitializeComponent();

			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			container.Add(this);
		}

		protected override void OnResize(EventArgs e)
		{
			Size = OnMeasureBound(CreateGraphics(), Font);
			base.OnResize(e);
		}

		protected override void OnFontChanged(EventArgs e)
		{
			Size = OnMeasureBound(CreateGraphics(), Font);
			base.OnFontChanged(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			Invalidate();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			Invalidate();
			base.OnLostFocus(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			BufferedGraphics grafx = context.Allocate(e.Graphics, e.ClipRectangle);

			PaintEventArgs args = new PaintEventArgs(grafx.Graphics, e.ClipRectangle);

			Utils.Drawing.SetGraphicsStyle(args.Graphics);

			if (DesignMode)
			{
				args.Graphics.Clear(BackColor);
				Utils.Drawing.DrawRating(graphics: args.Graphics, font: Font,
					borderColor: ForeColor,
					backBrush: new LinearGradientBrush(rect: args.ClipRectangle, color1: StarsColor1, color2: StarsColor2, 180),
					textColor: ForeColor,
					rect: args.ClipRectangle, starCount: STAR_COUNT);
			}
			else
			{
				if (Focused)
				{
					Brush brush = Rating == Rating.Empty ? new SolidBrush(BackColor) : Rating.Value > 0 ? new SolidBrush(StarsColor1) : new SolidBrush(StarsColor2);

					args.Graphics.Clear(SystemColors.Highlight);
					rectangles = Utils.Drawing.DrawRating(
						graphics: args.Graphics,
						font: Font,
						borderColor: SystemColors.HighlightText,
						backBrush: brush,
						textColor: SystemColors.HighlightText,
						rect: args.ClipRectangle, rating: Rating.Value, starCount: STAR_COUNT);
				}
				else
				{
					args.Graphics.Clear(BackColor);

					rectangles = Utils.Drawing.DrawRating(
						graphics: args.Graphics,
						font: Font,
						borderColor: ForeColor,
						backBrush: Rating == Rating.Empty ? (Brush)new SolidBrush(BackColor) : new LinearGradientBrush(args.ClipRectangle, StarsColor1, StarsColor2, 35),
						textColor: Rating == Rating.Empty ? ForeColor : StarsColor1,
						rect: args.ClipRectangle, rating: Rating.Value, starCount: STAR_COUNT);
				}

			}
			grafx.Render(e.Graphics);
			base.OnPaint(e);
		}

		protected override void OnClientSizeChanged(EventArgs e)
		{
			context.MaximumBuffer = new Size(ClientSize.Width + 1, ClientSize.Height + 1);
			base.OnClientSizeChanged(e);
		}

		protected Size OnMeasureBound(Graphics graphics, Font font)
		{
			return Utils.Drawing.MeasureRating(graphics: graphics, font: font, starCount: STAR_COUNT);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				if (rectangles[i].Contains(e.Location))
				{
					SetRating(i);
					Focus();
				}
			}
			base.OnMouseClick(e);
		}

		private void SetRating(int index)
		{
			switch (index)
			{
				case 0:
					if (Rating > Rating.MinValue) Rating--;
					break;
				case 1:
					if (Rating.Value == 3)
						Rating = (Rating)4;
					else if (Rating.Value >= 0)
						Rating = (Rating)3;
					else if (Rating.Value == -7)
						Rating = (Rating)(-8);
					else if (Rating.Value < 0)
						Rating = (Rating)(-7);
					break;
				case 2:
					if (Rating.Value == 5)
						Rating = (Rating)6;
					else if (Rating.Value >= 0)
						Rating = (Rating)5;
					else if (Rating.Value == -5)
						Rating = (Rating)(-6);
					else if (Rating.Value < 0)
						Rating = (Rating)(-5);
					break;
				case 3:
					if (Rating.Value == 7)
						Rating = (Rating)8;
					else if (Rating.Value >= 0)
						Rating = (Rating)7;
					else if (Rating.Value == -3)
						Rating = (Rating)(-4);
					else if (Rating.Value < 0)
						Rating = (Rating)(-3);
					break;
				case 4:
					if (Rating < Rating.MaxValue) Rating++;
					break;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Cursor = rectangles.Take(STAR_COUNT).Where(x => x.Contains(e.Location)).Any() ? Cursors.Hand : Cursors.Default;
			base.OnMouseMove(e);
		}

		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Left ||
				e.KeyCode == Keys.Right)
			{
				e.IsInputKey = true;
			}
			base.OnPreviewKeyDown(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == Keys.Left)
			{
				if (Rating > Rating.MinValue)
				{
					Rating--;
					e.SuppressKeyPress = true;
				}
			}
			else if (e.KeyData == Keys.Right)
			{
				if (Rating < Rating.MaxValue)
				{
					Rating++;
					e.SuppressKeyPress = true;
				}
			}
			base.OnKeyDown(e);
		}

		private void InitializeComponent()
		{
			components = new Container();
			SuspendLayout();
			// 
			// Controls
			// 
			ResumeLayout(false);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Rating

		public Rating Rating
		{
			get => rating;
			set
			{
				if (!Equals(rating, value))
				{
					rating = value;
					DoRatingChanged();
				}
			}
		}

		public event EventHandler<RatingEventArgs> RatingChanged;

		public void DoRatingChanged()
		{
			Invalidate();
			OnRatingChanged(new RatingEventArgs(rating));
		}

		protected virtual void OnRatingChanged(RatingEventArgs e) => RatingChanged?.Invoke(this, e);

		#endregion
	}

	public class RatingEventArgs : EventArgs
	{
		public RatingEventArgs(Rating args) => Rating = args;

		public Rating Rating { get; }
	}
}
