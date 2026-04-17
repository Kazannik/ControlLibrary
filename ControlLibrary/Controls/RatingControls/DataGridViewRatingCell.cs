using ControlLibrary.Structures;
using System;
using System.Windows.Forms;

namespace ControlLibrary.Controls.RatingControls
{
	public class DataGridViewRatingCell : DataGridViewTextBoxCell
	{
		public DataGridViewRatingCell() : base()
		{
			Style.Format = string.Empty;
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

			RatingEditingControl ctl = DataGridView.EditingControl as RatingEditingControl;

			if (Value != null && !DBNull.Value.Equals(Value))
			{
				ctl.Rating = (Rating)Value;
			}
		}

		public override Type EditType => typeof(RatingEditingControl);

		public override Type ValueType => typeof(int);
	}

	public class RatingEditingControl : RatingControl, IDataGridViewEditingControl
	{
		public RatingEditingControl()
		{

		}

		public object EditingControlFormattedValue
		{
			get => Rating;
			set
			{
				if (value is string v &&
					int.TryParse(v, out int strRating))
				{
					Rating = (Rating)strRating;
				}
				else if (value is int intRating)
				{
					Rating = (Rating)intRating;
				}
				else
				{
					Rating = Rating.Empty;
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

		protected override void OnRatingChanged(RatingEventArgs e)
		{
			EditingControlValueChanged = true;
			EditingControlDataGridView.NotifyCurrentCellDirty(true);
			base.OnRatingChanged(e);
		}
	}

	public class DataGridViewRatingColumn : DataGridViewColumn
	{
		public DataGridViewRatingColumn()
		{
			CellTemplate = new DataGridViewRatingCell();
		}
	}
}
