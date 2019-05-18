using CobWeb.Util.SocketHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.DashBoard
{
    public class FormVirtualWeb : Form
    {
        private Label label1;
        private Label label2;
        private Button btn_Con;
        private Button button2;
        private RichTextBox txt_send;
        private RichTextBox txt_recive;
        private Button button1;

        public FormVirtualWeb()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Con = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txt_send = new System.Windows.Forms.RichTextBox();
            this.txt_recive = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "入参";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(421, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "返回";
            // 
            // btn_Con
            // 
            this.btn_Con.Location = new System.Drawing.Point(539, 432);
            this.btn_Con.Name = "btn_Con";
            this.btn_Con.Size = new System.Drawing.Size(90, 34);
            this.btn_Con.TabIndex = 4;
            this.btn_Con.Text = "连接";
            this.btn_Con.UseVisualStyleBackColor = true;
            this.btn_Con.Click += new System.EventHandler(this.btn_Con_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(725, 432);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 34);
            this.button1.TabIndex = 5;
            this.button1.Text = "发送数据";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(629, 432);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 34);
            this.button2.TabIndex = 6;
            this.button2.Text = "关闭";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // txt_send
            // 
            this.txt_send.Location = new System.Drawing.Point(12, 24);
            this.txt_send.Name = "txt_send";
            this.txt_send.Size = new System.Drawing.Size(405, 437);
            this.txt_send.TabIndex = 7;
            this.txt_send.Text = "";
            // 
            // txt_recive
            // 
            this.txt_recive.Location = new System.Drawing.Point(423, 24);
            this.txt_recive.Name = "txt_recive";
            this.txt_recive.Size = new System.Drawing.Size(397, 402);
            this.txt_recive.TabIndex = 8;
            this.txt_recive.Text = "";
            // 
            // FormVirtualWeb
            // 
            this.ClientSize = new System.Drawing.Size(823, 473);
            this.Controls.Add(this.txt_recive);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_Con);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormVirtualWeb";       
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btn_Con_Click(object sender, EventArgs e)
        {
            //原来的
            AsyncConnect();
           // AsyncConnect2();
        }
        public void AsyncConnect2()
        {


            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666);
            c = new IOCPClient(ipe, this);
            c.Connect();
            c.Send("我已经连接");
        }


        IOCPClient c;
        Socket client;
        /// <summary>
        /// 连接到服务器
        /// </summary>
        public void AsyncConnect()
        {
            try
            {
                //端口及IP
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666);

                if (client == null)
                {
                    //创建套接字
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }                                          
                //开始连接到服务器
                client.BeginConnect(ipe, asyncResult =>
                {
                    client.EndConnect(asyncResult);
                    var req = SocketHelper.BuildRequest("你好我是客户端");
                    //向服务器发送消息
                    AsyncSend(client, req);
                    //接受消息
                    AsyncReceive(client);
                }, null);
            }
            catch (Exception ex)
            {

            }


        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        public void AsyncSend(Socket socket, string message)
        {
            if (socket == null || message == string.Empty) return;
            //编码
            byte[] data = Encoding.UTF8.GetBytes(message);
            AsyncSend(socket, data);          
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        public void AsyncSend(Socket socket, byte[] data)
        {
            if (socket == null || data.Length==0) return;         
            try
            {
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    //完成发送消息
                    int length = socket.EndSend(asyncResult);
                }, null);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socket"></param>
        public void AsyncReceive(Socket socket)
        {
            byte[] data = new byte[1024];
            try
            {

                //开始接收数据
                socket.BeginReceive(data, 0, data.Length, SocketFlags.None,
                asyncResult =>
                {
                    try
                    {
                        int length = socket.EndReceive(asyncResult);
                        SetText(Encoding.UTF8.GetString(data));
                    }
                    catch (Exception)
                    {
                        AsyncReceive(socket);
                    }


                    AsyncReceive(socket);
                }, null);
            }
            catch (Exception ex)
            {
            }
        }

        public void AsyncClose()
        {
            if (client!=null)
            {                
                client.Close();
                client.Dispose();
                client = null;
            }
        }
        public void SetText(string str)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => SetText(str)));
            }
            else
            {

                txt_recive.Text += "\r\n" + str;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            var req = SocketHelper.BuildRequest(txt_send.Text);
             AsyncSend(client, req);
            //c.Send(txt_send.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            AsyncClose();
        }
      
    }
}
