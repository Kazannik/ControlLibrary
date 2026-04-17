namespace ControlLibrary.Controls.PriodControls
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
			this.PeriodControl = new ControlLibrary.Controls.PriodControls.PeriodControl();
			this.SuspendLayout();
			// 
			// PeriodControl
			// 
			this.PeriodControl.BackColor = System.Drawing.SystemColors.Window;
			this.PeriodControl.ForeColor = System.Drawing.SystemColors.WindowText;
			this.PeriodControl.Location = new System.Drawing.Point(0, 0);
			this.PeriodControl.Margin = new System.Windows.Forms.Padding(4);
			this.PeriodControl.MinimumSize = new System.Drawing.Size(212, 134);
			this.PeriodControl.Name = "PeriodControl";
			this.PeriodControl.Size = new System.Drawing.Size(212, 134);
			this.PeriodControl.TabIndex = 0;
			this.PeriodControl.Text = "Period";
			// 
			// PeriodBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.PeriodControl);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "PeriodBox";
			this.Size = new System.Drawing.Size(252, 163);
			this.ResumeLayout(false);

        }

        #endregion

        private PeriodControl PeriodControl;
	}
}
