using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CobWeb.Util.FlashLog
{
    public static class FlashLogSetting
    {
        /// <summary>
        /// 单个日志的最大值
        /// </summary>
        public  static int Max_LogSize = 1024 * 1000;
    }
    /// <summary>
    /// 日志等级
    /// </summary>
    public enum FlashLogLevel
    {
        Debug,
        Info,
        Error,
        Warn,
        Fatal
    }
    /// <summary>
    /// 日志内容
    /// </summary>
    public class FlashLogMessage
    {
        /// <summary>
        /// 业务名
        /// </summary>
        public string Service { get; set; }
        public string Message { get; set; }
        public FlashLogLevel Level { get; set; }
        public Exception Exception { get; set; }
    }
}
