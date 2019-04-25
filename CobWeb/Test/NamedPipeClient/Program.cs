using CobWeb.Util.SocketHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NamedPipeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketClient socketClient = new SocketClient();
            socketClient.Conn();
            Console.ReadKey();
        }
    }



    public class SocketClient
    {
        public delegate void UpdateReceiveMsgCallback(string msg);

        byte[] dataBuffer = new byte[10];
        IAsyncResult result;
        public AsyncCallback pfnCallBack;
        public Socket clientSocket;

        string tb_ServerIP;
        int tb_ServerPort =8000;
        public SocketClient()
        {
           
            InitializeInfo();
            ConnectionServer();
        }

         void  InitializeInfo()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());

            string ipAddrV4 = String.Empty;
            foreach (IPAddress ipAddr in ipHost.AddressList)
            {
                if (ipAddr.AddressFamily == AddressFamily.InterNetwork)
                    ipAddrV4 = ipAddr.ToString();
            }
            tb_ServerIP= ipAddrV4;
        }
        void  ConnectionServer()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Create the end point 
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(tb_ServerIP), tb_ServerPort);

            // Connect to the remote host
            clientSocket.Connect(ipEnd);

            if (clientSocket.Connected)
            {
                Console.WriteLine("已经连接到服务器");
                Task.Run(() => {
                    Console.WriteLine("异步");

                    WaitForData();
                });
               
              
                //Wait for data asynchronously 
              
             }
        }
      public void Conn()
        {
            if (!clientSocket.Connected)
            {
                return;
            }
            while (true)
            {
                var str = Console.ReadLine();
                //Send Data By byte[]
                byte[] byData = System.Text.Encoding.UTF8.GetBytes(str);
                if (clientSocket != null)
                    clientSocket.Send(byData);

            }
        }
        void WaitForData()
        {
            if (pfnCallBack == null)
                pfnCallBack = new AsyncCallback(OnDataReceived);

            SocketPacket theSocPkt = new SocketPacket();
            theSocPkt.thisSocket = clientSocket;

            result = clientSocket.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnCallBack, theSocPkt);

        }
        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                int iRx = theSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                var res = new System.String(chars);
                if (res.Contains("接收完成"))
                {
                    Console.WriteLine("(已送达)");
                }
                else
                {
                    Console.WriteLine("服务器返回消息: ==>");
                    Console.WriteLine(new System.String(chars));
                    Console.WriteLine("==================");
                }
              
                

                WaitForData();
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", Environment.NewLine + "数据接收时: Socket 已关闭");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
    }
    public class SocketPacket
    {
        public System.Net.Sockets.Socket thisSocket;
        public byte[] dataBuffer = new byte[2048];
    }




    #region
    /*
     
            //创建客户端对象，默认连接本机127.0.0.1,端口为12345
            SocketClient client = new SocketClient(12345);

            //绑定当收到服务器发送的消息后的处理事件
            client.HandleRecMsg = new Action<byte[], SocketClient>((bytes, theClient) =>
            {
                string msg = Encoding.UTF8.GetString(bytes);
                Console.WriteLine($"收到消息:{msg}");
            });

            //绑定向服务器发送消息后的处理事件
            client.HandleSendMsg = new Action<byte[], SocketClient>((bytes, theClient) =>
            {
                string msg = Encoding.UTF8.GetString(bytes);
                Console.WriteLine($"向服务器发送消息:{msg}");
            });

            //开始运行客户端
            client.StartClient();

            while (true)
            {
                Console.WriteLine("输入:quit关闭客户端，输入其它消息发送到服务器");
                string str = Console.ReadLine();
                if (str == "quit")
                {
                    client.Close();
                    break;
                }
                else
                {
                    client.Send(str);
                }
            }
     */
    #endregion
}
