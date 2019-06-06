using CobWeb.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.DashBoard
{

    public class HttpListenerContextModel
    {      
        public string Id { get; set; }
        public HttpListenerContext Context { get; set; }
        public DateTime CreateTime { get; set; }

    }

    public static class HttpListenerContext_Pool
    {

        static Dictionary<string, HttpListenerContextModel> _HttpContext = new Dictionary<string, HttpListenerContextModel>();
        static readonly object _lock = new object();
        public static bool Add(HttpListenerContextModel model)
        {
           
            if (!model.Id.IsNotNullOrEmpty())
            {
                return false;
            }
            lock (_lock)
            {
                _HttpContext.Add(model.Id, model);
            }
            return true;
        }
             
        public static void DoWhileTest()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    HttpListenerContextModel single = null;
                    lock (_lock)
                    {
                        var item = _HttpContext.FirstOrDefault();
                        if (item.Key.IsNotNullOrEmpty())
                        {

                            //todo
                            //var data = new {
                            //    data = item.Value.Id,
                            //    msg = new {
                            //        header = item.Value.Context.Request.Headers.ToString()
                            //    }
                            //};
                            SetResponse(item.Value.Context.Response, 200, item.Value.Context.Request.Headers.ToString());
                            _HttpContext.Remove(item.Key);
                        }
                    }
                    Thread.Sleep(1000);
                }
            }));
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Name = "http";
            thread.Start();
        }

        public static bool SetOk(string key, object obj)
        {
            if (!key.IsNotNullOrEmpty())
            {
                return false;
            }
            HttpListenerContextModel single = null;
            lock (_lock)
            {
                single = _HttpContext[key];
                _HttpContext.Remove(key);
            }
            Task.Factory.StartNew(() =>
            {
                SetResponse(single.Context.Response, 200, obj);
                single = null;
            });
         
            return true;
        }
        public static void SetError(string key, object obj)
        {

        }

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

}
