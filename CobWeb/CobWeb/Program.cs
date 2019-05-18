using CobWeb.AProcess;
using CobWeb.Browser;
using CobWeb.Core;
using CobWeb.Core.Control;
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
            Init.Step1_Default();
            Init.Step2_GlobalException();
            var browserType = string.Empty;
            var port = 6666;
            var number = 0;
            FormBrowser formBrowser = null;
            string[] cmdArgs = Environment.GetCommandLineArgs();
            if (cmdArgs.Length > 0)
            {
                var param_str = cmdArgs.Where(m => m.Contains("cob") && m.Contains("|")).FirstOrDefault();
                if (param_str!=null)
                {
                    var param = param_str.Split('|');
                    if (param.Length > 1)
                    {
                        browserType = param[1];
                    }
                    if (param.Length > 2)
                    {
                        port = int.Parse(param[2]);
                    }
                    if (param.Length > 3)
                    {
                        number = int.Parse(param[3]);
                    }
                }
            }
            switch (browserType)
            {
                case "cef":
                    formBrowser = new CEF_Form(new CefKernelControl("about:blank"));
                    break;
                case "ie":
                    formBrowser = new IEForm(new TridentKernelControl());
                    break;
                case "webkit":
                    formBrowser = new WebKitForm(new WebKitKernelControl());
                    break;
                default:
                    formBrowser = new CEF_Form(new CefKernelControl("about:blank"));
                    break;
            }
            ProcessControl.FormBrowser = formBrowser;
            ProcessControl processControl = new ProcessControl()
            {
                Number = number,
                Port = port
            };
            processControl.StartListen_Core();
            var pro = Process.GetCurrentProcess();
            Application.Run(formBrowser);
        }
    }
}
