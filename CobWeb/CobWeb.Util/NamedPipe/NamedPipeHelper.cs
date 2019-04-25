using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace CobWeb.Util.NamedPipe
{
    public class PipeServerHelp
    {
        private static int numThreads = 1;
        /// <summary>
        /// 启动管道服通讯务器
        /// </summary>
        public static void PipeSeverSart()
        {
            try
            {
                int i;
                Thread[] servers = new Thread[numThreads];
                Console.WriteLine("Waiting for client connect...\n");
                for (i = 0; i < numThreads; i++)
                {
                    servers[i] = new Thread(ServerThread);
                    servers[i].Start();
                }
            }
            catch (Exception)
            {
                throw new Exception("管道服务启动失败.");
                //PipeSeverSart();
            }
        }
        /// <summary>
        /// 退出管道。（程序退出时别忘了调用）
        /// </summary>
        public static void PipeSeverClose()
        {
            if (pipeServer != null)
            {
                try
                {
                    pipeServer.Disconnect();
                    pipeServer.Close();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
        private static NamedPipeServerStream pipeServer;
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="data"></param>
        private static void ServerThread(object data)
        {
            try
            {
                Random reRandom = new Random();
                pipeServer = new NamedPipeServerStream("VisualPlatformPipe", PipeDirection.InOut, numThreads);
                //int threadId = Thread.CurrentThread.ManagedThreadId;
                // Wait for a client to connect
                pipeServer.WaitForConnection();
                StreamString ss = new StreamString(pipeServer);
                //Console.WriteLine("Client connected on thread[{0}].", threadId);
                while (true)
                {
                    try
                    {
                        // Read the request from the client. Once the client has
                        // written to the pipe its security token will be available.
                        //ss.WriteString("I am the one true server!");
                        string temp = ss.ReadString();
                        if (temp != null)
                        {
                            if (temp.ToLower() == "close")//客户端通知服务器客户端退出
                            {
                                //为了客户端退出之后，可以再次连接到服务器端，重新设置一下服务i其管道
                                Close();
                                pipeServer = new NamedPipeServerStream("VisualPlatformPipe", PipeDirection.InOut, numThreads);
                                pipeServer.WaitForConnection();
                                ss = new StreamString(pipeServer);
                            }
                            else
                            {
                                StringToStream fileReader = new StringToStream(ss, "我是数据");
                                pipeServer.RunAsClient(fileReader.Start);
                            }
                        }
                        else////客户端未通知服务器客户端退出，客户端直接异常退出
                        {
                            Close();
                            pipeServer = new NamedPipeServerStream("VisualPlatformPipe", PipeDirection.InOut, numThreads);
                            pipeServer.WaitForConnection();
                            ss = new StreamString(pipeServer);
                        }
                    }
                    // Catch the IOException that is raised if the pipe is broken
                    // or disconnected.
                    catch (IOException e)
                    {
                        Console.WriteLine("ERROR: {0}", e.Message);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("管道服务启动失败.");
            }
        }
        /// <summary>
        /// 退出管道
        /// </summary>
        public static void Close()
        {
            if (pipeServer != null)
            {
                pipeServer.Disconnect();
                pipeServer.Close();
            }
        }
    }
    public class PipeClientHelp
    {
        private static StreamString m_StreamString = null;
        private static NamedPipeClientStream pipeClient = null;
        /// <summary>
        /// 启动客户端，连接服务器，只允许连接一次
        /// </summary>
        /// <returns></returns>
        public static bool StartConnection()
        {
            try
            {
                if (pipeClient == null)
                {
                    pipeClient = new NamedPipeClientStream(".", "VisualPlatformPipe",PipeDirection.InOut, PipeOptions.None,TokenImpersonationLevel.Impersonation);
                    pipeClient.Connect(100);
                    m_StreamString = new StreamString(pipeClient);
                }
            }
            catch (Exception exception)
            {
                pipeClient = null;
                throw new Exception("未启动服务器端" + exception.Message);
            }
            return true;
        }
        /// <summary>
        /// 通知服务器客户端即将退出
        /// </summary>
        public static void ClosePipe()
        {
            if (pipeClient != null)
            {
                m_StreamString.WriteString("close");
                m_StreamString = null;
                pipeClient.Close();
                pipeClient = null;
            }
        }
        /// <summary>
        /// 从服务器获取数据
        /// </summary>
        /// <returns></returns>
        public static string GetSystemID()
        {
            if (m_StreamString != null)
            {
                m_StreamString.WriteString("GetBusinessSystemId");
                return m_StreamString.ReadString();
            }
            return null;
        }
    }
}
