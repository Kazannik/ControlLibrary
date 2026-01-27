// Ignore Spelling: Subitem

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Font = System.Drawing.Font;

namespace ControlLibrary.Controls.ListControls
{

	public class ListItem : IListItem
	{		
		private static readonly StringFormat LEFT_STRING_FORMAT = new StringFormat
		{
			Alignment = StringAlignment.Near,
			LineAlignment = StringAlignment.Near
		};

		protected readonly IListItemNote[] notes;

		public ListItem() : this(text: typeof(ListItem).Name) { }

		public ListItem(string text) :this(note: new Note(text: text))
		{
			Bounds = Rectangle.Empty;
			Size = Size.Empty;
			notes[0] = new Note(text);
		}

		public ListItem(IListItemNote note): this(new IListItemNote[] {note}) { }

		public ListItem(IListItemNote[] notes)
		{
			Bounds = Rectangle.Empty;
			this.notes = notes;

			for (int i = 0; i < notes.Length; i++)
			{
				this.notes[i].ClipSizeChanged += new System.EventHandler<System.EventArgs>(ListItem_ClipSizeChanged);
				this.notes[i].ContentChanged += new System.EventHandler<System.EventArgs>(ListItem_ContentChanged);
			}
			Size = Size.Empty;
		}

		public Rectangle Bounds { get; protected set; }

		public Size Size { get; private set; }

		public Rectangle GetSubitemRectangle(int index)
		{
			if (index >= notes.Length) return Rectangle.Empty;

			int top = Bounds.Y;
			for (int i = 0; i < index; i++)
			{
				top += notes[i].Size.IsEmpty ? 0 : notes[i].Size.Height;
			}
			return new Rectangle(Bounds.X, top, notes[index].Size.Width, notes[index].Size.Height);
		}

		public IListItemNote this[int index] => notes[index]; 

		public int Count => notes.Length; 
				
		protected virtual Size OnMeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight)
		{
			int height = 0;
			for (int i = 0; i < notes.Length; i++)
			{
				notes[i].MeasureBound(graphics, font, itemWidth, 0);
				height += notes[i].Size.IsEmpty ? 0 : notes[i].Size.Height;
			}
			Size = new Size(itemWidth, height);
			return Size;
		}
				
		protected virtual void OnDraw(DrawItemEventArgs e)
		{
			Bounds = e.Bounds;
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			int top = e.Bounds.Y;
			for (int i = 0; i < notes.Length; i++)
			{
				if (!notes[i].Size.IsEmpty)
				{
					Rectangle rectangle = new Rectangle(e.Bounds.X, top,
						notes[i].Size.Width, notes[i].Size.Height);
					top = rectangle.Y + rectangle.Height;
					notes[i].Draw(new DrawItemEventArgs(e.Graphics, e.Font, rectangle, e.Index, e.State, e.ForeColor, e.BackColor));
#if DEBUG
					//e.Graphics.DrawRectangle(Pens.LimeGreen, new Rectangle(rectangle.X + 1, rectangle.Y, rectangle.Width - 2, rectangle.Height - 1));
#endif
				}
			}
#if DEBUG
			//e.Graphics.DrawRectangle(Pens.Green, new Rectangle(e.Bounds.X + 1, e.Bounds.Y, e.Bounds.Width - 2, e.Bounds.Height - 1));
#endif
		}

		public event System.EventHandler<System.EventArgs> ClipSizeChanged;
		public event System.EventHandler<System.EventArgs> ContentChanged;

		private void ListItem_ClipSizeChanged(object sender, System.EventArgs e) =>
			DoClipSizeChanged();
		
		private void DoClipSizeChanged() => 
			OnClipSizeChanged(new System.EventArgs());

		protected virtual void OnClipSizeChanged(System.EventArgs e) =>
			ClipSizeChanged?.Invoke(this, e);
		
		private void ListItem_ContentChanged(object sender, System.EventArgs e) =>
			DoContentChanged();

		private void DoContentChanged() => 
			OnContentChanged(new System.EventArgs());

		protected virtual void OnContentChanged(System.EventArgs e) => 
			ContentChanged?.Invoke(this, e);

		Size IListItem.MeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight) => 
			OnMeasureBound(graphics: graphics, font: font, itemWidth: itemWidth, itemHeight: itemHeight);

		void IListItem.Draw(DrawItemEventArgs e) =>
			OnDraw(e);

		IEnumerator<IListItemNote> IEnumerable<IListItemNote>.GetEnumerator() => 
			new ListItemNoteEnumerator(GetEnumerator());

		public IEnumerator GetEnumerator() => 
			notes.GetEnumerator();

		private class Note : ListItemNote, IListItemNote
		{			
			private string text;

			public Note(string text)
			{
				this.text = text;
			}
			
			public string Text
			{
				get => text;
				set
				{
					if (text != value)
					{
						text = value;
						DoContentChanged();
					}
				}
			}

			protected sealed override void OnDraw(DrawItemEventArgs e)
			{
				Brush brush = new SolidBrush(e.ForeColor);
				e.Graphics.DrawString(Text, e.Font, brush, e.Bounds, LEFT_STRING_FORMAT);
				brush.Dispose();
			}
			
			protected sealed override Size OnMeasureBound(Graphics graphics, Font font, int itemWidth, int itemHeight) => 
				GetTextSize(graphics: graphics, Text, font: font, width: itemWidth, LEFT_STRING_FORMAT);
		}

		private class ListItemNoteEnumerator : IEnumerator<IListItemNote>
		{
			private readonly IEnumerator _enumerator;

			internal ListItemNoteEnumerator(IEnumerator enumerator)
			{
				_enumerator = enumerator;
			}

			public IListItemNote Current => (IListItemNote)_enumerator.Current;

			object IEnumerator.Current => Current;

			public void Dispose() => _enumerator.Reset();

			public bool MoveNext() => _enumerator.MoveNext();

			public void Reset() => _enumerator.Reset();
		}
	}	
}
