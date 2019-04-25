using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util.SocketHelper
{
    /// <summary>
    /// Socket服务端
    /// </summary>
    public class SocketServer
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">监听的IP地址</param>
        /// <param name="port">监听的端口</param>
        public SocketServer(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        /// <summary>
        /// 构造函数,监听IP地址默认为本机0.0.0.0
        /// </summary>
        /// <param name="port">监听的端口</param>
        public SocketServer(int port)
        {
            _ip = "0.0.0.0";
            _port = port;
        }

        #endregion

        #region 内部成员

        private Socket _socket = null;
        private string _ip = "";
        private int _port = 0;
        private bool _isListen = true;
        private void StartListen()
        {
            try
            {
                _socket.BeginAccept(asyncResult =>
                {
                    try
                    {
                        Socket newSocket = _socket.EndAccept(asyncResult);

                        //马上进行下一轮监听,增加吞吐量
                        if (_isListen)
                            StartListen();

                        SocketConnection newClient = new SocketConnection(newSocket, this)
                        {
                            HandleRecMsg = HandleRecMsg == null ? null : new Action<byte[], SocketConnection, SocketServer>(HandleRecMsg),
                            HandleClientClose = HandleClientClose == null ? null : new Action<SocketConnection, SocketServer>(HandleClientClose),
                            HandleSendMsg = HandleSendMsg == null ? null : new Action<byte[], SocketConnection, SocketServer>(HandleSendMsg),
                            HandleException = HandleException == null ? null : new Action<Exception>(HandleException)
                        };

                        newClient.StartRecMsg();
                        ClientList.AddLast(newClient);

                        HandleNewClientConnected?.Invoke(this, newClient);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 开始服务，监听客户端
        /// </summary>
        public void StartServer()
        {
            try
            {
                //实例化套接字（ip4寻址协议，流式传输，TCP协议）
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //创建ip对象
                IPAddress address = IPAddress.Parse(_ip);
                //创建网络节点对象包含ip和port
                IPEndPoint endpoint = new IPEndPoint(address, _port);
                //将 监听套接字绑定到 对应的IP和端口
                _socket.Bind(endpoint);
                //设置监听队列长度为Int32最大值(同时能够处理连接请求数量)
                _socket.Listen(int.MaxValue);
                //开始监听客户端
                StartListen();
                HandleServerStarted?.Invoke(this);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        /// <summary>
        /// 所有连接的客户端列表
        /// </summary>
        public LinkedList<SocketConnection> ClientList { get; set; } = new LinkedList<SocketConnection>();

        /// <summary>
        /// 关闭指定客户端连接
        /// </summary>
        /// <param name="theClient">指定的客户端连接</param>
        public void CloseClient(SocketConnection theClient)
        {
            theClient.Close();
        }

        #endregion

        #region 公共事件

        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<Exception> HandleException { get; set; }

        #endregion

        #region 服务端事件

        /// <summary>
        /// 服务启动后执行
        /// </summary>
        public Action<SocketServer> HandleServerStarted { get; set; }

        /// <summary>
        /// 当新客户端连接后执行
        /// </summary>
        public Action<SocketServer, SocketConnection> HandleNewClientConnected { get; set; }

        /// <summary>
        /// 服务端关闭客户端后执行
        /// </summary>
        public Action<SocketServer, SocketConnection> HandleCloseClient { get; set; }

        #endregion

        #region 客户端连接事件

        /// <summary>
        /// 客户端连接接受新的消息后调用
        /// </summary>
        public Action<byte[], SocketConnection, SocketServer> HandleRecMsg { get; set; }

        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public Action<byte[], SocketConnection, SocketServer> HandleSendMsg { get; set; }

        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public Action<SocketConnection, SocketServer> HandleClientClose { get; set; }

        #endregion

        //我加的  这个不能用

        /// <summary>
        /// 发送字符串（默认使用UTF-8编码）
        /// </summary>
        /// <param name="msgStr">字符串</param>
        public void Send(string msgStr)
        {
            Send(Encoding.UTF8.GetBytes(msgStr));
        }

        /// <summary>
        /// 发送字符串（使用自定义编码）
        /// </summary>
        /// <param name="msgStr">字符串消息</param>
        /// <param name="encoding">使用的编码</param>
        public void Send(string msgStr, Encoding encoding)
        {
            Send(encoding.GetBytes(msgStr));
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bytes">数据字节</param>
        public void Send(byte[] bytes)
        {
            try
            {
                _socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndSend(asyncResult);
                        
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }
    }
    /// <summary>
    /// Socket连接,双向通信
    /// </summary>
    public class SocketConnection
    {
        #region 构造函数

        public SocketConnection(Socket socket, SocketServer server)
        {
            _socket = socket;
            _server = server;
        }

        #endregion

        #region 私有成员

        private readonly Socket _socket;
        private bool _isRec = true;
        private SocketServer _server = null;
        private bool IsSocketConnected()
        {
            bool part1 = _socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (_socket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 开始接受客户端消息
        /// </summary>
        public void StartRecMsg()
        {
            try
            {
                byte[] container = new byte[1024 * 1024 * 2];
                _socket.BeginReceive(container, 0, container.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndReceive(asyncResult);

                        //马上进行下一轮接受，增加吞吐量
                        if (length > 0 && _isRec && IsSocketConnected())
                            StartRecMsg();

                        if (length > 0)
                        {
                            byte[] recBytes = new byte[length];
                            Array.Copy(container, 0, recBytes, 0, length);

                            //处理消息
                            HandleRecMsg?.Invoke(recBytes, this, _server);
                        }
                        else
                            Close();
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                        Close();
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
                Close();
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bytes">数据字节</param>
        public void Send(byte[] bytes)
        {
            try
            {
                _socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndSend(asyncResult);
                        HandleSendMsg?.Invoke(bytes, this, _server);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        /// <summary>
        /// 发送字符串（默认使用UTF-8编码）
        /// </summary>
        /// <param name="msgStr">字符串</param>
        public void Send(string msgStr)
        {
            Send(Encoding.UTF8.GetBytes(msgStr));
        }

        /// <summary>
        /// 发送字符串（使用自定义编码）
        /// </summary>
        /// <param name="msgStr">字符串消息</param>
        /// <param name="encoding">使用的编码</param>
        public void Send(string msgStr, Encoding encoding)
        {
            Send(encoding.GetBytes(msgStr));
        }

        /// <summary>
        /// 传入自定义属性
        /// </summary>
        public object Property { get; set; }

        /// <summary>
        /// 关闭当前连接
        /// </summary>
        public void Close()
        {
            try
            {
                _isRec = false;
                _socket.Disconnect(false);
                _server.ClientList.Remove(this);
                HandleClientClose?.Invoke(this, _server);
                _socket.Close();
                _socket.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 客户端连接接受新的消息后调用
        /// </summary>
        public Action<byte[], SocketConnection, SocketServer> HandleRecMsg { get; set; }

        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public Action<byte[], SocketConnection, SocketServer> HandleSendMsg { get; set; }

        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public Action<SocketConnection, SocketServer> HandleClientClose { get; set; }

        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<Exception> HandleException { get; set; }

        #endregion
    }
    /// <summary>
    /// Socket客户端
    /// </summary>
    public class SocketClient
    {
        #region 构造函数

        /// <summary>
        /// 构造函数,连接服务器IP地址默认为本机127.0.0.1
        /// </summary>
        /// <param name="port">监听的端口</param>
        public SocketClient(int port)
        {
            _ip = "127.0.0.1";
            _port = port;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">监听的IP地址</param>
        /// <param name="port">监听的端口</param>
        public SocketClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        #endregion

        #region 内部成员

        private Socket _socket = null;
        private string _ip = "";
        private int _port = 0;
        private bool _isRec = true;
        private bool IsSocketConnected()
        {
            bool part1 = _socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (_socket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 开始接受客户端消息
        /// </summary>
        public void StartRecMsg()
        {
            try
            {
                byte[] container = new byte[1024 * 1024 * 2];
                _socket.BeginReceive(container, 0, container.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndReceive(asyncResult);

                        //马上进行下一轮接受，增加吞吐量
                        if (length > 0 && _isRec && IsSocketConnected())
                            StartRecMsg();

                        if (length > 0)
                        {
                            byte[] recBytes = new byte[length];
                            Array.Copy(container, 0, recBytes, 0, length);

                            //处理消息
                            HandleRecMsg?.Invoke(recBytes, this);
                        }
                        else
                            Close();
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                        Close();
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
                Close();
            }
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 开始服务，连接服务端
        /// </summary>
        public void StartClient()
        {
            try
            {
                //实例化 套接字 （ip4寻址协议，流式传输，TCP协议）
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //创建 ip对象
                IPAddress address = IPAddress.Parse(_ip);
                //创建网络节点对象 包含 ip和port
                IPEndPoint endpoint = new IPEndPoint(address, _port);
                //将 监听套接字  绑定到 对应的IP和端口
                _socket.BeginConnect(endpoint, asyncResult =>
                {
                    try
                    {
                        _socket.EndConnect(asyncResult);
                        //开始接受服务器消息
                        StartRecMsg();

                        HandleClientStarted?.Invoke(this);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bytes">数据字节</param>
        public void Send(byte[] bytes)
        {
            try
            {
                _socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndSend(asyncResult);
                        HandleSendMsg?.Invoke(bytes, this);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        /// <summary>
        /// 发送字符串（默认使用UTF-8编码）
        /// </summary>
        /// <param name="msgStr">字符串</param>
        public void Send(string msgStr)
        {
            Send(Encoding.UTF8.GetBytes(msgStr));
        }

        /// <summary>
        /// 发送字符串（使用自定义编码）
        /// </summary>
        /// <param name="msgStr">字符串消息</param>
        /// <param name="encoding">使用的编码</param>
        public void Send(string msgStr, Encoding encoding)
        {
            Send(encoding.GetBytes(msgStr));
        }

        /// <summary>
        /// 传入自定义属性
        /// </summary>
        public object Property { get; set; }

        /// <summary>
        /// 关闭与服务器的连接
        /// </summary>
        public void Close()
        {
            try
            {
                _isRec = false;
                _socket.Disconnect(false);
                HandleClientClose?.Invoke(this);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 客户端连接建立后回调
        /// </summary>
        public Action<SocketClient> HandleClientStarted { get; set; }

        /// <summary>
        /// 处理接受消息的委托
        /// </summary>
        public Action<byte[], SocketClient> HandleRecMsg { get; set; }

        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public Action<byte[], SocketClient> HandleSendMsg { get; set; }

        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public Action<SocketClient> HandleClientClose { get; set; }

        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<Exception> HandleException { get; set; }

        #endregion
    }
}
