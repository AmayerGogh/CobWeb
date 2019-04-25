using CobWeb.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace CobWeb.Util.SocketHelper
{
    public  class SocketAccess
    {
        static readonly Object ObjLock = new Object();
        static Mutex _mutex = null;
        static int _portNumber = 0;
        public static int GetPort()
        {
            lock (ObjLock)
            {
                if (_mutex == null)
                {
                    _portNumber = 9999;
                    while (_portNumber < 65000)
                    {
                        bool createNew;
                        _mutex = new Mutex(true, string.Format("SocketAccess.GetPort.{0}", _portNumber), out createNew);
                        if (!createNew)
                        {
                            _portNumber += 1000;
                            _mutex.Dispose();
                            _mutex = null;
                        }
                        else
                        {
                            _portNumber += 2; //兼容老的
                            break;
                        }
                    }
                }
                while (_mutex != null)
                {
                    var tempPort = _portNumber;
                    _portNumber += 2;
                    if (CheckPort(tempPort))
                        return tempPort;
                }
                return 0;
            }
        }
        /// <summary>
        /// 检测端口是否可用
        /// </summary>
        static bool CheckPort(int port)
        {
            //每个进程占用两个端口
            Socket serverSocket1 = null;
            Socket serverSocket2 = null;
            try
            {
                IPEndPoint ipe1;
                serverSocket1 = SocketBasic.GetSocket(out ipe1, port);
                serverSocket1.Bind(ipe1);
                IPEndPoint ipe2;
                serverSocket2 = SocketBasic.GetSocket(out ipe2, port + 1);
                serverSocket2.Bind(ipe2);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (serverSocket1 != null)
                    serverSocket1.Dispose();
                if (serverSocket2 != null)
                    serverSocket2.Dispose();
            }
        }
        public static T2 Access<T1, T2>(string method, T1 param, long starttime, int timeout, string stopkey, int port, bool isUseForm)
        {
            Socket socket = null;
            try
            {
                IPEndPoint ipe;
                socket = SocketBasic.GetSocket(out ipe, port, "127.0.0.1");
                SocketBasic.Connect(socket, ipe, timeout / 2);
                if (!socket.Connected)
                    throw new Exception("socket 连接失败");
                var paramModel = new ArtificialParamModel();
                paramModel.Method = method;
                paramModel.IsUseForm = isUseForm;
                paramModel.Param = param;
                paramModel.Timeout = timeout;
                paramModel.StartTime = starttime;
                paramModel.StopKey = !string.IsNullOrWhiteSpace(stopkey) ? stopkey : Guid.NewGuid().ToString();
                string dataParam = paramModel.SerializeObject();
                string result = string.Empty;
                try
                {
                    SocketBasic.Send(socket, dataParam, timeout / 2);
                    result = SocketBasic.Receive(socket, timeout, timeout / 2);
                }
                catch
                {
                    throw new Exception("socket接收失败");
                }
                var resultModel = result.DeserializeObject<ArtificialResultModel>();
                if (!resultModel.IsSuccess)
                    throw new Exception(resultModel.Result);
                try
                {
                    //如果返回的不是对应返回类型,则可能是想抛出此异常
                    return resultModel.Result.DeserializeObject<T2>();
                }
                catch
                {
                    throw new Exception(resultModel.Result);
                }
            }
            finally
            {
                if (socket != null)
                    socket.Dispose();
            }
        }
    }
}
