using ControlLibrary.Structures;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.PriodControls
{
	public partial class PeriodPickerPopupForm : UserControl
	{
		public PeriodPickerPopupForm()
		{
			InitializeComponent();
		}

		private void PeriodPickerPopupForm_Resize(object sender, EventArgs e)
		{
			PopupMenu_PeriodBox.Location = new Point(0, 0);
			Size = PopupMenu_PeriodBox.Size;
		}

		#region Value

		public Period Value
		{
			get => PopupMenu_PeriodBox.Value;
			set => PopupMenu_PeriodBox.Value = value;
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

		#region Click

		private void PeriodBox_ButtonClick(object sender, EventArgs e)
		{
			OnButtonClick(new EventArgs());
		}

		public event EventHandler ButtonClick;

		protected virtual void OnButtonClick(EventArgs e)
		{
			ButtonClick?.Invoke(this, e);
		}

		#endregion
	}
}
