using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CobWeb.Core
{
    /// <summary>
    /// 浏览器内核基类
    /// </summary>
    public interface IKernelControl
    {      
        void Navigate(string url);
        void Refresh();
        IKernelControl GetBrowser();
        bool IsDisposed();
        void Dispose();
    }
}
