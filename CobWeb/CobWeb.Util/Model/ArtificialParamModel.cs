using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CobWeb.Util.Model
{
    public class ArtificialParamModel
    {
        /// <summary>
        /// 访问接口
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
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
        public string StopKey { get; set; }
        /// <summary>
        /// 是否使用窗口
        /// </summary>
        public bool IsUseForm { get; set; }
    }
}
