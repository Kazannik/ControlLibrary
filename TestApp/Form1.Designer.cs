namespace TestApp
{
	partial class Form1
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

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripPeriodBox1 = new ControlLibrary.Controls.ToolStripControls.ToolStripPeriodBox();
			this.toolStripRatingBox1 = new ControlLibrary.Controls.ToolStripControls.ToolStripRatingBox();
			this.periodPicker1 = new ControlLibrary.Controls.PriodControls.PeriodPicker();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Column1 = new ControlLibrary.Controls.ComboControls.DataGridViewPeriodPickerColumn();
			this.ratingControl1 = new ControlLibrary.Controls.RatingControls.RatingControl(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.myTreeViewCombo1 = new ControlLibrary.Controls.MyTreeViewCombo();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.button1 = new System.Windows.Forms.Button();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripPeriodBox1,
            this.toolStripRatingBox1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(800, 31);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(29, 28);
			this.toolStripButton1.Text = "toolStripButton1";
			// 
			// toolStripPeriodBox1
			// 
			this.toolStripPeriodBox1.Name = "toolStripPeriodBox1";
			this.toolStripPeriodBox1.Size = new System.Drawing.Size(121, 28);
			this.toolStripPeriodBox1.Text = "toolStripPeriodBox1";
			// 
			// toolStripRatingBox1
			// 
			this.toolStripRatingBox1.Name = "toolStripRatingBox1";
			this.toolStripRatingBox1.Size = new System.Drawing.Size(140, 28);
			this.toolStripRatingBox1.StarsColor = System.Drawing.Color.Red;
			this.toolStripRatingBox1.Text = "toolStripRatingBox1";
			// 
			// periodPicker1
			// 
			this.periodPicker1.DropDownHeight = 134;
			this.periodPicker1.DropDownWidth = 212;
			this.periodPicker1.FormattingEnabled = true;
			this.periodPicker1.IntegralHeight = false;
			this.periodPicker1.Location = new System.Drawing.Point(480, 192);
			this.periodPicker1.Name = "periodPicker1";
			this.periodPicker1.Size = new System.Drawing.Size(180, 24);
			this.periodPicker1.TabIndex = 1;
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
			this.dataGridView1.Location = new System.Drawing.Point(12, 74);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersWidth = 51;
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.Size = new System.Drawing.Size(413, 289);
			this.dataGridView1.TabIndex = 2;
			// 
			// Column1
			// 
			this.Column1.HeaderText = "Column1";
			this.Column1.MinimumWidth = 6;
			this.Column1.Name = "Column1";
			this.Column1.Width = 125;
			// 
			// ratingControl1
			// 
			this.ratingControl1.BackColor = System.Drawing.SystemColors.Control;
			this.ratingControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ratingControl1.ForeColor = System.Drawing.SystemColors.Desktop;
			this.ratingControl1.Location = new System.Drawing.Point(480, 84);
			this.ratingControl1.Name = "ratingControl1";
			this.ratingControl1.Size = new System.Drawing.Size(172, 25);
			this.ratingControl1.StarsColor1 = System.Drawing.Color.Red;
			this.ratingControl1.StarsColor2 = System.Drawing.Color.Yellow;
			this.ratingControl1.TabIndex = 3;
			this.ratingControl1.Text = "ratingControl1";
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Location = new System.Drawing.Point(0, 428);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(800, 22);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// myTreeViewCombo1
			// 
			this.myTreeViewCombo1.FormattingEnabled = true;
			this.myTreeViewCombo1.Location = new System.Drawing.Point(480, 120);
			this.myTreeViewCombo1.Name = "myTreeViewCombo1";
			this.myTreeViewCombo1.Size = new System.Drawing.Size(180, 24);
			this.myTreeViewCombo1.TabIndex = 5;
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(480, 156);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(180, 22);
			this.dateTimePicker1.TabIndex = 6;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(576, 288);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(132, 48);
			this.button1.TabIndex = 7;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.myTreeViewCombo1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.ratingControl1);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.periodPicker1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private ControlLibrary.Controls.PriodControls.PeriodPicker periodPicker1;
		private ControlLibrary.Controls.ToolStripControls.ToolStripPeriodBox toolStripPeriodBox1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private ControlLibrary.Controls.ComboControls.DataGridViewPeriodPickerColumn Column1;
		private ControlLibrary.Controls.RatingControls.RatingControl ratingControl1;
		private ControlLibrary.Controls.ToolStripControls.ToolStripRatingBox toolStripRatingBox1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private ControlLibrary.Controls.MyTreeViewCombo myTreeViewCombo1;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.Button button1;
	}
}

