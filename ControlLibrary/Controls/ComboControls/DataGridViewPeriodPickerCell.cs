using ControlLibrary.Controls.PriodControls;
using ControlLibrary.Structures;
using System;
using System.Windows.Forms;

namespace ControlLibrary.Controls.ComboControls
{
	public class DataGridViewPeriodPickerCell : DataGridViewTextBoxCell
	{
		public DataGridViewPeriodPickerCell() : base()
		{
			Style.Format = "d";
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			PeriodEditingControl ctl = DataGridView.EditingControl as PeriodEditingControl;

			if (Value != null && !DBNull.Value.Equals(Value))
			{
				ctl.Value = (DateTime)Value;
			}
		}

		public override Type EditType => typeof(PeriodEditingControl);

		public override Type ValueType => typeof(DateTime);
	}

	public class PeriodEditingControl : PeriodPicker, IDataGridViewEditingControl
	{
		public PeriodEditingControl()
		{
			//Format = DateTimePickerFormat.Short;
		}

		public object EditingControlFormattedValue
		{
			get => Value.ToShortDateString();
			set
			{
				if (value is string v &&
					DateTime.TryParse(v, out DateTime stringDate))
				{
					Value = stringDate;
				}
				else if (value is DateTime date)
				{
					Value = date;
				}
				else
				{
					Value = DateTime.Today;
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
			//CalendarForeColor = dataGridViewCellStyle.ForeColor;
			//CalendarMonthBackground = dataGridViewCellStyle.BackColor;
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

		protected override void OnValueChanged(PeriodEventArgs e)
		{
			EditingControlValueChanged = true;
			EditingControlDataGridView.NotifyCurrentCellDirty(true);
			base.OnValueChanged(e);
		}
	}

	public class DataGridViewPeriodPickerColumn : DataGridViewColumn
	{
		public DataGridViewPeriodPickerColumn()
		{
			CellTemplate = new DataGridViewPeriodPickerCell();
		}
	}
}
