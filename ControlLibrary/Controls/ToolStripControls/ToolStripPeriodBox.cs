using ControlLibrary.Controls.PriodControls;
using ControlLibrary.Structures;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ControlLibrary.Controls.ToolStripControls
{
	[DesignerCategory("Code")]
	[ToolboxBitmap(typeof(ComboBox))]
	[ComVisible(false)]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
	public class ToolStripPeriodBox : ToolStripControlHost
	{
		public ToolStripPeriodBox() : base(new PeriodPicker())
		{
			PeriodPickerControl.ValueChanged += new EventHandler<PeriodEventArgs>(Period_ValueChanged);
		}

		public PeriodPicker PeriodPickerControl => (PeriodPicker)Control;

		#region ValueChanged

		public Period Value
		{
			get => PeriodPickerControl.Value;
			set => PeriodPickerControl.Value = value;
		}

		public event EventHandler ValueChanged;

		public void DoValueChanged() => OnValueChanged(new PeriodEventArgs(Value));

		protected virtual void OnValueChanged(EventArgs e) => ValueChanged?.Invoke(this, e);

		private void Period_ValueChanged(object sender, PeriodEventArgs e) => DoValueChanged();

		#endregion
	}
}
