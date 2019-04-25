using CobWeb.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.DashBoard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            httpPostRequest.Prefixes.Add("http://127.0.0.1:30000/test/");
            httpPostRequest.Start();

            Thread ThrednHttpPostRequest = new Thread(new ThreadStart(httpPostRequestHandle));
            ThrednHttpPostRequest.Start();

            Application.Run(new FormDashboard());
        }
        private static HttpListener httpPostRequest = new HttpListener();


        private static void httpPostRequestHandle()
        {
            while (true)
            {
                HttpListenerContext requestContext = httpPostRequest.GetContext();
                Thread threadsub = new Thread(new ParameterizedThreadStart((requestcontext) =>
                {
                    HttpListenerContext request = (HttpListenerContext)requestcontext;

                    //获取Post请求中的参数和值帮助类  
                    HttpListenerPostParaHelper httppost = new HttpListenerPostParaHelper(request);
                    //获取Post过来的参数和数据  
                    List<HttpListenerPostValue> lst = httppost.GetHttpListenerPostValue();

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

                    //Response  
                    request.Response.StatusCode = 200;
                    request.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    request.Response.ContentType = "application/json";
                    requestContext.Response.ContentEncoding = Encoding.UTF8;
                    var data = new { success = "true", msg = "提交成功" };
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(data.SerializeObject());
                    request.Response.ContentLength64 = buffer.Length;
                    var output = request.Response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }));
                threadsub.Start(requestContext);
            }
        }
    }
}
