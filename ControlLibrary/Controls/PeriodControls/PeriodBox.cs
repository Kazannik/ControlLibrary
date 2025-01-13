using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.PriodControls
{
	public partial class PeriodBox : UserControl
	{
		public PeriodBox()
		{
			InitializeComponent();
			PeriodControl.Location = new Point(0, 0);
			if (BorderStyle == BorderStyle.Fixed3D)
			{
				Height = PeriodControl.Height + (SystemInformation.Border3DSize.Height * 2);
				Width = PeriodControl.Width + (SystemInformation.Border3DSize.Width * 2);
			}
			else if (BorderStyle == BorderStyle.FixedSingle)
			{
				Height = PeriodControl.Height + (SystemInformation.BorderSize.Height * 2);
				Width = PeriodControl.Width + (SystemInformation.BorderSize.Width * 2);
			}
			else
			{
				Height = PeriodControl.Height;
				Width = PeriodControl.Width;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			PeriodControl.Location = new Point(0, 0);
			if (BorderStyle == BorderStyle.Fixed3D)
			{
				Height = PeriodControl.Height + (SystemInformation.Border3DSize.Height * 2);
				Width = PeriodControl.Width + (SystemInformation.Border3DSize.Width * 2);
			}
			else if (BorderStyle == BorderStyle.FixedSingle)
			{
				Height = PeriodControl.Height + (SystemInformation.BorderSize.Height * 2);
				Width = PeriodControl.Width + (SystemInformation.BorderSize.Width * 2);
			}
			else
			{
				Height = PeriodControl.Height;
				Width = PeriodControl.Width;
			}
		}
	}
}
