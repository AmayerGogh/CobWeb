using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Core
{
    /// <summary>
    /// 不需要窗口的接口
    /// </summary>
    public interface IProcessBase2
    {
        /// <summary>
        /// 执行
        /// </summary>
        string Excute(object param);
    }
}
