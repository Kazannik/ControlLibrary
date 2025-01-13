using ControlLibrary.Controls.TextControl.TextEditor.Util;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.TextControl.TextEditor.Gui.CompletionWindow
{
	public interface IDeclarationViewWindow
	{
		string Description
		{
			get;
			set;
		}
		void ShowDeclarationViewWindow();
		void CloseDeclarationViewWindow();
	}

	public class DeclarationViewWindow : Form, IDeclarationViewWindow
	{
		private string description = String.Empty;
		private bool fixedWidth;

		public string Description
		{
			get => description;
			set
			{
				description = value;
				if (value == null && Visible)
				{
					Visible = false;
				}
				else if (value != null)
				{
					if (!Visible) ShowDeclarationViewWindow();
					Refresh();
				}
			}
		}

		public bool FixedWidth
		{
			get => fixedWidth;
			set => fixedWidth = value;
		}

		public int GetRequiredLeftHandSideWidth(Point p)
		{
			if (description != null && description.Length > 0)
			{
				using (Graphics g = CreateGraphics())
				{
					Size s = TipPainterTools.GetLeftHandSideDrawingSizeHelpTipFromCombinedDescription(this, g, Font, null, description, p);
					return s.Width;
				}
			}
			return 0;
		}

		public bool HideOnClick;

		public DeclarationViewWindow(Form parent)
		{
			SetStyle(ControlStyles.Selectable, false);
			StartPosition = FormStartPosition.Manual;
			FormBorderStyle = FormBorderStyle.None;
			Owner = parent;
			ShowInTaskbar = false;
			Size = new Size(0, 0);
			base.CreateHandle();
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams p = base.CreateParams;
				AbstractCompletionWindow.AddShadowToWindow(p);
				return p;
			}
		}

		protected override bool ShowWithoutActivation => true;

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if (HideOnClick) Hide();
		}

		public void ShowDeclarationViewWindow()
		{
			Show();
		}

		public void CloseDeclarationViewWindow()
		{
			Close();
			Dispose();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			if (description != null && description.Length > 0)
			{
				if (fixedWidth)
				{
					TipPainterTools.DrawFixedWidthHelpTipFromCombinedDescription(this, pe.Graphics, Font, null, description);
				}
				else
				{
					TipPainterTools.DrawHelpTipFromCombinedDescription(this, pe.Graphics, Font, null, description);
				}
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pe)
		{
			pe.Graphics.FillRectangle(SystemBrushes.Info, pe.ClipRectangle);
		}
	}
}
