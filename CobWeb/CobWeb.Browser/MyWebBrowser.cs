using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Browser
{
   public  class MyWebBrowser: ChromiumWebBrowser
    {
        public MyWebBrowser(string address, IRequestContext requestContext = null) :base(address,requestContext)
        {

        }
        public MyWebBrowser()
        {

        }
    }
}
