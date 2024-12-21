using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExcelAnalyzer.Controls
{
	public partial class PeriodBox : UserControl
	{
		public PeriodBox()
		{
			InitializeComponent();
			BasePeriodBox.Location = new Point(0, 0);
			if (BorderStyle == BorderStyle.Fixed3D)
			{
				Height = BasePeriodBox.Height + SystemInformation.Border3DSize.Height * 2;
				Width = BasePeriodBox.Width + SystemInformation.Border3DSize.Width * 2;
			}
			else if (BorderStyle == BorderStyle.FixedSingle)
			{
				Height = BasePeriodBox.Height + SystemInformation.BorderSize.Height * 2;
				Width = BasePeriodBox.Width + SystemInformation.BorderSize.Width * 2;
			}
			else
			{
				Height = BasePeriodBox.Height;
				Width = BasePeriodBox.Width;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			BasePeriodBox.Location = new Point(0, 0);
			if (BorderStyle == BorderStyle.Fixed3D)
			{
				Height = BasePeriodBox.Height + SystemInformation.Border3DSize.Height * 2;
				Width = BasePeriodBox.Width + SystemInformation.Border3DSize.Width * 2;
			}
			else if (BorderStyle == BorderStyle.FixedSingle)
			{
				Height = BasePeriodBox.Height + SystemInformation.BorderSize.Height * 2;
				Width = BasePeriodBox.Width + SystemInformation.BorderSize.Width * 2;
			}
			else
			{
				Height = BasePeriodBox.Height;
				Width = BasePeriodBox.Width;
			}
		}
	}
}
