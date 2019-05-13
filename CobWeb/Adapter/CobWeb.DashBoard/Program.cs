using CobWeb.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        static FormDashboard FormDashboard;
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

        /// <summary>
        /// 当前的 进程对应类
        /// </summary>
        public static List<CobWeb_ProcessList> CobWeb_ProcessList = new List<CobWeb_ProcessList>();


        static void Step1_Init()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
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
        }
        static void Socket_OnConnection(string msg)
        {
            FormDashboard?.SetText($"连接:{msg}");
        }
        static void Socket_OnClose(string msg)
        {
            FormDashboard?.SetText($"关闭:{msg}");
        }
        static void Socket_OnError(string msg)
        {
            FormDashboard?.SetText($"error:{msg}");
        }
        static void Step3_OutListen()
        {
            HttpListenerOut.Prefixes.Add("http://localhost:30000/");
            HttpListenerOut.Start();
            Task.Run(() =>
            {
                //httpPostRequestHandle();
                HttpListenerOut.BeginGetContext(new AsyncCallback(GetContextCallBack), HttpListenerOut);
            });
            //HttpListenerContextModel_Pool.DoWhileTest();
        }

        private static HttpListener HttpListenerOut = new HttpListener();
        static void GetContextCallBack(IAsyncResult ar)
        {
            HttpListenerOut = ar.AsyncState as HttpListener;
            HttpListenerContext context = HttpListenerOut.EndGetContext(ar);

            HttpListenerContextModel_Pool.Add(new HttpListenerContextModel()
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




    }

    public class FormAccessManager
    {
       
        //void ShowLogForm()
        //{
        //    if (_formLog != null && !_formLog.IsDisposed)
        //    {
        //        if (_formLog.Visible == true)
        //        {
        //            _formLog.Activate();
        //            return;
        //        }
        //    }
        //    Thread newThread = new Thread(new ThreadStart(() =>
        //    {
        //        try
        //        {
        //            FormLog.ShowDialog();
        //        }
        //        finally
        //        {
        //            FormLog.Close();
        //        }
        //    }));
        //    newThread.Name = "FormLog";
        //    newThread.SetApartmentState(ApartmentState.STA);
        //    newThread.IsBackground = true; //随主线程一同退出
        //    newThread.Start();
        //}
    }
}
