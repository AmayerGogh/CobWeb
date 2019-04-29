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
            this.Controls.SetChildIndex(this.txt_port, 0);
            this.Controls.SetChildIndex(this.btn_Excute, 0);
            this.Controls.SetChildIndex(this.btn_Connection, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //todo
        private void Btn_Connection_Click(object sender, EventArgs e)
        {
            btn_Connection.Enabled = false;
            IPEndPoint ipe;
            int port = 6666;
            int.TryParse(txt_port.Text, out port);
            IPAddress ipAddress =null;
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipAddr in ipHost.AddressList)
            {
                if (ipAddr.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ipAddr;
                    break;
                }
            }
          
            //创建监听Socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //邦定IP
            IPEndPoint ipLocal = new IPEndPoint(ipAddress,port);
            socket.Bind(ipLocal);
            //开始监听
            socket.Listen(4);
            btn_Connection.Text = "通信已建立";

            //socket.BeginAccept( )

            //Task.Run(() =>
            //{
            //    while (true)
            //    {


            //        var data = new byte[8000];
            //        var recv = socket.Receive(data);
            //        Console.WriteLine("接收到的信息长度为" + recv);
            //        //if (recv == 0)//当信息长度为0，说明客户端连接断开
            //        //    break;
            //        //return Encoding.UTF8.GetString(data, 0, recv);
            //        //var c=  SocketBasic.Receive(socket);
            //    }
               
            //});
        }
        int clientCount = 0;
        //void OnClientConnect(IAsyncResult asyn)
        //{
        //    try
        //    {
        //        // 创建一个新的 Socket 
        //        Socket workerSocket = socket.EndAccept(asyn);
        //        // 递增客户端数目
        //        Interlocked.Increment(ref clientCount);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


      
    }
}
