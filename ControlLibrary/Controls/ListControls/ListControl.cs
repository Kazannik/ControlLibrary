using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ControlLibrary.Controls.ListControls
{
	[ToolboxItem(false)]
	[DesignerCategory("code")]
	[DesignTimeVisible(true)]
	[ToolboxBitmap(typeof(ListBox))]
	[ComVisible(false)]
	public abstract class ListControl<I, S> : ListBox
		where I : IListItem, new()
		where S : IListItemNote
	{
		private Timer resizeTimer;
		private Size oldSize;

		private BufferedGraphicsContext context;
		private BufferedGraphics grafx;

		protected override ObjectCollection CreateItemCollection()
		{
			ItemCollection<I, S> collection = new ItemCollection<I, S>(this);
			collection.ClipSizeChanged += new EventHandler<ItemEventArgs<I>>(Collection_ClipSizeChanged);
			collection.ContentChanged += new EventHandler<ItemEventArgs<I>>(Collection_ContentChanged);
			collection.SizeChanged += new EventHandler<EventArgs>(Collection_SizeChanged);
			return collection;
		}

		private void Collection_SizeChanged(object sender, EventArgs e)
		{
			base.RefreshItems();
		}

		private void Collection_ClipSizeChanged(object sender, ItemEventArgs<I> e)
		{
			base.RefreshItems();
		}

		private void Collection_ContentChanged(object sender, ItemEventArgs<I> e)
		{
			base.Invalidate(e.Item.Bounds);
			OnItemContentChanged(e);
		}

		private void ResizeTimer_Tick(object sender, EventArgs e)
		{
			if (Size != oldSize)
			{
				oldSize = Size;
			}
			else
			{
				resizeTimer.Stop();
				base.RefreshItems();
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (!DesignMode && Size != oldSize)
				resizeTimer.Start();
		}

		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);

			if (!DesignMode
				&& e.Index >= 0
				&& e.Index < Items.Count)
			{
				I item = this[e.Index];

				item.MeasureBound(e.Graphics, Font, ClientSize.Width, ClientSize.Height);

				e.ItemHeight = item.Size.Height;
				e.ItemWidth = item.Size.Width;
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Bounds.Height > 0)
			{
				grafx = context.Allocate(e.Graphics, e.Bounds);
				DrawItemEventArgs args = new DrawItemEventArgs(grafx.Graphics, e.Font, e.Bounds, e.Index, e.State, e.ForeColor, e.BackColor);
				//args.DrawBackground();
				if (DesignMode || args.Index < 0)
				{
					const TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.Top;
					TextRenderer.DrawText(args.Graphics, GetType().Name, args.Font, ClientRectangle, args.ForeColor, flags);
				}
				else 
				{
					this[args.Index].Draw(args);
					args.DrawFocusRectangle();
				}
				grafx.Render(e.Graphics);
			}
			base.OnDrawItem(e);
		}

		public new ItemCollection<I, S> Items => (ItemCollection<I, S>)base.Items;

		public I this[int index] => (I)base.Items[index];

		#region Click Event

		[Category("Action"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemEventArgs<I>> SelectedItemChanged;
		[Category("Action"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemEventArgs<I>> ItemContentChanged;

		[Category("Mouse"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemMouseEventArgs<I, S>> ItemMouseDown;
		[Category("Mouse"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemMouseEventArgs<I, S>> ItemMouseUp;
		[Category("Action"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemMouseEventArgs<I, S>> ItemMouseClick;
		[Category("Action"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemMouseEventArgs<I, S>> ItemMouseDoubleClick;
		[Category("Mouse"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemMouseEventArgs<I, S>> ItemMouseMove;

		protected virtual void OnSelectedItemChanged(ItemEventArgs<I> e)
		{
			SelectedItemChanged?.Invoke(this, e);
		}

		protected virtual void OnItemContentChanged(ItemEventArgs<I> e)
		{
			ItemContentChanged?.Invoke(this, e);
		}

		protected virtual void OnItemMouseDown(ItemMouseEventArgs<I, S> e)
		{
			ItemMouseDown?.Invoke(this, e);
		}

		protected virtual void OnItemMouseUp(ItemMouseEventArgs<I, S> e)
		{
			ItemMouseUp?.Invoke(this, e);
		}

		protected virtual void OnItemMouseClick(ItemMouseEventArgs<I, S> e)
		{
			ItemMouseClick?.Invoke(this, e);
		}

		protected virtual void OnItemMouseDoubleClick(ItemMouseEventArgs<I, S> e)
		{
			ItemMouseDoubleClick?.Invoke(this, e);
		}
		
		protected virtual void OnItemMouseMove(ItemMouseEventArgs<I, S> e)
		{
			ItemMouseMove?.Invoke(this, e);
		}

		#endregion

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			if (SelectedIndex >=0)
			{
				I item = this[SelectedIndex];
				OnSelectedItemChanged(new ItemEventArgs<I>(item, null));
			}
			else
			{
				OnSelectedItemChanged(new ItemEventArgs<I>(default, null));
			}
			base.OnSelectedIndexChanged(e);
		}

		#region Mouse
				
		private I GetListItem(Point location)
		{
			for (int i = 0; i < Items.Count; i++)
			{
				if (GetItemRectangle(i).Contains(location))
				{
					return this[i];
				}
			}
			return default;
		}

		private S GetListItemNote(I item, Point location)
		{
			for (int i = 0; i < item.Count; i++)
			{
				if (item.GetSubitemRectangle(i).Contains(location))
				{
					return (S)item[i];
				}
			}
			return default;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			I item = GetListItem(e.Location);
			if (item != null)
			{
				S subitem = GetListItemNote(item, e.Location);
				OnItemMouseDown(new ItemMouseEventArgs<I, S>(item, subitem, null, e));
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			I item = GetListItem(e.Location);
			if (item != null)
			{
				S subitem = GetListItemNote(item, e.Location);
				OnItemMouseClick(new ItemMouseEventArgs<I, S>(item, subitem, null, e));
			}
			base.OnMouseClick(e);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			I item = GetListItem(e.Location);
			if (item != null)
			{
				S subitem = GetListItemNote(item, e.Location);
				OnItemMouseDoubleClick(new ItemMouseEventArgs<I, S>(item, subitem, null, e));
			}
			base.OnMouseDoubleClick(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			I item = GetListItem(e.Location);
			if (item != null)
			{
				S subitem = GetListItemNote(item, e.Location);
				OnItemMouseUp(new ItemMouseEventArgs<I, S>(item, subitem, null, e));
			}
			base.OnMouseUp(e);
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			I item = GetListItem(e.Location);
			if (item != null)
			{
				S subitem = GetListItemNote(item, e.Location);
				OnItemMouseMove(new ItemMouseEventArgs<I, S>(item, subitem, null, e));
			}
			base.OnMouseMove(e);
		}
		
		#endregion

		public ListControl()
		{
			oldSize = Size.Empty;
			context = BufferedGraphicsManager.Current;
			InitializeComponent();
		}

		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
			SuspendLayout();
			// 
			// Timer
			// 
			resizeTimer = new Timer
			{
				Interval = 20,
				Enabled = false
			};
			resizeTimer.Tick += new EventHandler(ResizeTimer_Tick);
			// 
			// ListControl
			// 
			base.ScrollAlwaysVisible = true;
			base.DrawMode = DrawMode.OwnerDrawVariable;
			base.ClientSizeChanged += new EventHandler(ListControl_ClientSizeChanged);
			ResumeLayout(false);
		}

		private void ListControl_ClientSizeChanged(object sender, EventArgs e)
		{
			context.MaximumBuffer = new Size(ClientSize.Width + 1, ClientSize.Height + 1);
			if (grafx != null)
			{
				grafx.Dispose();
				grafx = null;
			}
			grafx = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
		}

		[ReadOnly(true)]
		public new DrawMode DrawMode => base.DrawMode;

		[ReadOnly(true)]
		public new bool ScrollAlwaysVisible => base.ScrollAlwaysVisible;

		[ReadOnly(true)]
		public new int ItemHeight => base.ItemHeight;		
	}
	
	public class ItemMouseEventArgs<I, S> : MouseEventArgs where I : IListItem, new() where S : IListItemNote
	{
		public ItemMouseEventArgs(I item, S subItem, object argument, MouseEventArgs eventArgs) : base(eventArgs.Button, eventArgs.Clicks, eventArgs.X, eventArgs.Y, eventArgs.Delta)
		{
			Item = item;
			SubItem = subItem;
			Argument = argument;
		}

		public I Item { get; }

		public S SubItem { get; }

		public object Argument { get; }
	}
}
