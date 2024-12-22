using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DatabaseToolSuite.Controls
{
	[DesignerCategory("code")]
	[ToolboxBitmap(typeof(TextBox))]
	[ComVisible(false)]
	public partial class NumericTextBox : TextBox
	{
		public NumericTextBox()
		{

		}

		public NumericTextBox(IContainer container)
		{
			container.Add(this);
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (e.KeyChar == 0)
				e.Handled = false;
			else if (e.KeyChar == '\b')
				e.Handled = false;
			else if (e.KeyChar == 0x0d)
				e.Handled = false;
			else if (char.IsNumber(e.KeyChar))
				e.Handled = false;
			else if (char.IsControl(e.KeyChar))
				e.Handled = false;

			else
				e.Handled = true;
			base.OnKeyPress(e);
		}


		public long Value
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Text)) return 0;
				if (long.TryParse(Text, out long result))
					return result;
				else
					return 0;
			}
		}

		string oldText = string.Empty;

		protected override void OnTextChanged(EventArgs e)
		{
			Regex regex = new Regex("^\\d+$");
			if (regex.IsMatch(this.Text))
			{
				oldText = Text;
				base.OnTextChanged(e);
			}
			else if (string.IsNullOrEmpty(Text))
			{
				oldText = Text;
				base.OnTextChanged(e);
			}
			else
			{
				Text = oldText;
			}
		}
	}
}

