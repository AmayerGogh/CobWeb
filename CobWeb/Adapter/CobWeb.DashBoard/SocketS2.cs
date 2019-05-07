//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace CobWeb.DashBoard
//{
//    //https://www.cnblogs.com/zjoch/p/4175291.html
//    //https://blog.csdn.net/sqldebug_fan/article/details/17557341
//    public class AsyncSocketServer
//    {
//        Socket listenSocket;
//        private int m_numConnections; //最大支持连接个数
//        private int m_receiveBufferSize; //每个连接接收缓存大小
//        private Semaphore m_maxNumberAcceptedClients; //限制访问接收连接的线程数，用来控制最大并发数
//        private DaemonThread m_daemonThread;
//        public void Start(IPEndPoint localEndPoint)
//        {
//            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//            listenSocket.Bind(localEndPoint);
//            listenSocket.Listen(6666);
           
//            StartAccept(null);
//            m_daemonThread = new DaemonThread(this);
//        }

//        public void StartAccept(SocketAsyncEventArgs acceptEventArgs)
//        {
//            if (acceptEventArgs == null)
//            {
//                acceptEventArgs = new SocketAsyncEventArgs();
//                acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
//            }
//            else
//            {
//                acceptEventArgs.AcceptSocket = null; //释放上次绑定的Socket，等待下一个Socket连接
//            }

//            m_maxNumberAcceptedClients.WaitOne(); //获取信号量
//            bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArgs);
//            if (!willRaiseEvent)
//            {
//                ProcessAccept(acceptEventArgs);
//            }
//        }

//        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs acceptEventArgs)
//        {
//            try
//            {
//                ProcessAccept(acceptEventArgs);
//            }
//            catch (Exception E)
//            {
//               // Program.Logger.ErrorFormat("Accept client {0} error, message: {1}", acceptEventArgs.AcceptSocket, E.Message);
//              //  Program.Logger.Error(E.StackTrace);
//            }
//        }
//        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
//        {
//            SetText(string.Format( "Client connection accepted. Local Address: {0}, Remote Address: {1}",
//                acceptEventArgs.AcceptSocket.LocalEndPoint, acceptEventArgs.AcceptSocket.RemoteEndPoint));

//            AsyncSocketUserToken userToken = m_asyncSocketUserTokenPool.Pop();
//            m_asyncSocketUserTokenList.Add(userToken); //添加到正在连接列表
//            userToken.ConnectSocket = acceptEventArgs.AcceptSocket;
//            userToken.ConnectDateTime = DateTime.Now;

//            try
//            {
//                bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
//                if (!willRaiseEvent)
//                {
//                    lock (userToken)
//                    {
//                        ProcessReceive(userToken.ReceiveEventArgs);
//                    }
//                }
//            }
//            catch (Exception E)
//            {
//                Program.Logger.ErrorFormat("Accept client {0} error, message: {1}", userToken.ConnectSocket, E.Message);
//                Program.Logger.Error(E.StackTrace);
//            }

//            StartAccept(acceptEventArgs); //把当前异步事件释放，等待下次连接
//        }
//        void SetText(string msg)
//        {

//        }

//    }


//    class DaemonThread : Object
//    {
//        private Thread m_thread;
//        private AsyncSocketServer m_asyncSocketServer;

//        public DaemonThread(AsyncSocketServer asyncSocketServer)
//        {
//            m_asyncSocketServer = asyncSocketServer;
//            m_thread = new Thread(DaemonThreadStart);
//            m_thread.Start();
//        }

//        public void DaemonThreadStart()
//        {
//            while (m_thread.IsAlive)
//            {
//                AsyncSocketUserToken[] userTokenArray = null;
//                m_asyncSocketServer.AsyncSocketUserTokenList.CopyList(ref userTokenArray);
//                for (int i = 0; i < userTokenArray.Length; i++)
//                {
//                    if (!m_thread.IsAlive)
//                        break;
//                    try
//                    {
//                        if ((DateTime.Now - userTokenArray[i].ActiveDateTime).Milliseconds > m_asyncSocketServer.SocketTimeOutMS) //超时Socket断开
//                        {
//                            lock (userTokenArray[i])
//                            {
//                                m_asyncSocketServer.CloseClientSocket(userTokenArray[i]);
//                            }
//                        }
//                    }
//                    catch (Exception E)
//                    {
//                        Program.Logger.ErrorFormat("Daemon thread check timeout socket error, message: {0}", E.Message);
//                        Program.Logger.Error(E.StackTrace);
//                    }
//                }

//                for (int i = 0; i < 60 * 1000 / 10; i++) //每分钟检测一次
//                {
//                    if (!m_thread.IsAlive)
//                        break;
//                    Thread.Sleep(10);
//                }
//            }
//        }

//        public void Close()
//        {
//            m_thread.Abort();
//            m_thread.Join();
//        }
//    }

//}
