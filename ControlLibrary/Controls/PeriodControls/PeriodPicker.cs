using ControlLibrary.Structures;
using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ControlLibrary.Controls.PriodControls
{
	[System.ComponentModel.DesignerCategory("Code")]
	[System.Drawing.ToolboxBitmap(typeof(ComboBox))]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	public class PeriodPicker : PickerControls.PickerControl<PeriodPickerPopupForm>
	{
		public PeriodPicker() : base()
		{
			PopupForm.ValueChanged += new EventHandler<PeriodEventArgs>(PopupMenu_PeriodBox_ValueChanged);
		}

		#region Value

		public Period Value
		{
			get => PopupForm.Value;
			set => PopupForm.Value = value;
		}

		private void PopupMenu_PeriodBox_ValueChanged(object sender, PeriodEventArgs e)
		{
			OnValueChanged(new PeriodEventArgs(e.Period));
		}

		public event EventHandler<PeriodEventArgs> ValueChanged;

		protected virtual void OnValueChanged(PeriodEventArgs e) => ValueChanged?.Invoke(this, e);

		#endregion
	}
}
