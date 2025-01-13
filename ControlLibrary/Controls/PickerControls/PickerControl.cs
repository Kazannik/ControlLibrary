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
		private readonly ToolStripControlHost controlHost;
		private readonly T popupForm;
		private static ToolStripDropDown dropDown;

		public PickerControl()
		{
			popupForm = new T
			{
				BorderStyle = BorderStyle.None
			};

			controlHost = new ToolStripControlHost(popupForm);
			dropDown = new ToolStripDropDown();
			dropDown.Items.Add(controlHost);
			DropDownHeight = popupForm.Height;
			DropDownWidth = popupForm.Width;
			controlHost.Width = DropDownWidth;
			controlHost.Height = DropDownHeight;
		}

		public T PopupForm => (T)controlHost.Control;

		private void ShowDropDown()
		{
			if (dropDown != null)
			{
				controlHost.Width = DropDownWidth;
				controlHost.Height = DropDownHeight;

				dropDown.Show(this, 0, Height);
				//'   dropDown.Hide()
			}
		}

		private const int WM_USER = 0X400;
		private const int WM_REFLECT = WM_USER + 0X1C00;
		private const int WM_COMMAND = 0X111;
		private const int CBN_DROPDOWN = 0X7;

		public static int HIWORD(int n)
		{
			return (n >> 16) & 0xffff;
		}

		public static int HIWORD(IntPtr n)
		{
			return HIWORD(unchecked((int)(long)n));
		}

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
				if (dropDown != null)
				{
					dropDown.Dispose();
					dropDown = null;
				}
			}
			base.Dispose(disposing);
		}
	}
}