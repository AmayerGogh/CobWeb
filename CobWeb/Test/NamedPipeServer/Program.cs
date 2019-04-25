using CobWeb.Util.SocketHelper;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace NamedPipeServer
{
    class Program
    {        
        static void Main(string[] args)
        {

            SocketServer socketServer = new SocketServer();
            socketServer.Conne();
            Console.ReadKey();
        }    
    }
    class SocketServer
    {
         delegate void UpdateReceiveMsgCallback(string msg);

        public delegate void UpdateConnectedClientListCallback();
        private System.Collections.ArrayList workerSocketList = ArrayList.Synchronized(new System.Collections.ArrayList());

         int clientCount = 0;

         AsyncCallback pfnWorkerCallBack;
         Socket mainSocket;
        int tb_ServerPort =8000;
        List<IPAddress> ipAddressList = new List<IPAddress>();
        public SocketServer()
        {
          
            //初始化
            InitializeInfo();
            StartListen();
        }

      

        private void InitializeInfo()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipAddr in ipHost.AddressList)
            {
                if (ipAddr.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddressList.Add(ipAddr);
                }
            }   
        }
        private void StartListen()
        {
            IPAddress ipAddress = ipAddressList.First() as IPAddress;
            //创建监听Socket
            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //邦定IP
            IPEndPoint ipLocal = new IPEndPoint(ipAddress, tb_ServerPort);
            mainSocket.Bind(ipLocal);

            //开始监听
            mainSocket.Listen(4);

            //创建Call Back为任意客户端连接
            mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }

        private void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                // 创建一个新的 Socket 
                Socket workerSocket = mainSocket.EndAccept(asyn);

                // 递增客户端数目
                Interlocked.Increment(ref clientCount);

                // 添加到客户端数组中
                workerSocketList.Add(workerSocket);

                //发送一个消息
                string msg = "Welcome 客户端 " + clientCount + "\n";
                SendMsgToClient(msg, clientCount);

                //刷新已连接的客户端列表
                RefreshConnectedClientList();
            
                //指定这个Socket处理接收到的数据
                WaitForData(workerSocket, clientCount);

                // Main Socket继续等待客户端的连接
                mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                

              
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine ("\n 客户端连接: Socket 已关闭\n");
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

       public void Conne()
        {
            while (true)
            {
                var str = Console.ReadLine();
                SendMsgToClient(str, clientCount);
            }
        }
        //发送消息给客户端
        private void SendMsgToClient(string msg, int clientNumber)
        {
            byte[] byData = System.Text.Encoding.UTF8.GetBytes(msg);

            Socket workerSocket = (Socket)workerSocketList[clientNumber - 1];
            workerSocket.Send(byData);
            Console.WriteLine("向客户端广播 内容 ==> " + msg);
        }
        //等待客户端的数据
        private void WaitForData(System.Net.Sockets.Socket skt, int clientNumber)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceived);

                SocketPacket theSocPkt = new SocketPacket(skt, clientNumber);
                skt.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
        public void OnDataReceived(IAsyncResult asyn)
        {
            SocketPacket socketData = (SocketPacket)asyn.AsyncState;
            try
            {
                int iRx = socketData.currSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];

                System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = decoder.GetChars(socketData.dataBuffer, 0, iRx, chars, 0);

                System.String szData = new System.String(chars);

                Console.WriteLine (Environment.NewLine + "Client " + socketData.clientNO + " Data:" + new System.String(chars));

                //For Debug
                //string replyMsg = "Server 回复:" + szData.ToUpper();

                string replyMsg = "Server 回复: 接收完成";
                byte[] byData = System.Text.Encoding.UTF8.GetBytes(replyMsg);

                Socket workerSocket = (Socket)socketData.currSocket;
                workerSocket.Send(byData);

                WaitForData(socketData.currSocket, socketData.clientNO);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n 数据接收时: Socket 已关闭\n");
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10054) // 连接被管道重置
                {
                    string msg = "Client " + socketData.clientNO + " 断开连接" + "\n";
                  

                    workerSocketList[socketData.clientNO - 1] = null;
                  
                }
                else
                    MessageBox.Show(se.Message);
            }
        }

        private void RefreshConnectedClientList()
        {
            Console.WriteLine("当前连接:" + workerSocketList.Count);
        
        }
    }
    internal class SocketPacket
    {
        public System.Net.Sockets.Socket currSocket;
        public int clientNO;

        public byte[] dataBuffer = new byte[8192];

        public SocketPacket(System.Net.Sockets.Socket socket, int clientNumber)
        {
            currSocket = socket;
            clientNO = clientNumber;
        }
    }

    #region
    /*
         //创建服务器对象，默认监听本机0.0.0.0，端口12345
            SocketServer server = new SocketServer(12345);

            //处理从客户端收到的消息
            server.HandleRecMsg = new Action<byte[], SocketConnection, SocketServer>((bytes, client, theServer) =>
            {
                string msg = Encoding.UTF8.GetString(bytes);
                Console.WriteLine($"收到消息:{msg}");
                Console.WriteLine("输入:quit关闭客户端，输入其它消息发送到客户端");
            });

            //处理服务器启动后事件
            server.HandleServerStarted = new Action<SocketServer>(theServer =>
            {
                Console.WriteLine("服务已启动************");
            });

            //处理新的客户端连接后的事件
            server.HandleNewClientConnected = new Action<SocketServer, SocketConnection>((theServer, theCon) =>
            {
                Console.WriteLine($@"一个新的客户端接入，当前连接数：{theServer.ClientList.Count}");
            });

            //处理客户端连接关闭后的事件
            server.HandleClientClose = new Action<SocketConnection, SocketServer>((theCon, theServer) =>
            {
                Console.WriteLine($@"一个客户端关闭，当前连接数为：{theServer.ClientList.Count}");
            });

            //处理异常
            server.HandleException = new Action<Exception>(ex =>
            {
                Console.WriteLine(ex.Message);
            });

            //服务器启动
            server.StartServer();

            while (true)
            {
                Console.WriteLine("输入:quit，关闭服务器");
                string op = Console.ReadLine();
                if (op == "quit")
                {
                    break;
                }                    
                else
                {
                    server.Send(op);
                }
            }
     */
    #endregion
}
