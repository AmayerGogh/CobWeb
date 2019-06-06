using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CobWeb.Util.Model
{
    public class SocketRequestModel
    {

        public string Port { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        [Obsolete]
        public object Param { get; set; }
        /// <summary>
        /// 用于超时
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// ticks格式
        /// 用于超时
        /// </summary>
        public long StartTime { get; set; }
        /// <summary>
        /// 唯一标识的key
        /// 用于必要时的中止
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 是否使用窗口
        /// </summary>
        public bool IsUseForm { get; set; }
        /// <summary>
        /// 浏览器内核
        /// </summary>
        public string KernelType { get; set; }

        /// <summary>
        /// 包名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 访问接口
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Context { get; set; }
        /// <summary>
        /// 标志头
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// (暂缺)
        /// </summary>
        public string Access_token { get; set; }
      
    }
    public class SocketRequestHeader
    {

        [Description("使用窗口的爬虫")]
        public static string UserFormSpider = "FSp.";
        [Description("不使用窗口的爬虫")]
        public static string NoUserFormSpider = "NFSp";
      
        [Description("基本信息")]
        public static string Heartbeat = "Beat";
        [Description("执行指令")]
        public static string Command = "CMD.";

        public static HashSet<string> Head = new HashSet<string>();
        static SocketRequestHeader()
        {
            Head.Add(UserFormSpider);
            Head.Add(NoUserFormSpider);
          
            Head.Add(Heartbeat);
            Head.Add(Command);
        }
    }
    public class SocketKernelType
    {

        
        public static string CefSharp = "cef";
      
        public static string IE = "ie";       
       
        public static string Webkit = "webkit";
     
    }
   

}
