using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.Core.Control
{
    public class TridentKernelControl : WebBrowser, IKernelControl
    {
        public IKernelControl GetBrowser()
        {
            return null;
        }
     

        bool IKernelControl.IsDisposed()
        {
            return false;
        }
    }
}
