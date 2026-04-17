// Ignore Spelling: Priod

using ControlLibrary.Structures;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace ControlLibrary.Controls.PriodControls
{
	internal class PeriodControl : Control
	{
		private Rectangle periodRectangle;
		private Rectangle todateRectangle;

		private readonly DateTimeFormatInfo FormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
		private readonly StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center };
		private int month_label_width = 0;
		private int month_label_height = 0;
		private const int month_label_border = 2;

		private Period period;

		private PeriodBoxButtonCollection buttons;
		private PeriodBoxButton HoveringButton;
		private PeriodBoxButton LeftClickedButton;
		private PeriodBoxButton RightClickedButton;
		private PeriodBoxButton SelectedButton;

		#region Constructor

		public PeriodControl() : base()
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
			period = Period.Today();
			InitializeComponent();
			InitializeButton();
		}

		private PeriodControl(string text) : base(text: text)
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
			period = Period.Today();
			InitializeComponent();
			InitializeButton();
		}

		public PeriodControl(Control parent, string text) : base(parent: parent, text: text)
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
			period = Period.Today();
			InitializeComponent();
			InitializeButton();
		}

		public PeriodControl(string text, int left, int top, int width, int height) : base(text: text, left: left, top: top, width: width, height: height)
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
			period = Period.Today();
			InitializeComponent();
			InitializeButton();
		}

		public PeriodControl(Control parent, string text, int left, int top, int width, int height) : base(parent: parent, text: text, left: left, top: top, width: width, height: height)
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
			period = Period.Today();
			InitializeComponent();
			InitializeButton();
		}

		private void InitializeButton()
		{
			BackColor = SystemColors.Window;
			ForeColor = SystemColors.WindowText;

			buttons = new PeriodBoxButtonCollection(this);
			Graphics graphics = CreateGraphics();
			for (int i = 1; i <= 12; i++)
			{
				string monthName = FormatInfo.GetMonthName(i);
				SizeF labelSize = graphics.MeasureString(monthName, Font);
				if (month_label_height < labelSize.Height) month_label_height = (int)labelSize.Height;
				if (month_label_width < labelSize.Width) month_label_width = (int)labelSize.Width;
				buttons.Add(new PeriodBoxButton(i, monthName));
			}
			month_label_height += month_label_border * 2;
			month_label_width += month_label_border * 2;

			if (month_label_width > 0)
			{
				Width = month_label_border + ((month_label_width + month_label_border) * 3);
				Height = month_label_border + ((month_label_height + month_label_border) * 6);
			}
			MinimumSize = new Size(Width, Height);
			ClientSize = new Size(Width, Height);

			periodRectangle = new Rectangle(month_label_height + month_label_border, month_label_border, Width - (month_label_height * 2) - (month_label_border * 4), month_label_height);
			todateRectangle = new Rectangle(month_label_height + month_label_border, Height - month_label_height - month_label_border, (int)(month_label_height * 1.5), month_label_height);

			buttons.Add(new PeriodBoxButton(0, ""));
			buttons.Add(new PeriodBoxButton(0, ""));
		}

		#region Код, автоматически созданный конструктором компонентов

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// PeriodBox
			// 
			MouseDown += new MouseEventHandler(PeriodControl_MouseDown);
			MouseClick += new MouseEventHandler(PeriodBox_MouseClick);
			MouseLeave += new EventHandler(PeriodBox_MouseLeave);
			MouseMove += new MouseEventHandler(PeriodBox_MouseMove);
			MouseUp += new MouseEventHandler(PeriodBox_MouseUp);

			ResumeLayout(false);
		}

		#endregion

		#endregion

		protected override void OnResize(EventArgs e)
		{
			if (month_label_width > 0)
			{
				Width = month_label_border + ((month_label_width + month_label_border) * 3);
				Height = month_label_border + ((month_label_height + month_label_border) * 6);
				ClientSize = new Size(Width, Height);
				MinimumSize = new Size(Width, Height);
				return;
			}
			base.OnResize(e);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
			pevent.Graphics.Clear(BackColor);
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rect = this.ClientRectangle;

			Graphics graphics = e.Graphics;
			int startY = month_label_height + (month_label_border * 2);
			int startX = month_label_border;
			int i = 0;
			for (int y = 0; y <= 3; y++)
			{
				for (int x = 0; x <= 2; x++)
				{
					Rectangle rectangle = new Rectangle(startX + ((month_label_border + month_label_width) * x), startY + ((month_label_border + month_label_height) * y), month_label_width, month_label_height);
					buttons[i].rectangle = rectangle;
					PaintButton(buttons[i], graphics);
					i++;
				}
			}

			//graphics.DrawString(period.ToShortDateString(), Font, new SolidBrush(ForeColor), periodRectangle, sf);

			//Color borderColor = GetBorderColor(ButtonState.Selected);
			//Pen borderColorPen = new Pen(borderColor, 1);
			//graphics.DrawRectangle(borderColorPen, todateRectangle);
			//borderColorPen.Dispose();

			base.OnPaint(new PaintEventArgs(e.Graphics, new Rectangle(e.ClipRectangle.Location, rect.Size)));
		}

		private void PaintButton(PeriodBoxButton button, Graphics graphics)
		{
			Color foreColor;
			if (button.Equals(HoveringButton))
			{
				if (LeftClickedButton == null)
				{
					if (button.Equals(SelectedButton))
					{
						FillButton(button, graphics, ButtonState.Selected | ButtonState.Hovering);
						foreColor = GetForeColor(ButtonState.Selected | ButtonState.Hovering);
					}
					else
					{
						FillButton(button, graphics, ButtonState.Hovering);
						foreColor = GetForeColor(ButtonState.Hovering);
					}
				}
				else
				{
					FillButton(button, graphics, ButtonState.Selected | ButtonState.Hovering);
					foreColor = GetForeColor(ButtonState.Selected | ButtonState.Hovering);
				}
			}
			else
			{
				if (button.Equals(SelectedButton))
				{
					FillButton(button, graphics, ButtonState.Selected);
					foreColor = GetForeColor(ButtonState.Selected);
				}
				else
				{
					FillButton(button, graphics, ButtonState.Passive);
					foreColor = GetForeColor(ButtonState.Passive);
				}
			}

			if (button.ForeColor != foreColor)
			{
				button.ForeColor = foreColor;
				Brush foreColorBrush = new SolidBrush(foreColor);
				graphics.DrawString(button.Text, Font, foreColorBrush, button.rectangle, sf);
				foreColorBrush.Dispose();
			}
		}

		private void FillButton(PeriodBoxButton button, Graphics graphics, ButtonState buttonState)
		{
			Color backColor = GetButtonColor(buttonState, 0);
			if (button.BackColor != backColor)
			{
				button.BackColor = backColor;
				Brush backColorBrush = new LinearGradientBrush(button.rectangle, backColor, GetButtonColor(buttonState, 1), LinearGradientMode.Vertical);
				graphics.FillRectangle(backColorBrush, button.rectangle);
				backColorBrush.Dispose();
			}

			if (buttonState == ButtonState.Passive) return;

			Color borderColor = GetBorderColor(buttonState);
			if (button.BorderColor != borderColor)
			{
				button.BorderColor = borderColor;
				Pen borderColorPen = new Pen(borderColor, 1);
				graphics.DrawRectangle(borderColorPen, button.rectangle);
				borderColorPen.Dispose();
			}
		}

		#region Colors

		private Color GetButtonColor(ButtonState buttonState, int colorIndex)
		{
			switch (buttonState)
			{
				case ButtonState.Hovering | ButtonState.Selected:
					if (colorIndex == 0) return Color.FromArgb(235, 244, 253);
					if (colorIndex == 1) return Color.FromArgb(194, 220, 253);
					break;
				case ButtonState.Hovering:
					if (colorIndex == 0) return Color.FromArgb(253, 254, 255);
					if (colorIndex == 1) return Color.FromArgb(243, 248, 255);
					break;
				case ButtonState.Selected:
					if (colorIndex == 0) return Color.FromArgb(249, 249, 249);
					if (colorIndex == 1) return Color.FromArgb(241, 241, 241);
					break;
				case ButtonState.Passive:
					if (colorIndex == 0) return BackColor;
					if (colorIndex == 1) return BackColor;
					break;
				default:
					if (colorIndex == 0) return BackColor;
					if (colorIndex == 1) return BackColor;
					break;
			}
			return BackColor;
		}

		private Color GetBorderColor(ButtonState buttonState)
		{
			switch (buttonState)
			{
				case ButtonState.Hovering | ButtonState.Selected:
					return Color.FromArgb(0, 102, 204);
				case ButtonState.Hovering:
					return Color.FromArgb(185, 215, 252);
				case ButtonState.Selected:
					return Color.FromArgb(0, 102, 204);
				case ButtonState.Passive:
					return BackColor;
				default:
					return BackColor;
			}
		}

		private Color GetForeColor(ButtonState buttonState)
		{
			switch (buttonState)
			{
				case ButtonState.Hovering | ButtonState.Selected:
					return Color.FromArgb(0, 102, 204);
				case ButtonState.Hovering:
					return Color.FromArgb(0, 102, 204);
				case ButtonState.Selected:
					return Color.FromArgb(0, 102, 204);
				case ButtonState.Passive:
					return ForeColor;
				default:
					return ForeColor;
			}
		}

		#endregion

		private void PeriodControl_MouseDown(object sender, MouseEventArgs e)
		{
			RightClickedButton = null;
			PeriodBoxButton button = buttons[e.X, e.Y];
			if (button != null)
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						SelectedButton = button;
						Value = Period.Create(Value.Year, SelectedButton.Index);
						OnButtonClick(new EventArgs());
						Invalidate();
						break;
					case MouseButtons.Right:
						RightClickedButton = button;
						Invalidate();
						break;
				}
			}
		}

		private void PeriodBox_MouseClick(object sender, MouseEventArgs e)
		{
			//RightClickedButton = null;
			//PeriodBoxButton button = buttons[e.X, e.Y];
			//if (button != null)
			//{
			//	switch (e.Button)
			//	{
			//		case MouseButtons.Left:
			//			SelectedButton = button;
			//			Value = Period.Create(Value.Year, SelectedButton.Index);
			//			OnButtonClick(new EventArgs());
			//			Invalidate();
			//			break;
			//		case MouseButtons.Right:
			//			RightClickedButton = button;
			//			Invalidate();
			//			break;
			//	}
			//}
		}

		private void PeriodBox_MouseLeave(object sender, EventArgs e)
		{
			if (RightClickedButton == null)
			{
				if (HoveringButton != null)
				{
					Rectangle rec = HoveringButton.rectangle;
					HoveringButton = null;
					Invalidate(rec);
				}
			}
		}

		private void PeriodBox_MouseMove(object sender, MouseEventArgs e)
		{
			PeriodBoxButton focusButton = buttons[e.X, e.Y];
			if (focusButton != null)
			{
				Cursor = Cursors.Hand;
				if (HoveringButton != focusButton)
				{
					HoveringButton = focusButton;
					Invalidate(HoveringButton.rectangle);
				}
			}
			else
			{
				if (HoveringButton != null)
				{
					Rectangle rec = HoveringButton.rectangle;
					HoveringButton = null;
					Invalidate(rec);
				}
				Cursor = Cursors.Default;
			}
		}

		private void PeriodBox_MouseUp(object sender, MouseEventArgs e)
		{
			LeftClickedButton = null;
		}

		#region Value

		public Period Value
		{
			get => period;
			set
			{
				if (!Equals(period, value))
				{
					period = value;
					DoValueChanged();
				}
			}
		}

		public event EventHandler<PeriodEventArgs> ValueChanged;

		public void DoValueChanged()
		{
			Invalidate();
			OnValueChanged(new PeriodEventArgs(period));
		}

		protected virtual void OnValueChanged(PeriodEventArgs e) => ValueChanged?.Invoke(this, e);

		#endregion

		#region Click

		public event EventHandler ButtonClick;

		protected virtual void OnButtonClick(EventArgs e) => ButtonClick?.Invoke(this, e);

		#endregion

		internal class PeriodBoxButton
		{
			internal PeriodControl owner;
			internal bool visible = true;
			internal bool allowed = true;
			//Private _Image As Icon = My.Resources.DefaultIcon
			internal Rectangle rectangle;
			internal bool selected = false;

			internal Color BackColor = Color.Transparent;
			internal Color BorderColor = Color.Transparent;
			internal Color ForeColor = Color.Transparent;

			public PeriodBoxButton(int index, string text)
			{
				Index = index;
				Text = text;
			}

			public string Text { get; }

			public int Index { get; }
		}

		internal class PeriodBoxButtonCollection : CollectionBase
		{
			private readonly PeriodControl owner;

			public PeriodBoxButtonCollection(PeriodControl owner) : base()
			{
				this.owner = owner;
			}

			public PeriodBoxButton this[int index] => (PeriodBoxButton)List[index];

			public PeriodBoxButton this[string text]
			{
				get
				{
					foreach (PeriodBoxButton item in List)
					{
						if (item.Text.Equals(text)) return item;
					}
					return null;
				}
			}

			public PeriodBoxButton this[int x, int y]
			{
				get
				{
					foreach (PeriodBoxButton item in List)
					{
						if (item.rectangle.Contains(new Point(x, y)))
						{
							return item;
						}
					}
					return null;
				}
			}

			public int Add(PeriodBoxButton item)
			{
				item.owner = owner;
				return List.Add(item);
			}

			public void AddRange(PeriodBoxButtonCollection items)
			{
				foreach (PeriodBoxButton item in items)
				{
					Add(item);
				}
			}

			public int IndexOf(PeriodBoxButton item) => List.IndexOf(item);

			public void Insert(int index, PeriodBoxButton value)
			{
				value.owner = owner;
				List.Insert(index, value);
			}

			public void Remove(PeriodBoxButton value) => List.Remove(value);

			public bool Contains(PeriodBoxButton value) => List.Contains(value);

			protected override void OnValidate(object value)
			{
				if (!typeof(PeriodBoxButton).IsAssignableFrom(value.GetType()))
				{
					throw new ArgumentException("value не является типом PeriodBoxButton.", nameof(value));
				}
			}
		}

		private enum ButtonState : int
		{
			Passive = 0,
			Hovering = 1,
			Selected = 2
		}
	}
}
