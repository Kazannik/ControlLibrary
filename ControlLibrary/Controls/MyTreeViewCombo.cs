using ControlLibrary.Controls.PriodControls;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ControlLibrary.Controls
{
	public class MyTreeViewCombo : ComboBox
	{
		private readonly ToolStripControlHost treeViewHost;
		private ToolStripDropDown dropDown;
		private readonly ToolStripDropDownButton dropDownButton;


		public MyTreeViewCombo()
		{
			PeriodBox treeView = new PeriodBox();
			//{
			//	//BorderStyle = BorderStyle.None
			//};
			treeViewHost = new ToolStripControlHost(treeView);
			dropDown = new ToolStripDropDown();
			dropDown.Items.Add(treeViewHost);
			dropDown.ItemClicked += DropDown_ItemClicked;
			dropDown.MouseClick += DropDown_MouseClick;
			dropDown.MouseDown += DropDown_MouseDown;
			dropDown.MouseUp += DropDown_MouseUp;


			dropDownButton = new ToolStripDropDownButton("Text", Properties.Resources.DownArrow2, DropDownButton_Click);
		}

		private void DropDown_MouseUp(object sender, MouseEventArgs e)
		{
			Debug.WriteLine(DateTime.Now + " DropDown_MouseUp " + e.Button);
		}

		private void DropDown_MouseDown(object sender, MouseEventArgs e)
		{
			Debug.WriteLine(DateTime.Now + " DropDown_MouseDown " + e.Button);
		}

		private void DropDown_MouseClick(object sender, MouseEventArgs e)
		{
			Debug.WriteLine(DateTime.Now + " DropDown_MouseClick " + e.Button);
		}

		private void DropDown_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			Debug.WriteLine(DateTime.Now + " DropDown_ItemClicked " + e.ClickedItem);
		}

		public TreeView TreeView
		{
			get { return treeViewHost.Control as TreeView; }
		}

		private void DropDownButton_Click(object sender, EventArgs e)
		{
			ShowDropDown();
		}

		private void ShowDropDown()
		{
			if (dropDown != null)
			{
				treeViewHost.Width = DropDownWidth;
				treeViewHost.Height = DropDownHeight;
				dropDown.Show(this, 0, this.Height);
			}
		}

		private const int WM_USER = 0x0400,
						  WM_REFLECT = WM_USER + 0x1C00,
						  WM_COMMAND = 0x0111,
						  CBN_DROPDOWN = 7,
						  CBN_CLOSEUP = 8,
			CBN_DBLCLK = 2,
			CBN_EDITCHANGE = 5,
			CBN_EDITUPDATE = 6,
			CBN_ERRSPACE = -1,
			CBN_KILLFOCUS = 4,
			CBN_SELCHANGE = 1,
			CBN_SELENDCANCEL = 10,
			CBN_SELENDOK = 9,
			CBN_SETFOCUS = 3;

		public static int HIWORD(int n) => (n >> 16) & 0xffff;

		public static int HIWORD(IntPtr n) => HIWORD(unchecked((int)(long)n));

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == (WM_REFLECT + WM_COMMAND))
			{
				int cbn = HIWORD(m.WParam);
				switch (cbn)
				{
					case CBN_SETFOCUS:
						Debug.WriteLine(DateTime.Now + " CBN_SETFOCUS");
						break;
					case CBN_KILLFOCUS:
						Debug.WriteLine(DateTime.Now + " CBN_KILLFOCUS");
						break;
					case CBN_DROPDOWN:
						ShowDropDown();
						Debug.WriteLine(DateTime.Now + " CBN_DROPDOWN");
						return;
					//break;
					case CBN_CLOSEUP:
						Debug.WriteLine(DateTime.Now + " CBN_CLOSEUP");
						break;
					case CBN_SELENDCANCEL:
						Debug.WriteLine(DateTime.Now + " CBN_SELENDCANCEL");
						break;
					default:
						Debug.WriteLine(DateTime.Now + " Other: " + HIWORD((int)m.WParam));
						break;
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

		private static class WndUI
		{
			public const int
				WM_USER = 0x0400,
				WM_REFLECT = WM_USER + 0x1C00,
				WM_COMMAND = 0x0111,
				CBN_DROPDOWN = 7;

			public static int HIWORD(IntPtr n)
			{
				return HIWORD(unchecked((int)(long)n));
			}

			public static int HIWORD(int n)
			{
				return (n >> 16) & 0xffff;
			}
		}
	}
}