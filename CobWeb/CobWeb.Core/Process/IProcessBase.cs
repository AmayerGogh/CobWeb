using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CobWeb.Core
{
    /// <summary>
    /// 需要窗口的接口,同时需要继承ProcessBase类
    /// </summary>
    public interface IProcessBase
    {
        /// <summary>
        /// 开始执行
        /// </summary>
        void Begin();
        /// <summary>
        /// 结束执行,还必须调用父类的End()方法
        /// </summary>
        void End();
    }
}
