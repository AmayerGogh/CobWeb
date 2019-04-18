using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util.Control
{
    public interface IBrowserBase
    {
        void Navigate(string url);
        void Refresh();
        IBrowserBase GetBrowser();
        bool IsDisposed();
        void Dispose();
    }
}
