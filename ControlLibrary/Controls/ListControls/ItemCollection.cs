using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Windows.Forms.ListBox;

namespace ControlLibrary.Controls.ListControls
{
	public class ItemCollection<I, S> : ObjectCollection, IList<I>, ICollection<I>, IEnumerable<I> 
		where I : IListItem, new()
		where S : IListItemNote
		
	{
		public ItemCollection(ListControl<I, S> owner) : base(owner: owner) { }
		public ItemCollection(ListControl<I, S> owner, object[] value) : base(owner: owner, value: value) { }
		public ItemCollection(ListControl<I, S> owner, ObjectCollection value) : base(owner: owner, value: value) { }

		public new I this[int index]
		{
			get => (I)base[index];
			set => base[index] = value;
		}

		public new void Add(object value)
		{
			OnValidate(value);
			((I)value).ClipSizeChanged += new EventHandler<EventArgs>(ItemCollection_ClipSizeChanged);
			((I)value).ContentChanged += new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			base.Add(value);
			DoItemAdded(item: (I)value);
			DoSizeChanged();
		}
				
		public void Add(I item)
		{
			Add(value: item);
		}

		public new void AddRange(object[] value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				OnValidate(value[i]);
				I item = (I)value[i];
				item.ClipSizeChanged += new EventHandler<EventArgs>(ItemCollection_ClipSizeChanged);
				item.ContentChanged += new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			}
			base.AddRange(items: value);
			foreach (I item in value)
			{
				DoItemAdded(item: item);
			}
			DoSizeChanged();
		}

		public new void AddRange(ObjectCollection value)
		{
			for (int i = 0; i < value.Count; i++)
			{
				OnValidate(value[i]);
				I item = (I)value[i];
				item.ClipSizeChanged += new EventHandler<EventArgs>(ItemCollection_ClipSizeChanged);
				item.ContentChanged += new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			}
			base.AddRange(value: value);
			DoSizeChanged();
		}

		public void AddRange(ItemCollection<I, S> items)
		{
			AddRange(value: items);
		}

		public void AddRange(IList<I> items)
		{
			AddRange(value: (from I item in items select (object)item).ToArray());
		}

		public void AddRange(IList items)
		{
			AddRange(value: items.Cast<object>().ToArray());
		}

		public new int IndexOf(object value)
		{
			OnValidate(value);
			return base.IndexOf(value);
		}

		public int IndexOf(I item)
		{
			return IndexOf(value: item);
		}

		public new void Insert(int index, object value)
		{
			OnValidate(value: value);
			((I)value).ClipSizeChanged += new EventHandler<EventArgs>(ItemCollection_ClipSizeChanged);
			((I)value).ContentChanged += new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			Insert(index: index, item: value);
			DoItemAdded(item: (I)value);
			DoSizeChanged();
		}

		public void Insert(int index, I item)
		{
			Insert(index: index, value: item);
		}

		public new void Remove(object value)
		{
			OnValidate(value: value);
			((I)value).ClipSizeChanged -= new EventHandler<EventArgs>(ItemCollection_ClipSizeChanged);
			((I)value).ContentChanged -= new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			base.Remove(value: value);
			DoItemDeleted();
			DoSizeChanged();
		}

		public void Remove(I item)
		{
			Remove(value: item);
		}

		bool ICollection<I>.Remove(I item)
		{
			Remove(value: item);
			return !base.Contains(item);
		}

		public new void Clear()
		{
			ItemCollection<I, S> list = this;
			for (int i = 0; i < list.Count; i++)
			{
				I item = list[i];
				item.ClipSizeChanged -= new EventHandler<EventArgs>(ItemCollection_ClipSizeChanged);
				item.ContentChanged -= new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			}
			base.Clear();
			DoSizeChanged();
		}

		public new bool Contains(object value)
		{
			OnValidate(value: value);
			return base.Contains(value: value);
		}

		public bool Contains(I item)
		{
			return Contains(value: item);
		}

		protected void OnValidate(object value)
		{
			if (!typeof(I).IsAssignableFrom(value.GetType()))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Не удалось привести тип Value: {0} к поддурживаемому коллекцией типу: {1}.", value.GetType().ToString(), typeof(I).ToString()));
			}
		}


		public event EventHandler<ItemEventArgs<I>> ItemAdded;

		private void DoItemAdded(I item)
		{
			OnItemAdded(new ItemEventArgs<I>(item: item, argument: null));
		}

		protected virtual void OnItemAdded(ItemEventArgs<I> e)
		{
			ItemAdded?.Invoke(this, e);
		}


		public event EventHandler<EventArgs> ItemDeleted;

		private void DoItemDeleted()
		{
			OnItemDeleted(new EventArgs());
		}

		protected virtual void OnItemDeleted(EventArgs e)
		{
			ItemDeleted?.Invoke(this, e);
		}

		public event EventHandler<EventArgs> SizeChanged;

		private void DoSizeChanged()
		{
			OnSizeChanged(new EventArgs());
		}

		protected virtual void OnSizeChanged(EventArgs e)
		{
			SizeChanged?.Invoke(this, e);
		}

		private void ItemCollection_ClipSizeChanged(object sender, EventArgs e)
		{
			DoClipSizeChanged((I)sender);
		}

		public event EventHandler<ItemEventArgs<I>> ClipSizeChanged;

		private void DoClipSizeChanged(I item)
		{
			OnClipSizeChanged(new ItemEventArgs<I>(item: item, argument: null));
		}

		protected virtual void OnClipSizeChanged(ItemEventArgs<I> e)
		{
			ClipSizeChanged?.Invoke(this, e);
		}

		private void ItemCollection_ContentChanged(object sender, EventArgs e)
		{
			DoContentChanged((I)sender);
		}
		
		public event EventHandler<ItemEventArgs<I>> ContentChanged;

		private void DoContentChanged(I item)
		{
			OnContentChanged(new ItemEventArgs<I>(item: item, argument: null));
		}

		protected virtual void OnContentChanged(ItemEventArgs<I> e)
		{
			ContentChanged?.Invoke(this, e);
		}

		IEnumerator<I> IEnumerable<I>.GetEnumerator()
		{
			return new ListItemEnumerator(GetEnumerator());
		}

		public void CopyTo(I[] array, int arrayIndex)
		{
			Array.Copy(sourceArray: this.ToArray(), sourceIndex: 0, destinationArray: array, destinationIndex: arrayIndex, length: Count);
		}

		private class ListItemEnumerator : IEnumerator<I>
		{
			private readonly IEnumerator enumerator;

			internal ListItemEnumerator(IEnumerator enumerator)
			{
				this.enumerator = enumerator;
			}

			public I Current => (I)enumerator.Current;

			object IEnumerator.Current => Current;

			public void Dispose()
			{
				enumerator.Reset();
			}

			public bool MoveNext()
			{
				return enumerator.MoveNext();
			}

			public void Reset()
			{
				enumerator.Reset();
			}
		}
	}

	public class ItemEventArgs<I> : EventArgs where I : IListItem, new()
	{
		public ItemEventArgs(I item, object argument) : base()
		{
			Item = item;
			Argument = argument;
		}

		public I Item { get; }

		public object Argument { get; }
	}
}
