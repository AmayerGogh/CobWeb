using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CobWeb.Util.Model
{
    public class SocketResponseModel
    {
        public SocketResponseCode StateCode { get; set; }
        public string ErrMsg { get; set; }
        public string Result { get; set; }
    }

    public enum SocketResponseCode
    {

        /// <summary>
        /// 未知方法名
        /// </summary>
        A_UnknownMethod = -99998,

        /// <summary>
        /// 超时时间为零
        /// </summary>
        A_TimeOutZero = -99997,

        /// <summary>
        /// 人工模拟超时未能得到执行结果
        /// </summary>
        A_TimeOutResult = -99996,

        /// <summary>
        /// 系统繁忙,请稍后再试
        /// </summary>
        A_SystemBusy = -99995,

        /// <summary>
        /// 标记更换一个进程执行
        /// </summary>
        A_ChangeProcess = -99994,

        /// <summary>
        /// 请求被意外中断
        /// </summary>
        A_RequestAccidentBreak = -99993,

        /// <summary>
        /// 
        /// </summary>
        A_RequestNormalBreak = -99992,

        /// <summary>
        /// 内核错误
        /// </summary>
        A_KernelError = -99990,
        /// <summary>
        /// 序列化失败
        /// </summary>
        A_JsonError = -99989,
        /// <summary>
        /// 成功
        /// </summary>
        A_Success = 200,
        /// <summary>
        /// 未知异常
        /// </summary>        
        A_UnknownException = 500

    }




}
