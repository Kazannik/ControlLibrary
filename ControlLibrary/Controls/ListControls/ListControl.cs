using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ControlLibrary.Controls.ListControls
{
	[DesignerCategory("code")]
	[ToolboxBitmap(typeof(ListBox))]
	[ComVisible(false)]
	public abstract class ListControl<I, S> : ListBox
		where I : IListItem, new()
		where S : IListItemNote
	{
		private const TextFormatFlags DEFAULT_ALIGNMENT_FLAG = TextFormatFlags.Left | TextFormatFlags.Top;

		private I firstViewItem;
		private int firstViewItemIndex;

		private Timer resizeTimer;
		private Size oldSize;

		private readonly BufferedGraphicsContext context;

		#region InitializeObjectCollection

		protected override ObjectCollection CreateItemCollection()
		{
			ItemCollection<I, S> collection = new ItemCollection<I, S>(this);
			collection.ClipSizeChanged += new EventHandler<ItemEventArgs<I>>(Collection_ClipSizeChanged);
			collection.ContentChanged += new EventHandler<ItemEventArgs<I>>(Collection_ContentChanged);
			collection.SizeChanged += new EventHandler<EventArgs>(Collection_SizeChanged);
			collection.ItemAdded += new EventHandler<ItemEventArgs<I>>(Collection_ItemAdded);
			collection.ItemDeleted += new EventHandler<EventArgs>(Collection_ItemDeleted);
			return collection;
		}

		private void Collection_ItemDeleted(object sender, EventArgs e) => OnItemDeleted(e);

		private void Collection_ItemAdded(object sender, ItemEventArgs<I> e) => OnItemAdded(e);

		private void Collection_SizeChanged(object sender, EventArgs e) => base.RefreshItems();

		private void Collection_ClipSizeChanged(object sender, ItemEventArgs<I> e) => base.RefreshItems();

		private void Collection_ContentChanged(object sender, ItemEventArgs<I> e) => OnItemContentChanged(e);

		#endregion

		private void ResizeTimer_Tick(object sender, EventArgs e)
		{
			if (Size != oldSize)
			{
				Size = oldSize;
				resizeTimer.Stop();
				int topIndex = TopIndex;
				int selectedIndex = SelectedIndex;
				base.RefreshItems();
				if (selectedIndex>=0) 
					SelectedIndex = selectedIndex;
				TopIndex = topIndex;
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
			if (!DesignMode &&
				e.Index >= 0 &&
				e.Index < Items.Count)
			{
				I item = this[e.Index];

				item.MeasureBound(e.Graphics, Font, ClientSize.Width, ClientSize.Height);

				e.ItemHeight = item.Size.Height;
				e.ItemWidth = item.Size.Width;
			}
			base.OnMeasureItem(e);
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Bounds.Height > 0 && e.Bounds.Width > 0)
			{
				BufferedGraphics grafx = context.Allocate(e.Graphics, e.Bounds);
				DrawItemEventArgs args = new DrawItemEventArgs(grafx.Graphics, e.Font, e.Bounds, e.Index, e.State, e.ForeColor, e.BackColor);

				Utils.Drawing.SetGraphicsStyle(args.Graphics);

				args.DrawBackground();

				if (DesignMode || args.Index < 0)
				{
					TextRenderer.DrawText(args.Graphics, GetType().Name, args.Font, e.Bounds, args.ForeColor, DEFAULT_ALIGNMENT_FLAG);
				}
				else
				{
					if (args.Bounds.Y == 0)
					{
						firstViewItemIndex = args.Index;
						FirstViewItem = Items[args.Index];
					}
					this[args.Index].Draw(args);					
				}

				args.DrawFocusRectangle();
				grafx.Render(e.Graphics);
			}
			base.OnDrawItem(e);
		}

		public new ItemCollection<I, S> Items => (ItemCollection<I, S>)base.Items;

		public I this[int index] => (I)base.Items[index];

		[Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new I SelectedItem
		{
			get => (I)base.SelectedItem;
			set => base.SelectedItem = value;
		}


		[Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public I FirstViewItem
		{
			get => firstViewItem;
			private set
			{
				if (!Equals(firstViewItem, value))
				{
					firstViewItem = value;
					OnFirstViewItemChanged(new ItemEventArgs<I>(firstViewItem, null));
				}
			}
		}

		#region Events

		[Category("Action"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemEventArgs<I>> FirstViewItemChanged;
		[Category("Action"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemEventArgs<I>> SelectedItemChanged;
		[Category("Action"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemEventArgs<I>> ItemContentChanged;
		[Category("Action"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<ItemEventArgs<I>> ItemAdded;
		[Category("Action"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public event EventHandler<EventArgs> ItemDeleted;

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

		protected virtual void OnFirstViewItemChanged(ItemEventArgs<I> e) => FirstViewItemChanged?.Invoke(this, e);

		protected virtual void OnSelectedItemChanged(ItemEventArgs<I> e) => SelectedItemChanged?.Invoke(this, e);

		protected virtual void OnItemContentChanged(ItemEventArgs<I> e) => ItemContentChanged?.Invoke(this, e);

		protected virtual void OnItemAdded(ItemEventArgs<I> e) => ItemAdded?.Invoke(this, e);

		protected virtual void OnItemDeleted(EventArgs e) => ItemDeleted?.Invoke(this, e);

		protected virtual void OnItemMouseDown(ItemMouseEventArgs<I, S> e) => ItemMouseDown?.Invoke(this, e);

		protected virtual void OnItemMouseUp(ItemMouseEventArgs<I, S> e) => ItemMouseUp?.Invoke(this, e);

		protected virtual void OnItemMouseClick(ItemMouseEventArgs<I, S> e) => ItemMouseClick?.Invoke(this, e);

		protected virtual void OnItemMouseDoubleClick(ItemMouseEventArgs<I, S> e) => ItemMouseDoubleClick?.Invoke(this, e);

		protected virtual void OnItemMouseMove(ItemMouseEventArgs<I, S> e) => ItemMouseMove?.Invoke(this, e);

		#endregion

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			if (SelectedIndex >= 0)
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
		
		private const int WM_MOUSEWHEEL = 0x020A;

		public static int HIWORD(int n) => (n >> 16);

		public static int HIWORD(IntPtr n) => HIWORD(unchecked((int)(long)n));

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_MOUSEWHEEL)
			{
				int delta = HIWORD(m.WParam);
				if (delta > 0)
					TopIndex--;
				else if (delta < 0)
					TopIndex++;
				return;
			}
			base.WndProc(ref m);			
		}

		#endregion

		#region Initialize

		[DebuggerNonUserCode()]
		public ListControl(IContainer container) : this() => container?.Add(this);

		[DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
					components.Dispose();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		private IContainer components;

		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
			components = new Container();

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
			base.DrawMode = DrawMode.OwnerDrawVariable;
			base.ScrollAlwaysVisible = true;
			ClientSizeChanged += new EventHandler(ListControl_ClientSizeChanged);
			ResumeLayout(false);
		}

		public ListControl() : base()
		{
			oldSize = Size.Empty;
			context = BufferedGraphicsManager.Current;
			InitializeComponent();
		}

		#endregion

		private void ListControl_ClientSizeChanged(object sender, EventArgs e)
		{
			if (ClientSize.Width > 0 && ClientSize.Height > 0)
				context.MaximumBuffer = new Size(ClientSize.Width + 1, ClientSize.Height + 1);
		}

		[ReadOnly(true)]
		[Browsable(false)]
		public new DrawMode DrawMode => base.DrawMode;

		[ReadOnly(true)]
		[Browsable(false)]
		public new bool ScrollAlwaysVisible => base.ScrollAlwaysVisible;

		[ReadOnly(true)]
		[Browsable(false)]
		public new int ItemHeight => base.ItemHeight;
	}
}
