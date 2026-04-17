// Ignore Spelling: HIWORD

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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
			popupForm = new T()
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
				controlHost.Height = DropDownHeight;
				controlHost.Width = DropDownWidth;

				dropDown.Show(this, 0, Height);
				//'   dropDown.Hide()
			}
		}

		private const int WM_USER = 0X400;
		private const int WM_REFLECT = WM_USER + 0X1C00;
		private const int WM_COMMAND = 0X111;

		private const int CBN_DROPDOWN = 7;
		private const int CBN_CLOSEUP = 8;
		private const int CBN_DBLCLK = 2;
		private const int CBN_EDITCHANGE = 5;
		private const int CBN_EDITUPDATE = 6;
		private const int CBN_ERRSPACE = -1;
		private const int CBN_KILLFOCUS = 4;
		private const int CBN_SELCHANGE = 1;
		private const int CBN_SELENDCANCEL = 10;
		private const int CBN_SELENDOK = 9;
		private const int CBN_SETFOCUS = 3;

		public static int HIWORD(int n) => (n >> 16) & 0xffff;

		public static int HIWORD(IntPtr n) => HIWORD(unchecked((int)(long)n));

		//protected override void WndProc(ref Message m)
		//{
		//	if (m.Msg == WM_REFLECT + WM_COMMAND)
		//	{
		//		if (HIWORD(m.WParam) == CBN_DROPDOWN)
		//		{
		//			ShowDropDown();
		//			return;
		//		}
		//	}
		//	base.WndProc(ref m);
		//}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_REFLECT + WM_COMMAND)
			{
				int CBN = HIWORD(m.WParam);
				switch (CBN)
				{
					case CBN_SETFOCUS:
						Debug.WriteLine(DateTime.Now + " CBN_SETFOCUS");
						break;
					case CBN_KILLFOCUS:
						Debug.WriteLine(DateTime.Now + " CBN_KILLFOCUS");
						break;
					case CBN_DROPDOWN:
						Debug.WriteLine(DateTime.Now + " CBN_DROPDOWN");
						ShowDropDown();
						return; // break;
					case CBN_CLOSEUP:
						Debug.WriteLine(DateTime.Now + " CBN_CLOSEUP");
						break;
					case CBN_SELENDCANCEL:
						Debug.WriteLine(DateTime.Now + " CBN_SELENDCANCEL");
						break;
					default:
						Debug.WriteLine(DateTime.Now + " Other: " + HIWORD((int)m.WParam));
						break;
				}
			}
			base.WndProc(ref m);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				dropDown?.Dispose();
				dropDown = null;
			}
			base.Dispose(disposing);
		}


		[ToolboxItem(false)]
		private class ControlDropDown : ToolStripDropDownMenu
		{
			public ControlDropDown()
			{
				this.ShowImageMargin = this.ShowCheckMargin = false;
				this.AutoSize = false;
				this.DoubleBuffered = true;
				this.Padding = Margin = Padding.Empty;
			}

			public int AddControl(Control control)
			{
				var host = new ToolStripControlHost(control);
				host.Padding = host.Margin = Padding.Empty;
				host.BackColor = Color.Transparent;
				return this.Items.Add(host);
			}

			public void Show(Control control)
			{
				Rectangle area = control.ClientRectangle;
				Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
				location = control.PointToClient(location);
				Show(control, location, ToolStripDropDownDirection.BelowRight);
				this.Focus();
			}
		}
	}
}