namespace ControlLibrary.Controls.ComboControls
{
	public interface IComboItem
	{
		string Guid { get; }
		long Id { get; }
		string Code { get; }
		string Caption { get; }
	}
}
