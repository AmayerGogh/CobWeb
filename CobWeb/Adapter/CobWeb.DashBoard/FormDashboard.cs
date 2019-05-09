
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.DashBoard
{
    public partial class FormDashboard : Form
    {
        public delegate void SetListBoxCallBack(string str);
        public SetListBoxCallBack setlistboxcallback;
       
        public FormDashboard()
        {           
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            Step1_Init();
            Step2_InnerListen();
            Step3_OutListen();
         
        }
        private void FormDashboard_Load(object sender, EventArgs e)
        {

            //iocp.Init();
            //iocp.Start("127.0.0.1", 6666);
            Step4_TimerStart();
        }
        private void btn_test_debug_Click(object sender, EventArgs e)
        {
            FormAccessTest form = new FormAccessTest();
            form.Show();
        }
        private void btn_bin_debug_Click(object sender, EventArgs e)
        {

            var path = Path.GetFullPath(Program.cobwebPath);
            var process = Process.Start(path + "CobWeb.exe");

            //FormAdapter form = new FormAdapter();
            //form.Show();
        }

        private void btn_socketStart_Click(object sender, EventArgs e)
        {
            FormVirtualWeb formVritualWeb = new FormVirtualWeb();
            formVritualWeb.Show();
        }

        Socket socketListen;//用于监听的socket
        Socket socketConnect;//用于通信的socket
        string RemoteEndPoint;     //客户端的网络节点  
        public Dictionary<string, Socket> dicClient = new Dictionary<string, Socket>();//连接的客户端集合
        public void StartSocket()
        {
            //创建套接字
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666);
            socketListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定端口和IP
            socketListen.Bind(ipe);
            //设置监听
            socketListen.Listen(1);
            //连接客户端
            AsyncConnect(socketListen);

        }
        /// <summary>
        /// 连接到客户端
        /// </summary>
        /// <param name="socket"></param>
        private void AsyncConnect(Socket socket)
        {
            try
            {
                socket.BeginAccept(asyncResult =>
                {
                    //获取客户端套接字
                    socketConnect = socket.EndAccept(asyncResult);
                    RemoteEndPoint = socketConnect.RemoteEndPoint.ToString();
                    dicClient.Add(RemoteEndPoint, socketConnect);//添加至客户端集合
                    comboBox1.Items.Add(RemoteEndPoint);//添加客户端端口号

                    AsyncSend(socketConnect, string.Format("欢迎你{0}", socketConnect.RemoteEndPoint));
                    AsyncReceive(socketConnect);
                    AsyncConnect(socketListen);
                }, null);


            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="client"></param>
        private void AsyncReceive(Socket socket)
        {
            byte[] data = new byte[1024];
            IAsyncResult resu = null;
            try
            {
                //开始接收消息
                resu = socket.BeginReceive(data, 0, data.Length, SocketFlags.None,
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
            finally
            {
                resu = null;
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="p"></param>
        private void AsyncSend(Socket client, string message)
        {
            if (client == null || message == string.Empty) return;
            //数据转码
            byte[] data = Encoding.UTF8.GetBytes(message);
            IAsyncResult resu = null;
            try
            {
                //开始发送消息
                resu = client.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
                  {
                    //完成消息发送
                    int length = client.EndSend(asyncResult);
                  }, null);
            }
            catch (Exception ex)
            {
                //发送失败，将该客户端信息删除
                string deleteClient = client.RemoteEndPoint.ToString();
                dicClient.Remove(deleteClient);
                comboBox1.Items.Remove(deleteClient);
            }
            finally
            {
                resu = null;
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
                textBox1.Text += "\r\n" + str;
            }
        }

        /// <summary>
        /// send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                var sock =  dicClient[comboBox1.SelectedItem.ToString()];
                server.Send(textBox2.Text, sock);
            }
           


          


        }

     
    }

}
