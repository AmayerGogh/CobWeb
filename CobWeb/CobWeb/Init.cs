using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb
{
    public static class Init
    {
        public const string CefLibName = "CEFSharp"; //cef目录名称
        public  static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);
            var assemblyAllName = assemblyName.Name + ".dll";
            //加载CefSharp相关库 
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyPath = Path.Combine(Application.StartupPath, CefLibName, assemblyAllName);
                return File.Exists(assemblyPath) ? Assembly.LoadFile(assemblyPath) : null;
            } //判断程序集的区域性 
            if (!assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture))
            {
                assemblyAllName = string.Format(@"{0}\{1}", assemblyName.CultureInfo, assemblyAllName);
            }
            using (Stream stream = executingAssembly.GetManifestResourceStream(assemblyAllName))
            {
                if (stream == null) return null;
                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }
        public static void Step1_InitializeCefSetting()
        {
            string appPath = Application.StartupPath;
            CefSettings settings = new CefSettings
            {
                Locale = "zh-CN", //中文 
                BrowserSubprocessPath = Path.Combine(appPath, CefLibName, "CefSharp.BrowserSubprocess.exe"),//设置浏览器子程序启动路径 
                
            };
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;//启用CEF中和网页的JS交互
            Cef.Initialize(settings);

        }
    }
}
