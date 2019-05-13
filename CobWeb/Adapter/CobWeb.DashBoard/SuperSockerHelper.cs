
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CobWeb.DashBoard
{

    class AsyncUserToken
    {
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// 远程地址
        /// </summary>
        public EndPoint Remote { get; set; }

        /// <summary>
        /// 通信SOKET
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPassWord { get; set; }


        /// <summary>
        /// 数据缓存区
        /// </summary>
        public List<byte> Buffer { get; set; }


        public AsyncUserToken()
        {
            this.Buffer = new List<byte>();
        }


    }
    class SocketEventPool
    {
        Stack<SocketAsyncEventArgs> m_pool;


        public SocketEventPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) { throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null"); }
            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }

        // Removes a SocketAsyncEventArgs instance from the pool
        // and returns the object removed from the pool
        public SocketAsyncEventArgs Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }

        // The number of SocketAsyncEventArgs instances in the pool
        public int Count
        {
            get { return m_pool.Count; }
        }

        public void Clear()
        {
            m_pool.Clear();
        }
    }

    class SocketManager
    {

        private int m_maxConnectNum;    //最大连接数
        private int m_revBufferSize;    //最大接收字节数
        BufferManager m_bufferManager;
        const int opsToAlloc = 2;
        Socket listenSocket;            //监听Socket
        SocketEventPool m_pool;
        int m_clientCount;              //连接的客户端数量
        /// <summary>
        /// 用于服务器执行的互斥同步对象
        /// </summary>
        private static Mutex mutex = new Mutex();
        Semaphore m_maxNumberAcceptedClients;

        List<AsyncUserToken> m_clients; //客户端列表

        #region 定义委托

        /// <summary>
        /// 客户端连接数量变化时触发
        /// </summary>
        /// <param name="num">当前增加客户的个数(用户退出时为负数,增加时为正数,一般为1)</param>
        /// <param name="token">增加用户的信息</param>
        public delegate void OnClientNumberChange(int num, AsyncUserToken token);

        /// <summary>
        /// 接收到客户端的数据
        /// </summary>
        /// <param name="token">客户端</param>
        /// <param name="buff">客户端数据</param>
        public delegate void OnReceiveData(AsyncUserToken token, byte[] buff);

        #endregion

        #region 定义事件
        /// <summary>
        /// 客户端连接数量变化事件
        /// </summary>
        public event OnClientNumberChange ClientNumberChange;

        /// <summary>
        /// 接收到客户端的数据事件
        /// </summary>
        public event OnReceiveData ReceiveClientData;

      
        #endregion

        #region 定义属性

        /// <summary>
        /// 获取客户端列表
        /// </summary>
        public List<AsyncUserToken> ClientList { get { return m_clients; } }

        #endregion
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numConnections">最大连接数</param>
        /// <param name="receiveBufferSize">缓存区大小</param>
        public SocketManager(int numConnections, int receiveBufferSize)
        {
            m_clientCount = 0;
            m_maxConnectNum = numConnections;
            m_revBufferSize = receiveBufferSize;
            // allocate buffers such that the maximum number of sockets can have one outstanding read and 
            //write posted to the socket simultaneously  
            m_bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToAlloc, receiveBufferSize);

            m_pool = new SocketEventPool(numConnections);
            m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
           
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds 
            // against memory fragmentation
            m_bufferManager.InitBuffer();
            m_clients = new List<AsyncUserToken>();
            // preallocate pool of SocketAsyncEventArgs objects
            SocketAsyncEventArgs readWriteEventArg;

            for (int i = 0; i < m_maxConnectNum; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.UserToken = new AsyncUserToken();

                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object
                m_bufferManager.SetBuffer(readWriteEventArg);
                // add SocketAsyncEventArg to the pool
                m_pool.Push(readWriteEventArg);
            }
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="ipstr"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Start(string ipstr, int port)
        {
            try
            {
                // 获得主机相关信息
                IPAddress iPAddress = IPAddress.Parse(ipstr);
                IPEndPoint localEndPoint = new IPEndPoint(iPAddress, port);

                // 创建监听socket
                this.listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.listenSocket.ReceiveBufferSize = this.m_revBufferSize;
                //this.listenSocket.SendBufferSize = this.bufferSize;

                if (localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    // 配置监听socket为 dual-mode (IPv4 & IPv6) 
                    // 27 is equivalent to IPV6_V6ONLY socket option in the winsock snippet below,
                    this.listenSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                    this.listenSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, localEndPoint.Port));
                }
                else
                {
                    this.listenSocket.Bind(localEndPoint);
                }

                // 开始监听
                this.listenSocket.Listen(this.m_maxConnectNum);

                // 在监听Socket上投递一个接受请求。
                this.StartAccept(null);

                // Blocks the current thread to receive incoming messages.
                mutex.WaitOne();
                ////////////////////////////
                return true;


            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            foreach (AsyncUserToken token in m_clients)
            {
                try
                {
                    token.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception) { }
            }
            try
            {
                listenSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }

            listenSocket.Close();
            int c_count = m_clients.Count;
            lock (m_clients) { m_clients.Clear(); }

            if (ClientNumberChange != null)
                ClientNumberChange(-c_count, null);
            mutex.ReleaseMutex();
        }


        public void CloseClient(AsyncUserToken token)
        {
            try
            {
                token.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
        }


        // Begins an operation to accept a connection request from the client 
        //
        // <param name="acceptEventArg">The context object to use when issuing 
        // the accept operation on the server's listening socket</param>
        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                // socket must be cleared since the context object is being reused
                acceptEventArg.AcceptSocket = null;
            }

            m_maxNumberAcceptedClients.WaitOne();
            if (!listenSocket.AcceptAsync(acceptEventArg))
            {
                ProcessAccept(acceptEventArg);
            }
        }

        // This method is the callback method associated with Socket.AcceptAsync 
        // operations and is invoked when an accept operation is complete
        //
        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                Interlocked.Increment(ref m_clientCount);
                // Get the socket for the accepted client connection and put it into the 
                //ReadEventArg object user token
                SocketAsyncEventArgs readEventArgs = m_pool.Pop();
                AsyncUserToken userToken = (AsyncUserToken)readEventArgs.UserToken;
                userToken.Socket = e.AcceptSocket;
                userToken.ConnectTime = DateTime.Now;
                userToken.Remote = e.AcceptSocket.RemoteEndPoint;
                userToken.IPAddress = ((IPEndPoint)(e.AcceptSocket.RemoteEndPoint)).Address;
                //  Socket st = e.UserToken as Socket;

                lock (m_clients) { m_clients.Add(userToken); }
                SetResponse(String.Format("客户 {0} 连入, 共有 {1} 个连接。", e.AcceptSocket.RemoteEndPoint.ToString(), m_clients.Count()));
                if (ClientNumberChange != null)
                    ClientNumberChange(1, userToken);
                if (!e.AcceptSocket.ReceiveAsync(readEventArgs))
                {

                    ProcessReceive(readEventArgs);
                }
                SetResponse(userToken.Remote.ToString() + "连接成功！");
                
            }
            catch (Exception me)
            {
                SetResponse( me.Message + "\r\n" + me.StackTrace);
            }

            // Accept the next connection request
            if (e.SocketError == SocketError.OperationAborted) return;
            StartAccept(e);
        }


        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }

        }


        // This method is invoked when an asynchronous receive operation completes. 
        // If the remote host closed the connection, then the socket is closed.  
        // If data was received then the data is echoed back to the client.
        //
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            string rstr = null;
            try
            {
                // check if the remote host closed the connection
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                {
                    //读取数据
                    byte[] data = new byte[e.BytesTransferred];
                    Array.Copy(e.Buffer, e.Offset, data, 0, e.BytesTransferred);
                    lock (token.Buffer)
                    {
                        token.Buffer.AddRange(data);
                    }
                    //注意:你一定会问,这里为什么要用do-while循环? 
                    //如果当客户发送大数据流的时候,e.BytesTransferred的大小就会比客户端发送过来的要小,
                    //需要分多次接收.所以收到包的时候,先判断包头的大小.够一个完整的包再处理.
                    //如果客户短时间内发送多个小数据包时, 服务器可能会一次性把他们全收了.
                    //这样如果没有一个循环来控制,那么只会处理第一个包,
                    //剩下的包全部留在token.Buffer中了,只有等下一个数据包过来后,才会放出一个来.
                    do
                    {
                        //判断包的长度
                        byte[] lenBytes = token.Buffer.GetRange(0, 4).ToArray();
                        int packageLen = BitConverter.ToInt32(lenBytes, 0);
                        if (packageLen > token.Buffer.Count - 4)
                        {   //长度不够时,退出循环,让程序继续接收
                            break;
                        }

                        //包够长时,则提取出来,交给后面的程序去处理
                        byte[] rev = token.Buffer.GetRange(4, packageLen).ToArray();
                        rstr = rstr + System.Text.Encoding.Default.GetString(rev);
                     
                        SetResponse(String.Format("收到 {0} 数据为 {1}", null, rstr));
                        SendMessage(token, rev);


                        //从数据池中移除这组数据
                        lock (token.Buffer)
                        {
                            token.Buffer.RemoveRange(0, packageLen + 4);
                        }
                        //将数据包交给后台处理,这里你也可以新开个线程来处理.加快速度.
                        if (ReceiveClientData != null)
                            ReceiveClientData(token, rev);
                        //这里API处理完后,并没有返回结果,当然结果是要返回的,却不是在这里, 这里的代码只管接收.
                        //若要返回结果,可在API处理中调用此类对象的SendMessage方法,统一打包发送.不要被微软的示例给迷惑了.
                    } while (token.Buffer.Count > 4);

                    //   

                    //继续接收. 为什么要这么写,请看Socket.ReceiveAsync方法的说明
                    if (!token.Socket.ReceiveAsync(e))
                        this.ProcessReceive(e);
                }
                else
                {
                    CloseClientSocket(e);
                }
            }
            catch (Exception xe)
            {
                SetResponse( xe.Message + "\r\n" + xe.StackTrace);
            }
        }
        public void Send(string msg, Socket socket)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            // Send(SocketAsyncEventArgs e, byte[] data)

            Send(socket, data, 0, data.Length, 100);
        }

        /// <summary>
        /// 同步的使用socket发送数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="timeout"></param>
        public void Send(Socket socket, byte[] buffer, int offset, int size, int timeout)
        {
            socket.SendTimeout = 0;
            int startTickCount = Environment.TickCount;
            int sent = 0; // how many bytes is already sent
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                {
                    //throw new Exception("Timeout.");
                }
                try
                {
                    sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                    ex.SocketErrorCode == SocketError.IOPending ||
                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably full, wait and try again
                        Thread.Sleep(30);
                    }
                    else
                    {
                        throw ex; // any serious error occurr
                    }
                }
            } while (sent < size);
        }

        // This method is invoked when an asynchronous send operation completes.  
        // The method issues another receive on the socket to read any additional 
        // data sent from the client
        //
        // <param name="e"></param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // done echoing data back to the client
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                // read the next block of data send from the client
                bool willRaiseEvent = token.Socket.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        //关闭客户端
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;

            lock (m_clients) { m_clients.Remove(token); }
            //如果有事件,则调用事件,发送客户端数量变化通知
            if (ClientNumberChange != null)
                ClientNumberChange(-1, token);
            // close the socket associated with the client
            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }
            token.Socket.Close();
            // decrement the counter keeping track of the total number of clients connected to the server
            Interlocked.Decrement(ref m_clientCount);
            m_maxNumberAcceptedClients.Release();
            // Free the SocketAsyncEventArg so they can be reused by another client
            e.UserToken = new AsyncUserToken();
            m_pool.Push(e);
        }



        /// <summary>
        /// 对数据进行打包,然后再发送
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public void SendMessage(AsyncUserToken token, byte[] message)
        {
            if (token == null || token.Socket == null || !token.Socket.Connected)
                return;
            try
            {
                //对要发送的消息,制定简单协议,头4字节指定包的大小,方便客户端接收(协议可以自己定)
                byte[] buff = new byte[message.Length + 4];
                byte[] len = BitConverter.GetBytes(message.Length);
                Array.Copy(len, buff, 4);
                Array.Copy(message, 0, buff, 4, message.Length);
                //token.Socket.Send(buff);  //这句也可以发送, 可根据自己的需要来选择
                //新建异步发送对象, 发送消息
                SocketAsyncEventArgs sendArg = new SocketAsyncEventArgs();
                sendArg.UserToken = token;
                sendArg.SetBuffer(buff, 0, buff.Length);  //将数据放置进去.
                token.Socket.SendAsync(sendArg);
            }
            catch (Exception e)
            {
                SetResponse( e.Message + "\r\n" + e.StackTrace);
            }
        }

        public void SetResponse(string msg)
        {
            //Console.WriteLine("notice:" + msg);
            //_baseForm.SetText(msg);
        }
    }

    class BufferManager
    {
        int m_numBytes;                 // the total number of bytes controlled by the buffer pool  
        byte[] m_buffer;                // the underlying byte array maintained by the Buffer Manager  
        Stack<int> m_freeIndexPool;     //   
        int m_currentIndex;
        int m_bufferSize;

        public BufferManager(int totalBytes, int bufferSize)
        {
            m_numBytes = totalBytes;
            m_currentIndex = 0;
            m_bufferSize = bufferSize;
            m_freeIndexPool = new Stack<int>();
        }

        // Allocates buffer space used by the buffer pool  
        public void InitBuffer()
        {
            // create one big large buffer and divide that   
            // out to each SocketAsyncEventArg object  
            m_buffer = new byte[m_numBytes];
        }

        // Assigns a buffer from the buffer pool to the   
        // specified SocketAsyncEventArgs object  
        //  
        // <returns>true if the buffer was successfully set, else false</returns>  
        public bool SetBuffer(SocketAsyncEventArgs args)
        {

            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        // Removes the buffer from a SocketAsyncEventArg object.    
        // This frees the buffer back to the buffer pool  
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }

}
