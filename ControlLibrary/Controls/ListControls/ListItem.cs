using ControlLibrary.Structures;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Font = System.Drawing.Font;

namespace ControlLibrary.Controls.ListControls
{
	public abstract class ListItem : IListItem
	{
		private const int BORDER_WIDTH = 1;
		private static readonly StringFormat CENTER_STRING_FORMAT = new StringFormat
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center
		};
		private const TextFormatFlags CENTER_FORMAT_FLAGS = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
		private static readonly StringFormat LEFT_STRING_FORMAT = new StringFormat
		{
			Alignment = StringAlignment.Far,
			LineAlignment = StringAlignment.Far
		};
		private const TextFormatFlags LEFT_FORMAT_FLAGS = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.LeftAndRightPadding;

		private Size versionSize;
		private Size captionSize;
		private Size textSize;
		private Size descriptionSize;

		private Version version;

		private string caption;
		private string text;
		private string description;
		private Rating rating;

		private bool isAppled;

		public ListItem()
		{
			versionSize = Size.Empty;
			captionSize = Size.Empty;
			textSize = Size.Empty;
			descriptionSize = Size.Empty;
			this.Size = Size.Empty;

			version = Version.Empty();
			caption = string.Empty;
			text = string.Empty;
			description = string.Empty;
			rating = Rating.Empty;
			ApplyButton = Rectangle.Empty;
			CancelButton = Rectangle.Empty;
			isAppled = false;
		}

		public virtual Version Version
		{
			get
			{
				return version;
			}
			set
			{
				if (version != value)
				{
					version = value;
					DoContentChanged();
				}
			}
		}

		public Size Size { get; private set; }

		public virtual string Caption
		{
			get
			{
				return caption;
			}
			set
			{
				if (caption != value)
				{ 
					caption = value;
					DoContentChanged();
				}
			}
		}

		public virtual string Text
		{
			get
			{
				return text;
			}
			set
			{
				if (text != value)
				{
					text = value;
					DoContentChanged();
				}
			}
		}

		public virtual string Description
		{
			get
			{
				return description;
			}
			set
			{
				if (description != value) 
				{
					description = value;
					DoContentChanged();
				}
			}
		}

		public virtual Rating Rating
		{
			get
			{
				return rating;
			}
			set
			{
				if (rating != value)
				{ 
					rating = value;
					DoContentChanged();
				}
			}
		}

		public virtual bool IsAppled
		{
			get
			{
				return isAppled;
			}
			set
			{
				if (isAppled != value)
				{
					isAppled = value;
					DoContentChanged();
				}
			}
		}

		public Rectangle ApplyButton { get; private set; }

		public Rectangle CancelButton { get; private set; }

		protected virtual Size OnMeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight)
		{
			Font boldFont = new Font(font.FontFamily, font.Size, FontStyle.Bold);
			SizeF measureVersion = graphics.MeasureString("XXX.XXX", boldFont, itemWidth - 2, CENTER_STRING_FORMAT);

			versionSize = new Size((int)measureVersion.Width, (int)measureVersion.Height + 2);

			captionSize = GetTextSize(graphics, Caption, font, itemWidth - (int)measureVersion.Width - 6);
			textSize = GetTextSize(graphics, Text, font, itemWidth);
						
			Font italicFont = new Font(font.FontFamily, font.Size - 1, FontStyle.Italic);
			descriptionSize = GetTextSize(graphics, Description, italicFont, itemWidth);
			
			int height = versionSize.Height + 4;
			if (versionSize.Height + 4 < captionSize.Height)
			{
				height = captionSize.Height;
			}
			height += textSize.IsEmpty ? 0 : textSize.Height + 2;
			height += descriptionSize.IsEmpty ? 0 : descriptionSize.Height + 2;
			
			if (textSize.IsEmpty && descriptionSize.IsEmpty)
			{
				height += 2;
			}

			height += versionSize.Height;

			Size = new Size(itemWidth, height);
			return Size;
		}

		private Size GetTextSize(Graphics graphics, string text, Font font, int width)
		{
			if (!string.IsNullOrEmpty(text))
			{
				SizeF measure = graphics.MeasureString(text, font, width - 8, LEFT_STRING_FORMAT);
				return new Size(width - 3, (int)measure.Height + 6);
			}
			else
			{
				return Size.Empty;
			}
		}

		protected virtual void OnDraw(DrawItemEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			Rectangle versionRectagle = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2,
				versionSize.Width, versionSize.Height);
			Font boldFont = new Font(e.Font.FontFamily, e.Font.Size, FontStyle.Bold);
			OnDrawVersion(new DrawItemEventArgs(e.Graphics, boldFont, versionRectagle, e.Index, e.State, e.ForeColor, e.BackColor));
			
			int top = versionRectagle.Y + versionRectagle.Height + 3;

			if (!captionSize.IsEmpty)
			{
				Rectangle rectagle = new Rectangle(e.Bounds.Width - captionSize.Width - 1, e.Bounds.Y, 
					captionSize.Width, captionSize.Height);
				OnDrawCaption(new DrawItemEventArgs(e.Graphics, e.Font, rectagle, e.Index, e.State, e.ForeColor, e.BackColor));
				if (top < rectagle.Y + rectagle.Height)
					top = rectagle.Y + rectagle.Height;
			}

			if (!textSize.IsEmpty || !descriptionSize.IsEmpty)
			{
				Pen linePen = e.State == (e.State | DrawItemState.Selected) ? new Pen(e.ForeColor) : SystemPens.InactiveCaption;
				e.Graphics.DrawLine(linePen,
					e.Bounds.X + 4, top - 1,
					e.Bounds.X + e.Bounds.Width - 8, top - 1);
			}

			if (!textSize.IsEmpty)
			{
				Rectangle rectagle = new Rectangle(e.Bounds.X + 2, top,
					textSize.Width, textSize.Height);
				top = rectagle.Y + rectagle.Height;
				OnDrawText(new DrawItemEventArgs(e.Graphics, e.Font, rectagle, e.Index, e.State, e.ForeColor, e.BackColor), Text);
			}

			if (!descriptionSize.IsEmpty)
			{
				Rectangle rectagle = new Rectangle(e.Bounds.X + 2, top,
					descriptionSize.Width, descriptionSize.Height);
				top = rectagle.Y + rectagle.Height;
				Font italicFont = new Font(e.Font.FontFamily, e.Font.Size - 1, FontStyle.Italic);
				OnDrawText(new DrawItemEventArgs(e.Graphics, italicFont, rectagle, e.Index, e.State, e.ForeColor, e.BackColor), Description);
			}
			Rectangle bottomBarRectagle = new Rectangle(e.Bounds.X + 2, top,
				e.Bounds.Width - 4, versionSize.Height);
				OnDrawBottomBar(new DrawItemEventArgs(e.Graphics, e.Font, bottomBarRectagle, e.Index, e.State, e.ForeColor, e.BackColor, e.RatingStarColor));
		}

		protected virtual void OnDrawVersion(DrawItemEventArgs e)
		{
			Brush BackColorBrush = new SolidBrush(e.ForeColor);
			Pen BackColorPen = new Pen(e.ForeColor, BORDER_WIDTH);
			Utils.Drawing.FillRoundedRectangle(e.Graphics, BackColorBrush, e.Bounds, (e.Bounds.Height / 3) == 0 ? 1 : (e.Bounds.Height / 3));
			TextRenderer.DrawText(e.Graphics, Version.ToString(), e.Font, e.Bounds, e.BackColor, e.ForeColor, CENTER_FORMAT_FLAGS);
			Utils.Drawing.DrawRoundedRectangle(e.Graphics, BackColorPen, e.Bounds, (e.Bounds.Height / 3) == 0 ? 1 : (e.Bounds.Height / 3));
			BackColorBrush.Dispose();
			BackColorPen.Dispose();
		}

		protected virtual void OnDrawCaption(DrawItemEventArgs e)
		{
			TextRenderer.DrawText(e.Graphics, Caption, e.Font, e.Bounds, e.ForeColor, e.BackColor, LEFT_FORMAT_FLAGS);
#if DEBUG
			//e.Graphics.DrawRectangle(Pens.Gray, e.Bounds);
#endif
		}

		protected virtual void OnDrawText(DrawItemEventArgs e, string text)
		{
			TextRenderer.DrawText(e.Graphics, text, e.Font, e.Bounds, e.ForeColor, e.BackColor, LEFT_FORMAT_FLAGS);
#if DEBUG
			//e.Graphics.DrawRectangle(Pens.Gray, e.Bounds);
#endif
		}

		protected virtual void OnDrawBottomBar(DrawItemEventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				int percent = Rating.GetPercent(i + 1, 5);
				Rectangle star = new Rectangle(e.Bounds.X + (e.Bounds.Height + 1) * i, e.Bounds.Y, e.Bounds.Height, e.Bounds.Height);
				Utils.Drawing.DrawStar(e.Graphics, e.ForeColor, e.RatingStarColor, percent, star);
			}

			Rectangle raitingRectagle = new Rectangle(e.Bounds.X + (e.Bounds.Height + 1) * 5, e.Bounds.Y + 1,
				e.Bounds.Height * 2, versionSize.Height);
			OnDrawRaiting(new DrawItemEventArgs(e.Graphics, e.Font, raitingRectagle, e.Index, e.State, e.ForeColor, e.BackColor));

			if (!isAppled)
			{
				ApplyButton = new Rectangle(e.Bounds.Width - (int)(e.Bounds.Height * 2.5), e.Bounds.Y - 1,
				e.Bounds.Height, versionSize.Height);
				Utils.Drawing.DrawButtonOk(e.Graphics, e.ForeColor, e.BackColor, ApplyButton);

				CancelButton = new Rectangle(e.Bounds.Width - e.Bounds.Height, e.Bounds.Y - 1,
					e.Bounds.Height, versionSize.Height);
				Utils.Drawing.DrawButtonCancel(e.Graphics, e.ForeColor, e.BackColor, CancelButton);
			}
		}

		protected virtual void OnDrawRaiting(DrawItemEventArgs e)
		{
			TextRenderer.DrawText(e.Graphics, Rating.ToString(), e.Font, e.Bounds, e.ForeColor, e.BackColor,  CENTER_FORMAT_FLAGS);
		}

		public event System.EventHandler<System.EventArgs> ContentChanged;

		protected void DoContentChanged()
		{
			OnContentChanged(new System.EventArgs());
		}

		protected virtual void OnContentChanged(System.EventArgs e)
		{
			ContentChanged?.Invoke(this, e);
		}

		Size IListItem.MeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight)
		{
			return OnMeasureBound(graphics: graphics, font: font, itemWidth: itemWidth, itemHeight: itemHeight);
		}

		void IListItem.Draw(DrawItemEventArgs e)
		{
			OnDraw(e);
		}
	}
}
