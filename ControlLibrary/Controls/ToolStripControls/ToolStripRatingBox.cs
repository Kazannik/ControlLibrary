using ControlLibrary.Controls.RatingControls;
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
	public class ToolStripRatingBox : ToolStripControlHost
	{
		public ToolStripRatingBox() : base(new RatingControl())
		{
			RatingControl.RatingChanged += new EventHandler<RatingEventArgs>(Control_RatingChanged);
		}

		public RatingControl RatingControl => (RatingControl)Control;

		#region ValueChanged

		public Rating Rating
		{
			get => RatingControl.Rating;
			set => RatingControl.Rating = value;
		}

		public Color StarsColor
		{
			get => RatingControl.StarsColor1;
			set => RatingControl.StarsColor1 = value;
		}

		public event EventHandler RatingChanged;

		public void DoRatingChanged() => OnRatingChanged(new RatingEventArgs(Rating));

		protected virtual void OnRatingChanged(EventArgs e) => RatingChanged?.Invoke(this, e);

		private void Control_RatingChanged(object sender, RatingEventArgs e) => DoRatingChanged();

		#endregion
	}
}
