using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Windows.Forms.ListBox;

namespace ControlLibrary.Controls.ListControls
{
	public class ItemCollection<T> : ObjectCollection, IList<T>, ICollection<T>, IEnumerable<T> where T : IListItem, new()
	{
		public ItemCollection(ListControl<T> owner) : base(owner: owner) { }
		public ItemCollection(ListControl<T> owner, object[] value) : base(owner: owner, value: value) { }
		public ItemCollection(ListControl<T> owner, ObjectCollection value) : base(owner: owner, value: value) { }

		public new T this[int index]
		{
			get => (T)base[index];
			set => base[index] = value;
		}

		public new void Add(object value)
		{
			OnValidate(value);
			((T)value).ContentChanged += new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			base.Add(value);
			DoCountChanged();
		}

		private void ItemCollection_ContentChanged(object sender, EventArgs e)
		{
			DoContentChanged();
		}

		public void Add(T item)
		{			
			Add(value: item);
		}

		public new void AddRange(object[] value)
		{
			for (int i = 0; i < value.Length; i++)
			{
				OnValidate(value[i]);
				T item = (T)value[i];
				item.ContentChanged += new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			}
			base.AddRange(items: value);
			DoCountChanged();
		}

		public new void AddRange(ObjectCollection value)
		{
			for (int i = 0; i < value.Count; i++)
			{
				OnValidate(value[i]);
				T item = (T)value[i];
				item.ContentChanged += new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			}
			base.AddRange(value: value);
			DoCountChanged();
		}

		public void AddRange(ItemCollection<T> items)
		{
			AddRange(value: items);			
		}

		public void AddRange(IList<T> items)
		{
			AddRange(value: (from T item in items select (object)item).ToArray());
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

		public int IndexOf(T item)
		{
			return IndexOf(value: item);
		}

		public new void Insert(int index, object value)
		{
			OnValidate(value: value);
			((T)value).ContentChanged += new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			Insert(index: index, item: value);
			DoCountChanged();
		}

		public void Insert(int index, T item)
		{
			Insert(index: index,  value: item);
		}

		public new void Remove(object value)
		{
			OnValidate(value: value);
			((T)value).ContentChanged -= new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			base.Remove(value: value);
		}

		public void Remove(T item)
		{
			Remove(value: item);			
		}

		bool ICollection<T>.Remove(T item)
		{
			Remove(value: item);
			return !base.Contains(item);
		}

		public new void Clear()
		{
			ItemCollection<T> list = this;
			for (int i = 0; i < list.Count; i++)
			{
				T item = list[i];
				item.ContentChanged -= new EventHandler<EventArgs>(ItemCollection_ContentChanged);
			}
			base.Clear();
			DoCountChanged();
		}

		public new bool Contains(object value)
		{
			OnValidate(value: value);
			return base.Contains(value: value);
		}

		public bool Contains(T item)
		{
			return Contains(value: item);
		}
				
		protected void OnValidate(object value)
		{
			if (!typeof(T).IsAssignableFrom(value.GetType()))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Не удалось привести тип Value: {0} к поддурживаемому коллекцией типу: {1}.", value.GetType().ToString(), typeof(T).ToString()));
			}
		}

		public event EventHandler<EventArgs> CountChanged;

		private void DoCountChanged()
		{			
			OnCountChanged(new EventArgs());
			DoContentChanged();
		}

		protected virtual void OnCountChanged(EventArgs e)
		{
			CountChanged?.Invoke(this, e);
		}

		private void ListItem_ContentChanged(object sender, EventArgs e)
		{
			DoContentChanged();
		}

		public event EventHandler<EventArgs> ContentChanged;

		private void DoContentChanged()
		{
			OnContentChanged(new EventArgs());
		}

		protected virtual void OnContentChanged(EventArgs e)
		{
			ContentChanged?.Invoke(this, e);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new ListItemEnumerator(GetEnumerator());
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			//array.CopyTo(this, arrayIndex);
		}

		private class ListItemEnumerator : IEnumerator<T>
		{
			private readonly IEnumerator enumerator;

			internal ListItemEnumerator(IEnumerator enumerator)
			{
				this.enumerator = enumerator;
			}

			public T Current
			{
				get
				{
					return (T)enumerator.Current;					
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}

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
}
