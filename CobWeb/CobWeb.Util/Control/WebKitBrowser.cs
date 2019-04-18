using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebKit;

namespace CobWeb.Util.Control
{
     public  class MyWebKitBrowser: WebKitBrowser,IBrowserBase
    {
        public MyWebKitBrowser()
        {
            
        }
        public IBrowserBase GetBrowser()
        {
            return this;
        }

        bool IBrowserBase.IsDisposed()
        {
            throw new NotImplementedException();
        }
    }
}
