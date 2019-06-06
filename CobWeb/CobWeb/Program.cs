﻿using CobWeb.AProcess;
using CobWeb.Browser;
using CobWeb.Core;
using CobWeb.Core.Control;
using CobWeb.Util;
using CobWeb.Util.Model;
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
        static void Main(string[] args)
        {           
            //AppDomain.CurrentDomain.AssemblyResolve += Init.OnResolveAssembly;
            Init.Step1_Default();
            Init.Step2_GlobalException();
            var browserType = string.Empty;
            var port = 6666;
            var number = 0;
            FormBrowser formBrowser = null;
            if (args!=null && args.Length > 0)
            {
                var param_str = args.Where(m => m.Contains("cob") && m.Contains("|")).FirstOrDefault();
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
            if (browserType == SocketKernelType.CefSharp)
            {
                formBrowser = new CEF_Form(new CefKernelControl("about:blank"));
            }
            else if(browserType == SocketKernelType.Webkit)
            {
                formBrowser = new WebKitForm(new WebKitKernelControl());
            }
            else if (browserType == SocketKernelType.IE)
            {
                formBrowser = new IEForm(new TridentKernelControl());
            }
            else
            {
                formBrowser = new CEF_Form(new CefKernelControl("about:blank"));
            }          
            ProcessControl.FormBrowser = formBrowser;
            ProcessControl processControl = new ProcessControl(browserType, number,port){};
            processControl.StartListen_Core();
            var pro = Process.GetCurrentProcess();
            Application.Run(formBrowser);
        }
    }
}
