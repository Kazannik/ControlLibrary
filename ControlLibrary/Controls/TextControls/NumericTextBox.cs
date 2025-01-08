using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ControlLibrary.Controls.TextControl
{
	[DesignerCategory("code")]
	[ToolboxBitmap(typeof(TextBox))]
	[ComVisible(false)]
	public partial class NumericTextBox : TextBox
	{
		public NumericTextBox()
		{
			InitializeComponent();
			base.Multiline = false;
		}

		public NumericTextBox(IContainer container)
		{
			container.Add(this);

			InitializeComponent();

			base.Multiline = false;
			base.MaxLength = 15;
			base.ScrollBars = ScrollBars.None;
		}

		[Browsable(false), ReadOnly(true)]
		public new bool Multiline
		{
			get { return base.Multiline; }
		}

		[Browsable(false), ReadOnly(true)]
		public new int MaxLength
		{
			get { return base.MaxLength; }
		}

		[Browsable(false), ReadOnly(true)]
		public new ScrollBars ScrollBars
		{
			get { return base.ScrollBars; }
		}

		bool IsNumeric(string s)
		{
			double output;
			return double.TryParse(s, out output);
		}

		public double Value
		{
			get
			{
				if (double.TryParse(base.Text, out double output))
				{
					return output;
				}
				else
				{
					return 0;
				}
			}
			set
			{
				base.Text = value.ToString();
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			Regex regex = new Regex("\\d+([,]\\d{1,}){0,1}");
			if (char.IsNumber(e.KeyChar))
			{
				e.Handled = !regex.IsMatch(Text + e.KeyChar.ToString());
			}
			else if (Text.IndexOf(',') < 0 && e.KeyChar == ',')
			{
				e.Handled = false;
			}
			else if (!char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		string oldText = string.Empty;

		protected override void OnTextChanged(EventArgs e)
		{
			Regex regex = new Regex("\\d+([,]\\d{1,}){0,1}");
			if (regex.IsMatch(Text))
			{
				oldText = Text;
				base.OnTextChanged(e);
			}
			else if (Text.Length == 0 || Text == ",")
			{
				oldText = Text;
				base.OnTextChanged(e);
			}
			else
			{
				Text = oldText;
			}
		}

		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
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
			// NumericTextBox
			// 
			this.ResumeLayout(false);

		}

		#endregion

	}
}

