using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.Core.Control
{
    public static class Cef_Setting
    {
        public static void Step1_InitializeCefSetting()
        {
            string appPath = Application.StartupPath;
            CefSettings settings = new CefSettings
            {
                Locale = "zh-CN", //中文 
                                  //BrowserSubprocessPath = Path.Combine(appPath, CefLibName, "CefSharp.BrowserSubprocess.exe"),//设置浏览器子程序启动路径 
            };
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;//启用CEF中和网页的JS交互
            Cef.Initialize(settings);
        }
    }
}
