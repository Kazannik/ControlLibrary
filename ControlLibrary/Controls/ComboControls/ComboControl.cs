using ControlLibrary.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;

namespace ControlLibrary.Controls.ComboControls
{
	[DesignerCategory("code")]
	[ToolboxBitmap(typeof(ComboBox))]
	[ComVisible(false)]
	public abstract class ComboControl<T> : ComboBox where T : IComboItem
	{
		protected StringFormat sfCode;
		protected StringFormat sfCaption;
		protected StringFormat sfHighlight;

		#region Initialize

		[DebuggerNonUserCode()]
		public ComboControl(IContainer container) : this()
		{
			container?.Add(this);
		}

		[DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		private IContainer components;

		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
			components = new Container();
		}

		public ComboControl() : base()
		{
			sfCode = (StringFormat)StringFormat.GenericTypographic.Clone();
			sfCode.Alignment = StringAlignment.Center;
			sfCode.LineAlignment = StringAlignment.Center;
			sfCode.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;

			sfCaption = (StringFormat)StringFormat.GenericTypographic.Clone();
			sfCaption.Alignment = StringAlignment.Near;
			sfCaption.LineAlignment = StringAlignment.Near;
			sfCaption.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;

			InitializeComponent();

			base.DropDownStyle = ComboBoxStyle.DropDownList;
			DrawMode = DrawMode.OwnerDrawFixed;
			MaxDropDownItems = 20;
			DropDownWidth = 180;
			base.AutoSize = false;
			Width = 80;
			DropDownHeight = 180;
			Items.Clear();
		}

		private IEnumerable<T> List => from T item in Items select item;
		
		#endregion

		#region Draw Item

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			Graphics graphics = e.Graphics;

			SizeF CodeSize = graphics.MeasureString("FFFFF", Font);
			ItemHeight = (int)CodeSize.Height + (SystemInformation.BorderSize.Height * 4);
			DropDownHeight = (ItemHeight * 20) + (SystemInformation.BorderSize.Height * 4);

			Rectangle rectSelection = new Rectangle(e.Bounds.X + 1, e.Bounds.Y, e.Bounds.Width - 3, e.Bounds.Height - 1);
			Rectangle rectCode = new Rectangle(rectSelection.X + 2, rectSelection.Y + 2, (int)CodeSize.Width, rectSelection.Height - 4);
			Rectangle rectText = new Rectangle(rectCode.X + rectCode.Width + 6, rectCode.Y, e.Bounds.Width - rectCode.X - rectCode.Width - 6, rectCode.Height);

			Brush backCodeBrush, foreCodeBrush, backCaptionBrush, foreCaptionBrush;
			Pen borderPen;

			if (e.Index == -1)
			{
				backCodeBrush = SystemBrushes.Control;
				foreCodeBrush = SystemBrushes.ControlText;
				foreCaptionBrush = new SolidBrush(ForeColor);
				backCaptionBrush = new SolidBrush(BackColor);
				borderPen = new Pen(ForeColor);

				graphics.FillRectangle(backCaptionBrush, e.Bounds);
				graphics.FillRectangle(backCodeBrush, rectCode);
				graphics.DrawRectangle(borderPen, rectCode);
				graphics.DrawString("", Font, foreCodeBrush, rectCode, sfCode);
				graphics.DrawString("(не выбрано)", Font, foreCaptionBrush, rectText, sfCaption);
				return;
			} 
			else if (Items.Count <= e.Index) return;

			string itemCode = this[e.Index].Code;
			string itemCaption = this[e.Index].Caption.Trim();

			if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
			{
				backCodeBrush = new SolidBrush(BackColor);
				foreCodeBrush = new SolidBrush(ForeColor);
				foreCaptionBrush = new SolidBrush(ForeColor);
				backCaptionBrush = new SolidBrush(BackColor);
				borderPen = new Pen(ForeColor);

				graphics.FillRectangle(backCaptionBrush, e.Bounds);
				graphics.FillRectangle(backCodeBrush, rectCode);

				DrawHighlightText(graphics, rectCode, Font, foreCodeBrush, SystemBrushes.HighlightText, SystemBrushes.Highlight, 
					sfCode, itemCode,  codeBuffer);

				graphics.DrawRectangle(borderPen, rectCode);

				DrawHighlightText(graphics, rectText, Font, foreCaptionBrush, SystemBrushes.HighlightText, SystemBrushes.Highlight,
					sfCaption, itemCaption,  textBuffer);
			}
			else
			{
				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
				{
					backCodeBrush = new LinearGradientBrush(rectCode, Color.FromArgb(249, 249, 249), Color.FromArgb(241, 241, 241), LinearGradientMode.Vertical);
					foreCodeBrush = new SolidBrush(Color.FromArgb(0, 102, 204));
					backCaptionBrush = SystemBrushes.Highlight;
					foreCaptionBrush = SystemBrushes.HighlightText;
					borderPen = new Pen(Color.FromArgb(0, 102, 204), 1);
				}
				else
				{
					backCodeBrush = new SolidBrush(BackColor);
					foreCodeBrush = new SolidBrush(ForeColor);
					backCaptionBrush = new SolidBrush(BackColor);
					foreCaptionBrush = new SolidBrush(ForeColor);
					borderPen = new Pen(ForeColor, 1);
				}

				graphics.FillRectangle(backCaptionBrush, e.Bounds);
				graphics.FillRectangle(backCodeBrush, rectCode);
				graphics.DrawRectangle(borderPen, rectCode);
				graphics.DrawString(itemCode, Font, foreCodeBrush, rectCode, sfCode);
				graphics.DrawString(itemCaption, Font, foreCaptionBrush, rectText, sfCaption);
			}
		}

		private void DrawHighlightText(Graphics graphics, Rectangle rectangle, Font font, 
			Brush foreColorBrush, Brush highlightForeColor, Brush highlightBackColor, 
			 StringFormat format, string text, string buffer)
		{
			if (string.IsNullOrWhiteSpace(buffer) || !text.Contains(buffer))
			{
				graphics.DrawString(text, font, foreColorBrush, rectangle, format);
			}
			else
			{
				string firstText = text.Substring(0, text.IndexOf(buffer, StringComparison.CurrentCultureIgnoreCase));
				SizeF firstSize = graphics.MeasureString(firstText, font, rectangle.Width, format);

				string centerText = text.Substring(firstText.Length, buffer.Length);
				SizeF centerSize = graphics.MeasureString(centerText, font, rectangle.Width, format);

				string lastText = text.Substring(firstText.Length + centerText.Length);
				SizeF lastSize = graphics.MeasureString(lastText, font, rectangle.Width, format);

				Rectangle firstRect;
				if (format.Alignment == StringAlignment.Near)
				{
					firstRect = new Rectangle(rectangle.X, rectangle.Y, (int)firstSize.Width, rectangle.Height);
				}
				else if (format.Alignment == StringAlignment.Far)
				{
					SizeF textSize = graphics.MeasureString(text, font, rectangle.Width, format);
					firstRect = new Rectangle(rectangle.Width - (int)textSize.Width, rectangle.Y, (int)firstSize.Width, rectangle.Height);
				}
				else
				{
					SizeF textSize = graphics.MeasureString(text, font, rectangle.Width, format);
					firstRect = new Rectangle(((rectangle.Width - (int)textSize.Width + 2) / 2) + 3, rectangle.Y, (int)firstSize.Width, rectangle.Height);
				}

				Rectangle centerRect = new Rectangle(firstRect.X + firstRect.Width + 1, rectangle.Y, (int)centerSize.Width, rectangle.Height);
				Rectangle lastRect = new Rectangle(centerRect.X + centerRect.Width + 1, rectangle.Y, (int)lastSize.Width, rectangle.Height);

				graphics.FillRectangle(highlightBackColor, centerRect);

				graphics.DrawString(firstText, font, foreColorBrush, firstRect, format);
				graphics.DrawString(centerText, font, highlightForeColor, centerRect, sfHighlight);
				graphics.DrawString(lastText, font, foreColorBrush, lastRect, format);
			}
		}

		#endregion Draw Item

		private string codeBuffer = string.Empty;
		private string textBuffer = string.Empty;
		private int findNext = 0;

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				codeBuffer = string.Empty;
				textBuffer = string.Empty;
				SelectedIndex = -1;
				findNext = 0;
			}
			base.OnKeyDown(e);
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			BeginUpdate();

			if (e.KeyChar == 0)
				e.Handled = false;
			else if (e.KeyChar == '\b')
			{
				e.Handled = false;
				if (!string.IsNullOrWhiteSpace(codeBuffer))
				{
					codeBuffer = codeBuffer.Substring(0, codeBuffer.Length - 1);
					if (!string.IsNullOrEmpty(codeBuffer)) FindCode(codeBuffer);
				}
				if (!string.IsNullOrWhiteSpace(textBuffer))
				{
					textBuffer = textBuffer.Substring(0, textBuffer.Length - 1);
					if (!string.IsNullOrEmpty(textBuffer)) FindText(textBuffer);
				}
				findNext = 0;
			}
			else if (e.KeyChar == 0x0d)
			{
				e.Handled = false;
				findNext += 1;
				if (!string.IsNullOrWhiteSpace(codeBuffer))
				{
					if (!FindCode(codeBuffer).Any())
						findNext = 0;
				}
				else if (!string.IsNullOrWhiteSpace(textBuffer))
				{
					if (!FindText(textBuffer).Any())
						findNext = 0;
				}
			}
			else if (e.KeyChar >= 48 & e.KeyChar <= 57)
			{
				e.Handled = false;
				if (FindCode(codeBuffer + e.KeyChar).Any())
				{
					codeBuffer += e.KeyChar;
					textBuffer = string.Empty;
				}
				findNext = 0;
			}
			else if (char.IsLetter(e.KeyChar))
			{
				e.Handled = false;
				if (FindText(textBuffer + e.KeyChar).Any())
				{
					textBuffer += e.KeyChar;
					codeBuffer = string.Empty;
				}
				findNext = 0;
			}
			else
				e.Handled = true;
			base.OnKeyPress(e);

			EndUpdate();
		}

		private IEnumerable<T> FindCode(string code)
		{
			Argument.AssertNotNullOrEmpty(code, nameof(code));
			string lowerCode = code.ToLower();
			return List.Where(x => x.Code.ToLower().Contains(lowerCode));			
		}

		private IEnumerable<T> FindText(string text)
		{
			Argument.AssertNotNullOrEmpty(text, nameof(text));
			string lowerText = text.ToLower();
			return List.Where(x => x.Caption.ToLower().Contains(lowerText));
		}

		public string Guid
		{
			get => SelectedItem != null ? SelectedItem.Code : string.Empty;
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					SelectedIndex = -1;
				}
				else
				{
					if (ContainsGuid(value))
					{
						SelectedItem = GetItem(value);
					}
					else
					{
						SelectedIndex = -1;
					}
				}
			}
		}

		public string Code
		{
			get => SelectedItem != null ? SelectedItem.Code : string.Empty;
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					SelectedIndex = -1;
				}
				else
				{
					if (Contains(value))
					{
						SelectedItem = GetItem(value);
					}
					else
					{
						SelectedIndex = -1;
					}
				}
			}
		}

		public long Id
		{
			get => SelectedItem != null ? SelectedItem.Id : -1;
			set
			{
				if (Contains(value))
				{
					SelectedItem = GetItem(value);
				}
				else
				{
					SelectedIndex = -1;
				}
			}
		}

		public long? Value => long.TryParse(Code, out long result) ? result : (long?)null;

		[ReadOnly(true)]
		public T this[int index] => (T)Items[index: index];

		[ReadOnly(true)]
		public new ComboBoxStyle DropDownStyle => base.DropDownStyle;

		protected void Insert(int index, T item)
		{
			Argument.AssertNotNull(item, nameof(item));
			if (Contains(code: item.Code))
			{
				throw new ArgumentException(string.Format("Элемент с кодом [{0}] ранее добавлен в коллекцию.", item.Code), nameof(item));
			}
			Items.Insert(index, item);
		}

		protected void Remove(T value) => Items.Remove(value);

		protected void RemoveAt(int index) => Items.RemoveAt(index);

		protected void Clear() => Items.Clear();

		public int Add(T item)
		{
			Argument.AssertNotNull(item, nameof(item));
			if (Contains(code: item.Code)) 
			{
				throw new ArgumentException(string.Format("Элемент с кодом [{0}] ранее добавлен в коллекцию.", item.Code ), nameof(item));
			}			
			return Items.Add(item);
		}

		public bool Contains(long id)
		{
			Argument.AssertNotNull(id, nameof(id));
			return List.Any(x=> x.Id == id);			
		}

		public bool Contains(string code)
		{
			Argument.AssertNotNullOrEmpty(code, nameof(code));
			return List.Any(x => x.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));			
		}
		public bool ContainsGuid(string guid)
		{
			Argument.AssertNotNullOrEmpty(guid, nameof(guid));
			return List.Any(x => x.Guid.Equals(guid, StringComparison.CurrentCultureIgnoreCase));
		}

		public new T SelectedItem
		{
			get => (T)base.SelectedItem;
			set
			{
				if (value == null)
					base.SelectedItem = null;
				else
				{
					T item;
					if (!string.IsNullOrEmpty(value.Guid))
					{
						item = GetItemOfGuid(value.Guid);
					}
					else if (!string.IsNullOrEmpty(value.Code))
					{
						item = GetItem(value.Code);
					}
					else
					{
						item = GetItem(value.Id);
					}
					base.SelectedItem = item;
				}
			} 
		}

		public T GetItem(long id)
		{
			Argument.AssertNotNull(id, nameof(id));
			return List.FirstOrDefault(x => x.Id == id);			
		}

		public T GetItem(string code)
		{
			Argument.AssertNotNullOrEmpty(code, nameof(code));
			return List.FirstOrDefault(x => x.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
		}

		public T GetItemOfGuid(string guid)
		{
			Argument.AssertNotNullOrEmpty(guid, nameof(guid));
			return List.FirstOrDefault(x => x.Guid.Equals(guid, StringComparison.CurrentCultureIgnoreCase));
		}

		public class ItemComparer : IComparer<T>
		{
			int IComparer<T>.Compare(T x, T y) => Compare(x, y);

			public static int Compare(T x, T y)
			{
				return int.TryParse(x.Code, out int xCode)
					&& int.TryParse(y.Code, out int yCode)
					? decimal.Compare(xCode, yCode)
					: string.Compare(x.Code, y.Code);
			}
		}
	}
}