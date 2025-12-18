using System.Windows.Forms;

namespace ControlLibrary.Controls.ListControls
{
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
