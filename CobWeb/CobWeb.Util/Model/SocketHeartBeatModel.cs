using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util.Model
{
    public class SocketHeartBeatModel
    {
        /// <summary>
        /// number
        /// </summary>
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public int Port { get; set; }

        public string BrowserType { get; set; }

        public long WorkingSet64 { get; set; }
        public DateTime StartTime { get; set; }

        public bool IsWorking { get; set; }
        public DateTime? LastWorkingEndTime { get; set; }
        public DateTime? CurrentWorkingStartTime { get; set; }
    }
}
