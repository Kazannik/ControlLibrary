using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ControlLibrary.Controls.ToolStripControls
{
	[DesignerCategory("code")]
	[ToolboxBitmap(typeof(NumericUpDown))]
	[ComVisible(false)]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
	public class ToolStripNumericUpDown : ToolStripControlHost
	{
		public ToolStripNumericUpDown() : base(new NumericUpDown())
		{
			NumericUpDownControl.ValueChanged += new EventHandler(NumericUpDown_ValueChanged);
		}

		public NumericUpDown NumericUpDownControl
		{
			get { return (NumericUpDown)Control; }
		}

		#region ValueChanged

		public decimal Value
		{
			get { return NumericUpDownControl.Value; }
			set { NumericUpDownControl.Value = value; }
		}

		public event EventHandler ValueChanged;

		public void DoValueChanged()
		{
			OnValueChanged(new EventArgs());
		}

		protected virtual void OnValueChanged(EventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}

		private void NumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			DoValueChanged();
		}

		#endregion
	}
}
