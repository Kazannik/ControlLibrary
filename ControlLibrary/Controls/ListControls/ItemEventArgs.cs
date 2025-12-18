using System;

namespace ControlLibrary.Controls.ListControls
{
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
