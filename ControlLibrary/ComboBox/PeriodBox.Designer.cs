namespace ExcelAnalyzer.Controls
{
    partial class PeriodBox
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
            this.BasePeriodBox = new ExcelAnalyzer.Controls.BasePeriodBox();
            this.SuspendLayout();
            // 
            // BasePeriodBox
            // 
            this.BasePeriodBox.BackColor = System.Drawing.SystemColors.Window;
            this.BasePeriodBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.BasePeriodBox.Location = new System.Drawing.Point(3, 15);
            this.BasePeriodBox.Name = "BasePeriodBox";
            this.BasePeriodBox.Size = new System.Drawing.Size(182, 116);
            this.BasePeriodBox.TabIndex = 0;
            this.BasePeriodBox.Text = "PeriodBox";
            this.BasePeriodBox.Theme = null;
            // 
            // PeriodBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.BasePeriodBox);
            this.Name = "PeriodBox";
            this.Size = new System.Drawing.Size(146, 146);
            this.ResumeLayout(false);

        }

        #endregion

        private BasePeriodBox BasePeriodBox;
    }
}
