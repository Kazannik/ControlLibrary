using ControlLibrary.Structures;
using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ControlLibrary.Controls.PriodControls
{
	[System.ComponentModel.DesignerCategory("Form")]
	[System.Drawing.ToolboxBitmap(typeof(ComboBox))]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	public class PeriodPicker : ComboBox
	{
		private readonly ToolStripControlHost controlHost;
		private readonly PeriodPickerPopupForm popupForm;
		private static ToolStripDropDown dropDown;

		public PeriodPicker()
		{
			popupForm = new PeriodPickerPopupForm
			{
				BorderStyle = BorderStyle.None
			};
			controlHost = new ToolStripControlHost(popupForm);

			popupForm.ValueChanged += new EventHandler<PeriodEventArgs>(this.PopupMenu_PeriodBox_ValueChanged);

			dropDown = new ToolStripDropDown();
			dropDown.Items.Add(controlHost);
			DropDownHeight = popupForm.Height;
			DropDownWidth = popupForm.Width;
			controlHost.Width = DropDownWidth;
			controlHost.Height = DropDownHeight;
		}

		#region Value

		public Period Value
		{
			get { return popupForm.Value; }
			set { popupForm.Value = value; }
		}

		private void PopupMenu_PeriodBox_ValueChanged(object sender, PeriodEventArgs e)
		{
			OnValueChanged(new PeriodEventArgs(e.Period));
		}

		public event EventHandler<PeriodEventArgs> ValueChanged;

		protected virtual void OnValueChanged(PeriodEventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}

		#endregion


		//Public ReadOnly Property PopupForm() As MonthPopupForm
		//    Get
		//        Return CType(controlHost.Control, MonthPopupForm)
		//    End Get
		//End Property

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
