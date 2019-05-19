namespace CobWeb.DashBoard
{
    partial class FormAccess
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Excute = new System.Windows.Forms.Button();
            this.numeric_Timeout = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.rtxt_send = new System.Windows.Forms.RichTextBox();
            this.rtxt_revice = new System.Windows.Forms.RichTextBox();
            this.txt_stopkey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_fileName = new System.Windows.Forms.TextBox();
            this.cob_kernel = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cob_RequestCode = new System.Windows.Forms.ComboBox();
            this.btn_rm_send = new System.Windows.Forms.Button();
            this.btn_rm_recive = new System.Windows.Forms.Button();
            this.lbl_Msg = new System.Windows.Forms.Label();
            this.cob_Type = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cob_clients = new System.Windows.Forms.ComboBox();
            this.btn_ref = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Timeout)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Excute
            // 
            this.btn_Excute.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Excute.Location = new System.Drawing.Point(768, 588);
            this.btn_Excute.Name = "btn_Excute";
            this.btn_Excute.Size = new System.Drawing.Size(251, 35);
            this.btn_Excute.TabIndex = 11;
            this.btn_Excute.Text = "调用";
            this.btn_Excute.UseVisualStyleBackColor = true;
            this.btn_Excute.Click += new System.EventHandler(this.btn_Excute_Click);
            // 
            // numeric_Timeout
            // 
            this.numeric_Timeout.Location = new System.Drawing.Point(831, 16);
            this.numeric_Timeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numeric_Timeout.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numeric_Timeout.Name = "numeric_Timeout";
            this.numeric_Timeout.Size = new System.Drawing.Size(165, 21);
            this.numeric_Timeout.TabIndex = 4;
            this.numeric_Timeout.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(766, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "超时时间:";
            // 
            // rtxt_send
            // 
            this.rtxt_send.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.rtxt_send.Location = new System.Drawing.Point(9, 84);
            this.rtxt_send.Name = "rtxt_send";
            this.rtxt_send.Size = new System.Drawing.Size(493, 498);
            this.rtxt_send.TabIndex = 1;
            this.rtxt_send.Text = "";
            // 
            // rtxt_revice
            // 
            this.rtxt_revice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxt_revice.Location = new System.Drawing.Point(508, 84);
            this.rtxt_revice.Name = "rtxt_revice";
            this.rtxt_revice.Size = new System.Drawing.Size(511, 498);
            this.rtxt_revice.TabIndex = 6;
            this.rtxt_revice.Text = "";
            // 
            // txt_stopkey
            // 
            this.txt_stopkey.Enabled = false;
            this.txt_stopkey.Location = new System.Drawing.Point(70, 45);
            this.txt_stopkey.Name = "txt_stopkey";
            this.txt_stopkey.Size = new System.Drawing.Size(165, 21);
            this.txt_stopkey.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "通信端口:";
            // 
            // txt_port
            // 
            this.txt_port.Enabled = false;
            this.txt_port.Location = new System.Drawing.Point(70, 16);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(165, 21);
            this.txt_port.TabIndex = 16;
            this.txt_port.Text = "6666";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(496, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "外部包名:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "key:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(255, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "内核:";
            // 
            // txt_fileName
            // 
            this.txt_fileName.Location = new System.Drawing.Point(573, 19);
            this.txt_fileName.Name = "txt_fileName";
            this.txt_fileName.Size = new System.Drawing.Size(165, 21);
            this.txt_fileName.TabIndex = 27;
            // 
            // cob_kernel
            // 
            this.cob_kernel.FormattingEnabled = true;
            this.cob_kernel.Items.AddRange(new object[] {
            "IE",
            "Cef",
            "WebKit"});
            this.cob_kernel.Location = new System.Drawing.Point(314, 19);
            this.cob_kernel.Name = "cob_kernel";
            this.cob_kernel.Size = new System.Drawing.Size(165, 20);
            this.cob_kernel.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(312, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 12);
            this.label8.TabIndex = 32;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(255, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 34;
            this.label9.Text = "操作类型:";
            // 
            // cob_RequestCode
            // 
            this.cob_RequestCode.FormattingEnabled = true;
            this.cob_RequestCode.Location = new System.Drawing.Point(314, 45);
            this.cob_RequestCode.Name = "cob_RequestCode";
            this.cob_RequestCode.Size = new System.Drawing.Size(165, 20);
            this.cob_RequestCode.TabIndex = 33;
            // 
            // btn_rm_send
            // 
            this.btn_rm_send.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_rm_send.Location = new System.Drawing.Point(9, 588);
            this.btn_rm_send.Name = "btn_rm_send";
            this.btn_rm_send.Size = new System.Drawing.Size(90, 35);
            this.btn_rm_send.TabIndex = 35;
            this.btn_rm_send.Text = "清空send";
            this.btn_rm_send.UseVisualStyleBackColor = true;
            this.btn_rm_send.Click += new System.EventHandler(this.Btn_rm_send_Click);
            // 
            // btn_rm_recive
            // 
            this.btn_rm_recive.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_rm_recive.Location = new System.Drawing.Point(105, 588);
            this.btn_rm_recive.Name = "btn_rm_recive";
            this.btn_rm_recive.Size = new System.Drawing.Size(90, 35);
            this.btn_rm_recive.TabIndex = 36;
            this.btn_rm_recive.Text = "清空recive";
            this.btn_rm_recive.UseVisualStyleBackColor = true;
            this.btn_rm_recive.Click += new System.EventHandler(this.Btn_rm_recive_Click);
            // 
            // lbl_Msg
            // 
            this.lbl_Msg.AutoSize = true;
            this.lbl_Msg.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Msg.ForeColor = System.Drawing.Color.Red;
            this.lbl_Msg.Location = new System.Drawing.Point(214, 601);
            this.lbl_Msg.Name = "lbl_Msg";
            this.lbl_Msg.Size = new System.Drawing.Size(56, 16);
            this.lbl_Msg.TabIndex = 37;
            this.lbl_Msg.Text = "初始值";
            // 
            // cob_Type
            // 
            this.cob_Type.FormattingEnabled = true;
            this.cob_Type.Items.AddRange(new object[] {
            "GetVersion"});
            this.cob_Type.Location = new System.Drawing.Point(573, 46);
            this.cob_Type.Name = "cob_Type";
            this.cob_Type.Size = new System.Drawing.Size(165, 20);
            this.cob_Type.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(496, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "方法名:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(766, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 39;
            this.label4.Text = "调用端口:";
            // 
            // cob_clients
            // 
            this.cob_clients.FormattingEnabled = true;
            this.cob_clients.Items.AddRange(new object[] {
            "由系统决定"});
            this.cob_clients.Location = new System.Drawing.Point(831, 45);
            this.cob_clients.Name = "cob_clients";
            this.cob_clients.Size = new System.Drawing.Size(114, 20);
            this.cob_clients.TabIndex = 38;
            // 
            // btn_ref
            // 
            this.btn_ref.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_ref.Location = new System.Drawing.Point(942, 43);
            this.btn_ref.Name = "btn_ref";
            this.btn_ref.Size = new System.Drawing.Size(54, 23);
            this.btn_ref.TabIndex = 40;
            this.btn_ref.Text = "刷新";
            this.btn_ref.UseVisualStyleBackColor = true;
            this.btn_ref.Click += new System.EventHandler(this.btn_ref_Click);
            // 
            // FormAccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 626);
            this.Controls.Add(this.btn_ref);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cob_clients);
            this.Controls.Add(this.lbl_Msg);
            this.Controls.Add(this.btn_rm_recive);
            this.Controls.Add(this.btn_rm_send);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cob_RequestCode);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cob_kernel);
            this.Controls.Add(this.txt_fileName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_stopkey);
            this.Controls.Add(this.rtxt_revice);
            this.Controls.Add(this.rtxt_send);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numeric_Timeout);
            this.Controls.Add(this.cob_Type);
            this.Controls.Add(this.btn_Excute);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FormAccess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "调用测试";
            this.Load += new System.EventHandler(this.FormAccess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Timeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        public System.Windows.Forms.Button btn_Excute;
        private System.Windows.Forms.NumericUpDown numeric_Timeout;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RichTextBox rtxt_send;
        public System.Windows.Forms.RichTextBox rtxt_revice;
        private System.Windows.Forms.TextBox txt_stopkey;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_fileName;
        private System.Windows.Forms.ComboBox cob_kernel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cob_RequestCode;
        private System.Windows.Forms.Button btn_rm_send;
        private System.Windows.Forms.Button btn_rm_recive;
        private System.Windows.Forms.Label lbl_Msg;
        private System.Windows.Forms.ComboBox cob_Type;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cob_clients;
        public System.Windows.Forms.Button btn_ref;
    }
}