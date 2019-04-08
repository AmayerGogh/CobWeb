namespace CobWebAdapter
{
    partial class FormAdapter
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
            this.cmb_Type = new System.Windows.Forms.ComboBox();
            this.numeric_Timeout = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtxt_Param = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rtxt_Result = new System.Windows.Forms.RichTextBox();
            this.txt_stopkey = new System.Windows.Forms.TextBox();
            this.lb_time = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_prot = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_istest = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Timeout)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Excute
            // 
            this.btn_Excute.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Excute.Location = new System.Drawing.Point(391, 414);
            this.btn_Excute.Name = "btn_Excute";
            this.btn_Excute.Size = new System.Drawing.Size(75, 25);
            this.btn_Excute.TabIndex = 11;
            this.btn_Excute.Text = "调用";
            this.btn_Excute.UseVisualStyleBackColor = true;
            // 
            // cmb_Type
            // 
            this.cmb_Type.FormattingEnabled = true;
            this.cmb_Type.Items.AddRange(new object[] {
            "GetVersion",
            "FindVehicle",
            "CarRenewal",
            "QuotePrice",
            "SetReqKeyVal",
            "FindReconciliationInfo",
            "GetCarInfos",
            "UpdateConfigInfo",
            "ReStartService",
            "GetVehiclesByName",
            "GetVehiclesByNameStep2",
            "NewCarRecord",
            "GetQuotedDataByTNo",
            "GetAccidentProduct",
            "GetCarRenewal"});
            this.cmb_Type.Location = new System.Drawing.Point(78, 61);
            this.cmb_Type.Name = "cmb_Type";
            this.cmb_Type.Size = new System.Drawing.Size(252, 20);
            this.cmb_Type.TabIndex = 3;
            // 
            // numeric_Timeout
            // 
            this.numeric_Timeout.Location = new System.Drawing.Point(78, 85);
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
            this.numeric_Timeout.Size = new System.Drawing.Size(252, 21);
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
            this.label1.Location = new System.Drawing.Point(15, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "超时时间:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "操作类型:";
            // 
            // rtxt_Param
            // 
            this.rtxt_Param.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.rtxt_Param.Location = new System.Drawing.Point(78, 112);
            this.rtxt_Param.Name = "rtxt_Param";
            this.rtxt_Param.Size = new System.Drawing.Size(400, 274);
            this.rtxt_Param.TabIndex = 1;
            this.rtxt_Param.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "请求参数:";
            // 
            // rtxt_Result
            // 
            this.rtxt_Result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxt_Result.Location = new System.Drawing.Point(496, 47);
            this.rtxt_Result.Name = "rtxt_Result";
            this.rtxt_Result.Size = new System.Drawing.Size(255, 339);
            this.rtxt_Result.TabIndex = 6;
            this.rtxt_Result.Text = "";
            // 
            // txt_stopkey
            // 
            this.txt_stopkey.Location = new System.Drawing.Point(496, 18);
            this.txt_stopkey.Name = "txt_stopkey";
            this.txt_stopkey.Size = new System.Drawing.Size(254, 21);
            this.txt_stopkey.TabIndex = 5;
            // 
            // lb_time
            // 
            this.lb_time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_time.AutoSize = true;
            this.lb_time.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_time.Location = new System.Drawing.Point(75, 420);
            this.lb_time.Name = "lb_time";
            this.lb_time.Size = new System.Drawing.Size(78, 12);
            this.lb_time.TabIndex = 12;
            this.lb_time.Text = "耗时(毫秒):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "通信端口:";
            // 
            // txt_prot
            // 
            this.txt_prot.Location = new System.Drawing.Point(78, 18);
            this.txt_prot.Name = "txt_prot";
            this.txt_prot.Size = new System.Drawing.Size(251, 21);
            this.txt_prot.TabIndex = 16;
            this.txt_prot.Text = "6666";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(79, 43);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "使用窗口:";
            // 
            // cb_istest
            // 
            this.cb_istest.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cb_istest.AutoSize = true;
            this.cb_istest.Location = new System.Drawing.Point(291, 420);
            this.cb_istest.Name = "cb_istest";
            this.cb_istest.Size = new System.Drawing.Size(96, 16);
            this.cb_istest.TabIndex = 19;
            this.cb_istest.Text = "是否测试环境";
            this.cb_istest.UseVisualStyleBackColor = true;
            // 
            // FormAdapter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.cb_istest);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.txt_prot);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lb_time);
            this.Controls.Add(this.txt_stopkey);
            this.Controls.Add(this.rtxt_Result);
            this.Controls.Add(this.rtxt_Param);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numeric_Timeout);
            this.Controls.Add(this.cmb_Type);
            this.Controls.Add(this.btn_Excute);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FormAdapter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "调用测试";
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Timeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Excute;
        private System.Windows.Forms.ComboBox cmb_Type;
        private System.Windows.Forms.NumericUpDown numeric_Timeout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtxt_Param;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtxt_Result;
        private System.Windows.Forms.TextBox txt_stopkey;
        private System.Windows.Forms.Label lb_time;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_prot;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cb_istest;
    }
}