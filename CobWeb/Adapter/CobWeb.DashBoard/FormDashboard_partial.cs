using CobWeb.Util;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.DashBoard
{
    public partial class FormDashboard
    {
        /// <summary>
        /// 当前的 进程对应类
        /// </summary>
        static List<CobWeb_ProcessList> CobWeb_ProcessList = new List<CobWeb_ProcessList>();

        /// <summary>
        /// 
        /// </summary>
        ConcurrentDictionary<int, object> hashtable2 = new ConcurrentDictionary<int, object>();

        static List<Socket> sockets = new List<Socket>();
        void Refesh_dataGridView1()
        {
            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        try
            //        {
            //            Process[] ps = Process.GetProcesses();
            //            this.dataGridView1.BeginInvoke(new ThreadStart(delegate ()
            //            {
            //                this.dataGridView1.Rows.Clear();
            //            }));


            //            foreach (var item in ps)
            //            {                          

            //                DataGridViewRow row = new DataGridViewRow();
            //                DataGridViewTextBoxCell textboxcell = new DataGridViewTextBoxCell() { Value = item.Id };
            //                DataGridViewTextBoxCell textboxcell2 = new DataGridViewTextBoxCell() { };
            //                DataGridViewTextBoxCell textboxcell3 = new DataGridViewTextBoxCell() { Value = item.WorkingSet64 / (1024 * 1024) };
            //                DataGridViewTextBoxCell textboxcell4 = new DataGridViewTextBoxCell();
            //                DataGridViewTextBoxCell textboxcell5 = new DataGridViewTextBoxCell() { Value = item.Threads.Count };
            //                //DataGridViewTextBoxCell textboxcell6 = new datagr 
            //                var commands = string.Empty;
            //                //using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + item.Id))
            //                //using (var objects = searcher.Get())
            //                //{
            //                //    var @object = objects.Cast<ManagementBaseObject>().SingleOrDefault();
            //                //    commands = @object?["CommandLine"]?.ToString() ?? "";
            //                //}

            //                textboxcell2.Value = item.ProcessName + "----" + commands;

            //                //textboxcell4.Value = (item.TotalProcessorTime - TimeSpan.Zero).TotalMilliseconds / 1000 / Environment.ProcessorCount * 100;

            //                row.Cells.AddRange(textboxcell, textboxcell2, textboxcell3, textboxcell4, textboxcell5);


            //                this.dataGridView1.BeginInvoke(new ThreadStart(delegate ()
            //                {
            //                    this.dataGridView1.Rows.Add(row);
            //                }));

            //            }
            //        }
            //        finally
            //        {

            //        }

            //        Thread.Sleep(1000);
            //    }

            //});




        }




        void Step1_Init()
        {
            
        }
        //SocketManager server;

        IOCPServer server;
        void Step2_InnerListen()
        {
            server = new IOCPServer(6666, 4, this);
            server.Start();
            //server =  new SocketManager(66, 1024, this);
            //server.Start("127.0.0.1",6666);
        }
        void Step3_OutListen()
        {
            httpPostRequest.Prefixes.Add("http://127.0.0.1:30000/");
            httpPostRequest.Start();
            Task.Run(() =>
            {
                httpPostRequestHandle();
            });
        }
        void Step4_TimerStart()
        {
            System.Timers.Timer timer = new System.Timers.Timer()
            {
                Interval = 1000,
                Enabled = true,
            };
            timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) =>
            {
                if (textBox2.Text.Length > 200)
                {
                    textBox2.Text = "";
                }
                textBox2.Text += DateTime.Now.ToString() + Thread.CurrentThread.ManagedThreadId + "_" + Process.GetCurrentProcess().Threads.Count + Environment.NewLine;
                Thread.Sleep(100);
                Refesh_dataGridView1();
            });
        }



        private static HttpListener httpPostRequest = new HttpListener();
     

        public static Dictionary<int, HttpListenerContext> HttpContext = new Dictionary<int, HttpListenerContext>();
        private static void httpPostRequestHandle()
        {
            while (true)
            {
                HttpListenerContext requestContext = httpPostRequest.GetContext();

                Thread threadsub = new Thread(new ParameterizedThreadStart((requestcontext) =>
                {
                    var context = (HttpListenerContext)requestcontext;


                    if (context.Request.ContentType != "application/json")
                    {
                        SetResponse(context.Response, 500, new { data = "ContentType指定 application/json" });
                        return;
                    }
                    try
                    {
                        var requestBody = string.Empty;
                        using (var stream = context.Request.InputStream)
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            requestBody = reader.ReadToEnd();
                        }

                        Action<HttpListenerContext> action = m =>
                        {
                            SetResponse(context.Response, 200, new { data = "ok", msg = requestBody });
                        };

                        action.Invoke(context);


                    }
                    catch (Exception e)
                    {
                        SetResponse(context.Response, 500, new { data = "服务器内部错误" });
                    }


                    //var s = requestBody.DeserializeObject<dynamic>();


                    /*
                    //获取Post请求中的参数和值帮助类  
                    HttpListenerPostParaHelper httppost = new HttpListenerPostParaHelper(request);
                    //获取Post过来的参数和数据  
                    List<HttpListenerPostValue> lst = httppost.GetHttpListenerPostValue();
                    if (lst!=null)
                    {
                        string userName = "";
                        string password = "";
                        string suffix = "";
                        string adType = "";
                        //使用方法  
                        foreach (var key in lst)
                        {
                            if (key.type == 0)
                            {
                                string value = Encoding.UTF8.GetString(key.datas).Replace("\r\n", "");
                                if (key.name == "username")
                                {
                                    userName = value;
                                    Console.WriteLine(value);
                                }
                                if (key.name == "password")
                                {
                                    password = value;
                                    Console.WriteLine(value);
                                }
                                if (key.name == "suffix")
                                {
                                    suffix = value;
                                    Console.WriteLine(value);
                                }
                                if (key.name == "adtype")
                                {
                                    adType = value;
                                    Console.WriteLine(value);
                                }
                            }
                            if (key.type == 1)
                            {
                                string fileName = request.Request.QueryString["FileName"];
                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    string filePath = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyMMdd_HHmmss_ffff") + Path.GetExtension(fileName).ToLower();
                                    if (key.name == "File")
                                    {
                                        FileStream fs = new FileStream(filePath, FileMode.Create);
                                        fs.Write(key.datas, 0, key.datas.Length);
                                        fs.Close();
                                        fs.Dispose();
                                    }
                                }
                            }
                        }
                    }
                   */


                }));
                threadsub.Start(requestContext);
            }
        }
        //todo
        //http://www.cnblogs.com/ysyn/p/3399351.html
        static void SetResponse(HttpListenerResponse response, int statusCode, object obj)
        {
            response.StatusCode = statusCode;
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(obj.SerializeObject());
            response.ContentLength64 = buffer.Length;
            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }

    }
    public class CobWeb_ProcessList
    {
        public int Id { get; set; }
        public string StartInfo { get; set; }
        public long WorkingSet64 { get; set; }
        public long Cpu { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? LastWorkingEndTime { get; set; }
        public bool IsWorking { get; set; }
        public DateTime? CurrentWorkingStartTime { get; set; }

        public int SocketPort { get; set; }
    }
}
