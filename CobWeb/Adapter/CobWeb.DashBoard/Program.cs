using CobWeb.Core.Manager;
using CobWeb.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.DashBoard
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            //var path = Path.GetFullPath(cobwebPath);
            //var process = Process.Start(path + "CobWeb.exe");

            Step1_Init();
            Step2_InnerListen();
            Step3_OutListen();
            FormDashboard = new FormDashboard();
            Application.Run(FormDashboard);
        }
        //
        public static FormDashboard FormDashboard;
        static FormAccess _formAccess;
        public static FormAccess FormAccess
        {
            get
            {
                if (_formAccess == null || _formAccess.IsDisposed)
                {
                    _formAccess = new FormAccess();
                }
                return _formAccess;
            }
        }
        public static string cobwebPath = @"..\..\..\..\CobWeb\bin\Debug\";

        static void Step1_Init()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //设置应用程序处理异常方式：ThreadException处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            AppDomain.CurrentDomain.FirstChanceException += new EventHandler<FirstChanceExceptionEventArgs>(CurrentDomain_FirstChanceException);
        }



        public static SocketServer server;
        static void Step2_InnerListen()
        {
            server = new SocketServer(6666, 4);
            server.OnRecive += Socket_OnRecive;
            server.OnConnection += Socket_OnConnection;
            server.OnClose += Socket_OnClose;
            server.OnError += Socket_OnError;
            server.Start();
        }
        static void Socket_OnRecive(string p, string msg)
        {
            FormDashboard?.SetText($"{p}接收:{msg}");
            _formAccess?.SetText($"{p}接收:{msg}");
        }
        static void Socket_OnConnection(string msg)
        {
            FormDashboard?.SetText($"连接:{msg}");
            _formAccess?.SetText($"连接:{msg}");
            _formAccess?.Refresh_ClientList();
        }
        static void Socket_OnClose(string msg)
        {
            FormDashboard?.SetText($"关闭:{msg}");
            _formAccess?.SetText($"关闭:{msg}");
            _formAccess?.Refresh_ClientList();
        }
        static void Socket_OnError(string msg)
        {
            FormDashboard?.SetText($"error:{msg}");
            _formAccess?.SetText($"error:{msg}");
        }
        static void Step3_OutListen()
        {
            HttpListenerOut.Prefixes.Add("http://localhost:30000/");
            HttpListenerOut.Start();
            Task.Run(() =>
            {
                HttpListenerOut.BeginGetContext(new AsyncCallback(GetContextCallBack), HttpListenerOut);
            });
            HttpListenerContext_Pool.DoWhileTest();

        }

        private static HttpListener HttpListenerOut = new HttpListener();
        static void GetContextCallBack(IAsyncResult ar)
        {
            HttpListenerOut = ar.AsyncState as HttpListener;
            HttpListenerContext context = HttpListenerOut.EndGetContext(ar);            
            HttpListenerContext_Pool.Add(new HttpListenerContextModel()
            {
                Id = Guid.NewGuid().ToString(),
                Context = context
            });
            //var requestBody = string.Empty;
            //using (var stream = context.Request.InputStream)
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    requestBody = reader.ReadToEnd();
            //}

            //Thread.Sleep(10000);
            //SetResponse(context.Response, 200, new { data = "ok", msg = requestBody });

            HttpListenerOut.BeginGetContext(new AsyncCallback(GetContextCallBack), HttpListenerOut);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = ExceptionHelper.GetExceptionMsg(e.Exception, e.ToString());
            LogManager.yc全局异常.Error(str);
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = ExceptionHelper.GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            LogManager.yc全局异常.Error(str);
        }
        static void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            var ee = e.Exception as Exception;
            string str = ExceptionHelper.GetExceptionMsg(ee, e.ToString());
            LogManager.yc全局异常.Error(str);
        }


    }

}
