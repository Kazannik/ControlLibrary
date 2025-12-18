// Ignore Spelling: HIWORD

using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ControlLibrary.Controls.PickerControls
{
	[System.ComponentModel.DesignerCategory("Form")]
	[System.Drawing.ToolboxBitmap(typeof(ComboBox))]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	public abstract class PickerControl<T> : ComboBox where T : UserControl, new()
	{
		private readonly ToolStripControlHost _controlHost;
		private readonly T _popupForm;
		private static ToolStripDropDown _dropDown;

		public PickerControl()
		{
			_popupForm = new T
			{
				BorderStyle = BorderStyle.None
			};

			_controlHost = new ToolStripControlHost(_popupForm);
			_dropDown = new ToolStripDropDown();
			_dropDown.Items.Add(_controlHost);
			DropDownHeight = _popupForm.Height;
			DropDownWidth = _popupForm.Width;
			_controlHost.Width = DropDownWidth;
			_controlHost.Height = DropDownHeight;
		}

		public T PopupForm => (T)_controlHost.Control;

		private void ShowDropDown()
		{
			if (_dropDown != null)
			{
				_controlHost.Width = DropDownWidth;
				_controlHost.Height = DropDownHeight;

				_dropDown.Show(this, 0, Height);
				//'   dropDown.Hide()
			}
		}

		private const int WM_USER = 0X400;
		private const int WM_REFLECT = WM_USER + 0X1C00;
		private const int WM_COMMAND = 0X111;
		private const int CBN_DROPDOWN = 0X7;

		public static int HIWORD(int n) => (n >> 16) & 0xffff;

		public static int HIWORD(IntPtr n) => HIWORD(unchecked((int)(long)n));

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_REFLECT + WM_COMMAND)
			{
				if (HIWORD(m.WParam) == CBN_DROPDOWN)
				{
					ShowDropDown();
					return;
				}
			}
			base.WndProc(ref m);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_dropDown != null)
				{
					_dropDown.Dispose();
					_dropDown = null;
				}
			}
			base.Dispose(disposing);
		}
	}
}