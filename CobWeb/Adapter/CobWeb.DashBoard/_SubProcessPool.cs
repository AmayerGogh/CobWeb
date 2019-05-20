
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

    public class SocketClient_Pool
    {
        static Dictionary<string, SubClientModel> _socketClient = new Dictionary<string, SubClientModel>();
        static readonly object _lock = new object();
        public static void Add(string key, SubClientModel pro)
        {
            _socketClient.Add(key, pro);
        }
        public static void Remove(string key)
        {
            if (_socketClient.ContainsKey(key))
            {
                _socketClient.Remove(key);
            }
        }
        public static bool Contains(string key)
        {
            return _socketClient.ContainsKey(key);
        }
        public static SubClientModel Get(string key)
        {
            SubClientModel ret = null;
            if (_socketClient.ContainsKey(key))
            {
                ret = _socketClient[key];
            }
            return ret;
        }
        public static List<string> GetKeys()
        {
            var list = new List<string>();
            foreach (var item in _socketClient)
            {
                list.Add(item.Key);
            }
            return list;
        }
    }

    public class SubClientModel
    {
        public int ProcessId { get; set; }
        public string StartInfo { get; set; }
        public long WorkingSet64 { get; set; }
        public long Cpu { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? LastWorkingEndTime { get; set; }
        public bool IsWorking { get; set; }
        public DateTime? CurrentWorkingStartTime { get; set; }        
        /// <summary>
        /// 通信SOKET
        /// </summary>
        public Socket Socket { get; set; }
        /// <summary>
        /// 数据缓存区
        /// </summary>
        public List<byte> Buffer { get; set; }
        public SubClientModel()
        {
            this.Buffer = new List<byte>();
        }
    }
    
}
