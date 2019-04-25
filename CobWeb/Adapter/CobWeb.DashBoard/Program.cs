using CobWeb.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Process[] ps = Process.GetProcessesByName("CobWeb");


            
            var path = Path.GetFullPath(cobwebPath);

            var process = Process.Start(path + "CobWeb.exe");
            
            httpPostRequest.Prefixes.Add("http://127.0.0.1:30000/");
            httpPostRequest.Start();

           
            Task.Run(() =>
            {
                httpPostRequestHandle();
            });
            Application.Run(new FormDashboard());
        }
        private static HttpListener httpPostRequest = new HttpListener();
        public static string cobwebPath = @"..\..\..\..\CobWeb\bin\Debug\";

        private static void httpPostRequestHandle()
        {
            while (true)
            {
                HttpListenerContext requestContext = httpPostRequest.GetContext();
                Thread threadsub = new Thread(new ParameterizedThreadStart((requestcontext) =>
                {
                    var context = (HttpListenerContext)requestcontext;

                   
                    if (context.Request.ContentType!= "application/json")
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
                        Task.Run(() =>
                        {

                        });

                        SetResponse(context.Response, 200, new { data = "ok", msg = requestBody });
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

        static void SetResponse(HttpListenerResponse response,int statusCode, object obj)
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
}
