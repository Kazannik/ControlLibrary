using ControlLibrary.Controls.TextControl.TextEditor.Gui.CompletionWindow;
using ControlLibrary.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlLibrary.Controls.RatingControls
{
	//[DesignerCategory("code")]
	[ToolboxBitmap(typeof(ComboBox))]
	[ComVisible(false)]
	public class RatingControl : Control
	{
		private IContainer components = null;


		public RatingControl()
		{
			InitializeComponent();
		}

		public RatingControl(IContainer container)
		{
			InitializeComponent();

			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Utils.Drawing.DrawStar(e.Graphics, Color.Black, Color.Cyan, 100, e.ClipRectangle);


		}



		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.SuspendLayout();
			// 
			// Controls
			// 
			this.ResumeLayout(false);

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
