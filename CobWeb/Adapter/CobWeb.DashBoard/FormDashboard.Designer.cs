namespace CobWeb.DashBoard
{
    partial class FormDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_test_debug = new System.Windows.Forms.Button();
            this.btn_bin_ie = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btn_testStart = new System.Windows.Forms.Button();
            this.txt_debug = new System.Windows.Forms.RichTextBox();
            this.btn_bin_ref = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_test_debug
            // 
            this.btn_test_debug.Location = new System.Drawing.Point(12, 12);
            this.btn_test_debug.Name = "btn_test_debug";
            this.btn_test_debug.Size = new System.Drawing.Size(124, 67);
            this.btn_test_debug.TabIndex = 0;
            this.btn_test_debug.Text = "开启调用";
            this.btn_test_debug.UseVisualStyleBackColor = true;
            this.btn_test_debug.Click += new System.EventHandler(this.btn_test_debug_Click);
            // 
            // btn_bin_ie
            // 
            this.btn_bin_ie.Location = new System.Drawing.Point(142, 12);
            this.btn_bin_ie.Name = "btn_bin_ie";
            this.btn_bin_ie.Size = new System.Drawing.Size(124, 67);
            this.btn_bin_ie.TabIndex = 1;
            this.btn_bin_ie.Text = "开启IE窗口";
            this.btn_bin_ie.UseVisualStyleBackColor = true;
            this.btn_bin_ie.Click += new System.EventHandler(this.btn_bin_debug_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column7,
            this.Column5,
            this.Column6});
            this.dataGridView1.Location = new System.Drawing.Point(0, 94);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(943, 218);
            this.dataGridView1.TabIndex = 3;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "进程Id";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "参数";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 300;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "内存占用";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "CPU占用";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "线程数";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "启动时间";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "在执行";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button4);
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 318);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(416, 219);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(3, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(113, 70);
            this.button4.TabIndex = 0;
            this.button4.Text = "QuotePrice\r\n使用窗口\r\n当前运行时间\r\n00:01:92\r\n";
            this.button4.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(122, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(129, 58);
            this.button5.TabIndex = 1;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(532, 12);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(124, 67);
            this.button6.TabIndex = 5;
            this.button6.Text = "todo";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // btn_testStart
            // 
            this.btn_testStart.Location = new System.Drawing.Point(402, 12);
            this.btn_testStart.Name = "btn_testStart";
            this.btn_testStart.Size = new System.Drawing.Size(124, 67);
            this.btn_testStart.TabIndex = 6;
            this.btn_testStart.TabStop = false;
            this.btn_testStart.Text = "Browser模拟";
            this.btn_testStart.UseVisualStyleBackColor = true;
            this.btn_testStart.Click += new System.EventHandler(this.btn_socketStart_Click);
            // 
            // txt_debug
            // 
            this.txt_debug.Location = new System.Drawing.Point(431, 322);
            this.txt_debug.Name = "txt_debug";
            this.txt_debug.Size = new System.Drawing.Size(512, 215);
            this.txt_debug.TabIndex = 10;
            this.txt_debug.Text = "";
            // 
            // btn_bin_ref
            // 
            this.btn_bin_ref.Location = new System.Drawing.Point(272, 12);
            this.btn_bin_ref.Name = "btn_bin_ref";
            this.btn_bin_ref.Size = new System.Drawing.Size(124, 67);
            this.btn_bin_ref.TabIndex = 11;
            this.btn_bin_ref.Text = "开启 Cef 窗口";
            this.btn_bin_ref.UseVisualStyleBackColor = true;
            this.btn_bin_ref.Click += new System.EventHandler(this.btn_bin_ref_Click);
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 563);
            this.Controls.Add(this.btn_bin_ref);
            this.Controls.Add(this.txt_debug);
            this.Controls.Add(this.btn_testStart);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_bin_ie);
            this.Controls.Add(this.btn_test_debug);
            this.Name = "FormDashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.FormDashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.Button btn_test_debug;
        private System.Windows.Forms.Button btn_bin_ie;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Button btn_testStart;
        private System.Windows.Forms.RichTextBox txt_debug;
        private System.Windows.Forms.Button btn_bin_ref;
    }
}

