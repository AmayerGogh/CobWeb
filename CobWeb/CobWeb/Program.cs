
using CobWeb.AProcess;
using CobWeb.Browser;
using CobWeb.Core;
using CobWeb.Core.Control;
using CobWeb.Web.Manager;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread] 
        static void Main()
        {          

            //AppDomain.CurrentDomain.AssemblyResolve += Init.OnResolveAssembly;
           // Init.Step1_InitializeCefSetting();
            Init.Step2_GlobalException();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ie
            //var browser = new TridentKernelControl();
            //var formBrowser = new IEForm(browser);

            //cefsharp
            var browser = new CefKernelControl("about:blank");
            var formBrowser = new CEF_Form(browser);

            //暂时不用
            //weibkit
            //var browser = new WebKitKernelControl();
            //var formBrowser = new WebKitForm(browser);

            ProcessControl.FormBrowser = formBrowser;
            Application.Run(formBrowser);
          
        }


      



      

    }
}
