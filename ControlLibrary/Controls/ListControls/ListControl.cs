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
	[ToolboxBitmap(typeof(ListBox))]
	[ComVisible(false)]
	public abstract class ListControl<T>: ListBox where T : IListItem, new()
	{
		private Timer resizeTimer;
		private Size oldSize;
		private Color ratingStarColor;

		protected override ObjectCollection CreateItemCollection()
		{
			ItemCollection<T> collection = new ItemCollection<T>(this);
			collection.ContentChanged += new EventHandler<EventArgs>(Collection_ContentChanged);
			collection.CountChanged += new EventHandler<EventArgs>(Collection_CountChanged);
			return collection;
		}

		private void Collection_CountChanged(object sender, EventArgs e)
		{

		}

		private void Collection_ContentChanged(object sender, EventArgs e)
		{
			base.RefreshItems();
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
				T item = this[e.Index];
				
				item.MeasureBound(e.Graphics, Font, ClientSize.Width, ClientSize.Height);

				e.ItemHeight = item.Size.Height;
				e.ItemWidth = item.Size.Width;
			}
		}

		protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
		{
			base.OnDrawItem(e);
			e.DrawBackground();
			if (DesignMode)
			{
				const TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.Top;
				TextRenderer.DrawText(e.Graphics, GetType().Name, e.Font, ClientRectangle, e.ForeColor, flags);
			}
			else if (e.Index >= 0)
			{
				this[e.Index].Draw(new DrawItemEventArgs(e, RatingStarColor));
				e.DrawFocusRectangle();
			}
		}
				
		public new ItemCollection<T> Items
		{
			get { return (ItemCollection<T>)base.Items; }
		}

		public T this[int index]
		{
			get { return (T)base.Items[index]; }
		}

		#region Click Event

		public event EventHandler<ItemMouseEventArgs> ItemMouseDown;
		public event EventHandler<ItemMouseEventArgs> ItemMouseUp;
		public event EventHandler<ItemMouseEventArgs> ItemMouseClick;
		public event EventHandler<ItemMouseEventArgs> ItemMouseDoubleClick;

		public event EventHandler<ItemMouseEventArgs> ItemApplyClick;
		public event EventHandler<ItemMouseEventArgs> ItemCancelClick;


		protected virtual void OnItemMouseDown(ItemMouseEventArgs e)
		{
			ItemMouseDown?.Invoke(this, e);
		}

		protected virtual void OnItemMouseUp(ItemMouseEventArgs e)
		{
			ItemMouseUp?.Invoke(this, e);
		}

		protected virtual void OnItemMouseClick(ItemMouseEventArgs e)
		{
			ItemMouseClick?.Invoke(this, e);
		}

		protected virtual void OnItemMouseDoubleClick(ItemMouseEventArgs e)
		{
			ItemMouseDoubleClick?.Invoke(this, e);
		}

		protected virtual void OnItemApplyClick(ItemMouseEventArgs e)
		{
			ItemApplyClick?.Invoke(this, e);
		}

		protected virtual void OnItemCancelClick(ItemMouseEventArgs e)
		{
			ItemCancelClick?.Invoke(this, e);
		}

		#endregion

		#region Mouse

		private T GetListItem(Point location)
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

		protected override void OnMouseDown(MouseEventArgs e)
		{
			T item = GetListItem(e.Location);
			if (item != null)
			{
				OnItemMouseDown(new ItemMouseEventArgs(item, e));
			}			
			base.OnMouseDown(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			T item = GetListItem(e.Location);
			if (item != null)
			{
				OnItemMouseClick(new ItemMouseEventArgs(item, e));
				if (item.ApplyButton.Contains(e.Location))
				{
					OnItemApplyClick(new ItemMouseEventArgs(item, e));
				}
				else if (item.CancelButton.Contains(e.Location))
				{
					OnItemCancelClick(new ItemMouseEventArgs(item, e));
				}
			}
			base.OnMouseClick(e);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			T item = GetListItem(e.Location);
			if (item != null)
			{
				OnItemMouseDoubleClick(new ItemMouseEventArgs(item, e));
			}
			base.OnMouseDoubleClick(e);
		}
		
		protected override void OnMouseUp(MouseEventArgs e)
		{
			T item = GetListItem(e.Location);
			if (item != null)
			{
				OnItemMouseUp(new ItemMouseEventArgs(item, e));
			}
			base.OnMouseUp(e);
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			T item = GetListItem(e.Location);
			if (item != null)
			{
				if (item.ApplyButton.Contains(e.Location) 
					|| item.CancelButton.Contains(e.Location))
				{
					Cursor = Cursors.Hand;
				}
				else
				{
					Cursor= Cursors.Default;
				}
			}
			base.OnMouseMove(e);
		}

		#endregion

		public class ItemMouseEventArgs : MouseEventArgs
		{
			public ItemMouseEventArgs(T item, MouseEventArgs arg) : base(arg.Button, arg.Clicks, arg.X, arg.Y, arg.Delta)
			{
				Item = item;
			}

			public T Item { get; }
		}

		public ListControl()
		{
			oldSize = Size.Empty;
			ratingStarColor = SystemColors.Info;
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
			base.RefreshItems();
		}

		[ReadOnly(true)]
		public new DrawMode DrawMode { get => base.DrawMode; }

		[ReadOnly(true)]
		public new bool ScrollAlwaysVisible { get => base.ScrollAlwaysVisible;}

		[ReadOnly(true)]
		public new int ItemHeight { get => base.ItemHeight; }

		public Color RatingStarColor
		{
			get
			{
				return ratingStarColor;
			}
			set
			{
				if (ratingStarColor != value)
				{
					ratingStarColor = value;
					base.RefreshItems();
				}
			}
		}		
	}
}
