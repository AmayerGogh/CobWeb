using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
namespace CobWeb.Util
{
    public class SocketBasic
    {
        // Token: 0x06000014 RID: 20 RVA: 0x00002AA4 File Offset: 0x00000CA4
        public static Socket GetSocket(out IPEndPoint ipe, int port, string ip = "0")
        {
            if (ip == "0")
            {
                ipe = new IPEndPoint(IPAddress.Any, port);
            }
            else
            {
                ipe = new IPEndPoint(IPAddress.Parse(ip), port);
            }
            uint num = 0u;
            byte[] array = new byte[Marshal.SizeOf(num) * 3];
            BitConverter.GetBytes(1u).CopyTo(array, 0);
            BitConverter.GetBytes(15000u).CopyTo(array, Marshal.SizeOf(num));
            BitConverter.GetBytes(15000u).CopyTo(array, Marshal.SizeOf(num) * 2);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //检查是否掉线
            socket.IOControl((IOControlCode)(-1744830460), array, null);
            return socket;
        }
        // Token: 0x06000015 RID: 21 RVA: 0x00002B50 File Offset: 0x00000D50
        public static bool Connect(Socket socket, IPEndPoint ipe, int timeout = 0)
        {
            if (timeout == 0)
            {
                while (!socket.Connected)
                {
                    try
                    {
                        socket.Connect(ipe);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(5000);
                    }
                }
                return true;
            }
            IAsyncResult asyncResult = socket.BeginConnect(ipe, null, null);
            asyncResult.AsyncWaitHandle.WaitOne(timeout * 1000);
            return socket.Connected;
        }
        // Token: 0x06000016 RID: 22 RVA: 0x00002BC0 File Offset: 0x00000DC0
        public static string Receive(Socket socket, int timeout = 0, int receiveTimeout = 60)
        {
            long ticks = DateTime.Now.Ticks;
            socket.ReceiveTimeout = timeout * 1000;
            List<byte> list = new List<byte>();
            byte[] array = new byte[8192];
            for (; ; )
            {
                int num;
                if ((num = socket.Receive(array)) > 0)
                {
                    for (int i = 0; i < num; i++)
                    {
                        list.Add(array[i]);
                    }
                    if (num >= array.Length)
                    {
                        continue;
                    }
                }
                if (SocketBasic.IsComplete(list))
                {
                    break;
                }
                if (list.Count == 0)
                {
                    goto Block_3;
                }
                if (timeout > 0)
                {
                    int timeout2 = SocketBasic.GetTimeout(ticks, timeout);
                    if (timeout2 <= 0)
                    {
                        goto Block_5;
                    }
                    socket.ReceiveTimeout = timeout2 * 1000;
                }
                else
                {
                    timeout = receiveTimeout;
                    socket.ReceiveTimeout = timeout * 1000;
                }
            }
            return Encoding.Default.GetString(list.ToArray(), 0, list.Count);
        Block_3:
            throw new Exception("连接被中断");
        Block_5:
            throw new Exception("数据未能完整获取");
        }
        // Token: 0x06000017 RID: 23 RVA: 0x00002C9C File Offset: 0x00000E9C
        public static int Send(Socket socket, string msg, int timeout = 0)
        {
            socket.SendTimeout = timeout * 1000;
            byte[] bytes = Encoding.Default.GetBytes(msg + "#Qpp");
            int num = socket.Send(bytes, bytes.Length, SocketFlags.None);
            if (num == bytes.Length)
            {
                return num;
            }
            throw new Exception("请求数据发送失败");
        }
        // Token: 0x06000018 RID: 24 RVA: 0x00002CEC File Offset: 0x00000EEC
        public static bool IsComplete(List<byte> data)
        {
            if (data.Count >= 4 && data[data.Count - 4] == 35 && data[data.Count - 3] == 81 && data[data.Count - 2] == 112 && data[data.Count - 1] == 112)
            {
                for (int i = 0; i < 4; i++)
                {
                    data.RemoveAt(data.Count - 1);
                }
                return true;
            }
            return false;
        }
        // Token: 0x06000019 RID: 25 RVA: 0x00002D68 File Offset: 0x00000F68
        public static int GetTimeout(long starttime, int timeout)
        {
            return (int)((long)timeout - (DateTime.Now.Ticks - starttime) / 10000L / 1000L);
        }
        // Token: 0x0600001A RID: 26 RVA: 0x00002D96 File Offset: 0x00000F96
        public SocketBasic()
        {
        }
    }
}
