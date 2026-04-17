using ControlLibrary.Structures;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ControlLibrary.Controls.PriodControls
{
	public partial class PeriodPickerPopupForm : UserControl
	{
		private PeriodControl PopupMenu_PeriodBox;

		public PeriodPickerPopupForm()
		{
			InitializeComponent();
		}

		protected override void OnResize(EventArgs e)
		{
			PopupMenu_PeriodBox.Location = new Point(0, 0);
			Size = PopupMenu_PeriodBox.Size;
			base.OnResize(e);
		}

		#region Value

		public Period Value
		{
			get => PopupMenu_PeriodBox.Value;
			set => PopupMenu_PeriodBox.Value = value;
		}

		private void PopupMenu_PeriodBox_ValueChanged(object sender, PeriodEventArgs e)
		{
			OnValueChanged(new PeriodEventArgs(e.Period));
		}

		public event EventHandler<PeriodEventArgs> ValueChanged;

		protected virtual void OnValueChanged(PeriodEventArgs e)
		{
			ValueChanged?.Invoke(this, e);
		}

		#endregion

		#region Click

		private void PeriodBox_ButtonClick(object sender, EventArgs e)
		{
			OnButtonClick(new EventArgs());
		}

		public event EventHandler ButtonClick;

		protected virtual void OnButtonClick(EventArgs e)
		{
			ButtonClick?.Invoke(this, e);
		}

		#endregion

		#region Initialize

		/// <summary> 
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором компонентов

		private void InitializeComponent()
		{
			this.PopupMenu_PeriodBox = new ControlLibrary.Controls.PriodControls.PeriodControl();
			this.SuspendLayout();
			// 
			// PopupMenu_PeriodBox
			// 
			this.PopupMenu_PeriodBox.BackColor = System.Drawing.SystemColors.Window;
			this.PopupMenu_PeriodBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PopupMenu_PeriodBox.ForeColor = System.Drawing.SystemColors.WindowText;
			this.PopupMenu_PeriodBox.Location = new System.Drawing.Point(0, 0);
			this.PopupMenu_PeriodBox.Margin = new System.Windows.Forms.Padding(4);
			this.PopupMenu_PeriodBox.MinimumSize = new System.Drawing.Size(212, 134);
			this.PopupMenu_PeriodBox.Name = "PopupMenu_PeriodBox";
			this.PopupMenu_PeriodBox.Size = new System.Drawing.Size(212, 134);
			this.PopupMenu_PeriodBox.TabIndex = 0;
			this.PopupMenu_PeriodBox.Text = "#Text";
			this.PopupMenu_PeriodBox.ValueChanged += new System.EventHandler<ControlLibrary.Structures.PeriodEventArgs>(this.PopupMenu_PeriodBox_ValueChanged);
			this.PopupMenu_PeriodBox.ButtonClick += new System.EventHandler(this.PeriodBox_ButtonClick);
			// 
			// PeriodPickerPopupForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.PopupMenu_PeriodBox);
			this.ForeColor = System.Drawing.SystemColors.WindowText;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "PeriodPickerPopupForm";
			this.Size = new System.Drawing.Size(233, 184);
			this.ResumeLayout(false);

		}

		#endregion


		#endregion


	}
}
