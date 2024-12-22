using System;
using System.Windows.Forms;

namespace DatabaseToolSuite.Controls
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

		public override Type EditType
		{
			get
			{
				return typeof(CalendarEditingControl);
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(DateTime);
			}
		}
	}

	public class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
	{
		public CalendarEditingControl()
		{
			Format = DateTimePickerFormat.Short;
		}

		public object EditingControlFormattedValue
		{
			get
			{
				return Value.ToShortDateString();
			}
			set
			{
				if (value is string)
				{
					try
					{
						Value = DateTime.Parse(s: (string)value);
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

		public bool RepositionEditingControlOnValueChange
		{
			get
			{
				return false;
			}
		}

		public DataGridView EditingControlDataGridView { get; set; }

		public bool EditingControlValueChanged { get; set; }

		public Cursor EditingPanelCursor
		{
			get
			{
				return base.Cursor;
			}
		}

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
