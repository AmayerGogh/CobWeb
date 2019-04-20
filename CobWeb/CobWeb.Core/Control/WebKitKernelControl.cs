using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebKit;

namespace CobWeb.Core.Control
{
     public  class WebKitKernelControl : WebKitBrowser,IKernelControl
    {
        public WebKitKernelControl ()
        {
            
        }
        public IKernelControl GetBrowser()
        {
            return this;
        }

        bool IKernelControl.IsDisposed()
        {
            throw new NotImplementedException();
        }
    }
}
