namespace ExcelAnalyzer.Controls
{
    partial class PeriodPickerPopupForm
    {
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

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.PopupMenu_PeriodBox = new ExcelAnalyzer.Controls.BasePeriodBox();
            this.SuspendLayout();
            // 
            // PopupMenu_PeriodBox
            // 
            this.PopupMenu_PeriodBox.BackColor = System.Drawing.SystemColors.Window;
            this.PopupMenu_PeriodBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PopupMenu_PeriodBox.Location = new System.Drawing.Point(0, 3);
            this.PopupMenu_PeriodBox.Name = "PopupMenu_PeriodBox";
            this.PopupMenu_PeriodBox.Size = new System.Drawing.Size(182, 116);
            this.PopupMenu_PeriodBox.TabIndex = 0;
            this.PopupMenu_PeriodBox.Text = "#Text";
            this.PopupMenu_PeriodBox.Theme = null;
            this.PopupMenu_PeriodBox.ValueChanged += new System.EventHandler<ExcelAnalyzer.Arm.PeriodEventArgs>(this.PopupMenu_PeriodBox_ValueChanged);
            this.PopupMenu_PeriodBox.ButtonClick += new System.EventHandler(this.PeriodBox_ButtonClick);
            this.PopupMenu_PeriodBox.Resize += new System.EventHandler(this.PeriodPickerPopupForm_Resize);
            // 
            // PeriodPickerPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.PopupMenu_PeriodBox);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Name = "PeriodPickerPopupForm";
            this.Size = new System.Drawing.Size(209, 148);
            this.ResumeLayout(false);

        }

        #endregion

        private BasePeriodBox PopupMenu_PeriodBox;
    }
}
