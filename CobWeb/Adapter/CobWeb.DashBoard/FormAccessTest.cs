using CobWeb.Core;
using CobWeb.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.DashBoard
{
    public class FormAccessTest : FormAccess
    {
        private Button btn_Connection;
        Socket socket;
        public FormAccessTest()
        {
            InitializeComponent();
            this.btn_Excute.Click += new System.EventHandler(this.btn_Excute_Click);
           
            
        }

        private void btn_Excute_Click(object sender, EventArgs e)
        { 
            var request = BuildRequest();
            if (request==null)
            {
                return;
            }
            var req_str =request.SerializeObject();
            SocketBasic.Send(socket, req_str, request.Timeout);
            
        }

        private void InitializeComponent()
        {
            this.btn_Connection = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_Excute
            //            
            this.btn_Excute.Click += new System.EventHandler(this.btn_Excute_Click);
            // 
            // btn_Connection
            // 
            this.btn_Connection.Location = new System.Drawing.Point(768, 44);
            this.btn_Connection.Name = "btn_Connection";
            this.btn_Connection.Size = new System.Drawing.Size(82, 34);
            this.btn_Connection.TabIndex = 38;
            this.btn_Connection.Text = "socket连接";
            this.btn_Connection.UseVisualStyleBackColor = true;
            this.btn_Connection.Click += new System.EventHandler(this.Btn_Connection_Click);
            // 
            // FormAccessTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1031, 626);
            this.Controls.Add(this.btn_Connection);
            this.Name = "FormAccessTest";
            this.Controls.SetChildIndex(this.rtxt_send, 0);
            this.Controls.SetChildIndex(this.rtxt_revice, 0);
            this.Controls.SetChildIndex(this.txt_port, 0);
            this.Controls.SetChildIndex(this.btn_Excute, 0);
            this.Controls.SetChildIndex(this.btn_Connection, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //todo
        private void Btn_Connection_Click(object sender, EventArgs e)
        {
           
        }

   
        
    }
}
