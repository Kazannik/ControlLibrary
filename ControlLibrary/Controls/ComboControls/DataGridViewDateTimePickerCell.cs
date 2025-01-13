using System;
using System.Windows.Forms;

namespace ControlLibrary.Controls.ComboControls
{
	internal class DataGridViewDateTimePickerCell : DataGridViewTextBoxCell
	{
		public DataGridViewDateTimePickerCell() : base()
		{
			Style.Format = "d";
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			CalendarEditingControl ctl = DataGridView.EditingControl as CalendarEditingControl;

			if (Value != null && !DBNull.Value.Equals(Value))
			{
				ctl.Value = (DateTime)Value;
			}
		}

		public override Type EditType => typeof(CalendarEditingControl);

		public override Type ValueType => typeof(DateTime);
	}

	public class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
	{
		public CalendarEditingControl()
		{
			Format = DateTimePickerFormat.Short;
		}

		public object EditingControlFormattedValue
		{
			get => Value.ToShortDateString();
			set
			{
				if (value is string v)
				{
					try
					{
						Value = DateTime.Parse(s: v);
					}
					catch
					{
						Value = DateTime.Today;
					}
				}
			}
		}

		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return EditingControlFormattedValue;
		}

		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			Font = dataGridViewCellStyle.Font;
			CalendarForeColor = dataGridViewCellStyle.ForeColor;
			CalendarMonthBackground = dataGridViewCellStyle.BackColor;
		}

		public int EditingControlRowIndex { get; set; }

		public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
		{
			switch (key & Keys.KeyCode)
			{
				case Keys.Left:
				case Keys.Up:
				case Keys.Down:
				case Keys.Right:
				case Keys.Home:
				case Keys.End:
				case Keys.PageDown:
				case Keys.PageUp:
					return true;
				default:
					return !dataGridViewWantsInputKey;
			}
		}

		public void PrepareEditingControlForEdit(bool selectAll) { }

		public bool RepositionEditingControlOnValueChange => false;

		public DataGridView EditingControlDataGridView { get; set; }

		public bool EditingControlValueChanged { get; set; }

		public Cursor EditingPanelCursor => base.Cursor;

		protected override void OnValueChanged(EventArgs eventargs)
		{
			EditingControlValueChanged = true;
			EditingControlDataGridView.NotifyCurrentCellDirty(true);
			base.OnValueChanged(eventargs);
		}
	}

	public class DataGridViewDateTimePickerColumn : DataGridViewColumn
	{
		public DataGridViewDateTimePickerColumn()
		{
			CellTemplate = new DataGridViewDateTimePickerCell();
		}
	}
}
