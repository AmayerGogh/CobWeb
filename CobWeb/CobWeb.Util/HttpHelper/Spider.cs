using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util.HttpHelper
{
    public class Spider
    {

        public event EventHandler<OnStartEventArgs> OnStart;//爬虫启动事件

        public event EventHandler<OnCompletedEventArgs> OnCompleted;//爬虫完成事件

        public event EventHandler<OnErrorEventArgs> OnError;//爬虫出错事件

        //public CookieContainer CookiesContainer { get; set; }//定义Cookie容器

        /// <summary>
        /// 异步创建爬虫
        /// </summary>
        /// <param name="uri">爬虫URL地址</param>
        /// <param name="proxy">代理服务器</param>
        /// <returns>网页源代码</returns>
        public async Task<string> StartAsync(SpiderRequestParam param)
        {
            return await Task.Run(() =>
            {
                var pageSource = string.Empty;
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(param.Uri);
                    if (param.CookieContainer.Count == 0)
                    {
                        request.CookieContainer = CookieHelper.CookieStr2CookieContainer(param.Cookies, param.Uri.ToString());
                    }
                    else
                    {
                        request.CookieContainer = param.CookieContainer;
                    }
                    if (this.OnStart != null) this.OnStart(this, new OnStartEventArgs(param.Uri));
                    var watch = new Stopwatch();
                    watch.Start();
                    request.ServicePoint.Expect100Continue = false;//加快载入速度
                    request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                    request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度      
                    request.AllowAutoRedirect = false;//禁止自动跳转                   
                    request.ProtocolVersion = HttpVersion.Version11;
                    request.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数
                    request.KeepAlive = true;

                    request.Timeout = param.Timeout;//定义请求超时时间为5秒 

                    if (param.Heads != null)
                    {
                        foreach (var item in param.Heads)
                        {
                            switch (item.Key)
                            {
                                case HttpRequestHeader.ContentType:
                                    break;
                                case HttpRequestHeader.Connection: //todo
                                    request.KeepAlive = true;
                                    break;
                                case HttpRequestHeader.Host:
                                    request.Host = item.Value;
                                    break;
                                default:
                                    request.Headers[item.Key.ToString()] = item.Value;
                                    break;
                            }
                        }
                    }



                    // //HttpRequestHeader.ho
                    // //request.Host = "localhost";
                    // //SetHeaderValue(request.Headers, "Host", "ithome.com"); //host不行
                    // request.Headers["UserAgent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/617.0";
                    // //SetHeaderValue(request.Headers, "UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/617.0");
                    // //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0";
                    // SetHeaderValue(request.Headers, "Headers", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    // //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    // SetHeaderValue(request.Headers, "AcceptEncoding", "gzip, deflate");
                    // //request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                    // SetHeaderValue(request.Headers, "AcceptLanguage", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                    // //request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                    // SetHeaderValue(request.Headers, "ContentType", "text/html; charset=utf-8");
                    // //request.ContentType = "text/html; charset=utf-8";//定义文档类型及编码
                    // //SetHeaderValue(request.Headers, "Method", "POST");
                    // SetHeaderValue(request.Headers, "TEST", "POST");                
                    // request.Method = "GET";//定义请求方式为GET
                    //// SetHeaderValue(request.Headers, "Connection", "Keep-Alive"); //不行




                    if (param.Proxy != null) request.Proxy = new WebProxy(param.Proxy);//设置代理服务器IP，伪装请求地址

                    //byte[] data = Encoding.Default.GetBytes("{\"UniqueKey\":\"" + "temp" + "\",\"CodeType\":\"5004\",\"Base64\":\"" + "" + "\"}");
                    //request.ContentType = "application/json";
                    //request.ContentLength = data.Length;

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        string cookie_str = response.Headers.Get("Set-Cookie");
                        Console.WriteLine(cookie_str);
                        //foreach (Cookie cookie in response.Cookies)
                        //{
                        //    this.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态
                        //}
                        if (response.ContentEncoding.ToLower().Contains("gzip"))//解压
                        {
                            using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else if (response.ContentEncoding.ToLower().Contains("deflate"))//解压
                        {
                            using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }

                            }
                        }
                        else
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                pageSource = reader.ReadToEnd();
                            }
                        }
                    }
                    request.Abort();
                    watch.Stop();
                    var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                    var milliseconds = watch.ElapsedMilliseconds;//获取请求执行时间
                    if (this.OnCompleted != null) this.OnCompleted(this, new OnCompletedEventArgs(param.Uri, threadId, milliseconds, pageSource));
                }
                catch (Exception ex)
                {
                    if (this.OnError != null) this.OnError(this, new OnErrorEventArgs(param.Uri, ex));
                }
                return pageSource;
            });
        }




        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
    }
    /// <summary>
    /// 爬虫启动事件
    /// </summary>
    public class OnStartEventArgs
    {
        public Uri Uri { get; set; }// 爬虫URL地址

        public OnStartEventArgs(Uri uri)
        {
            this.Uri = uri;
        }
    }

    public class OnErrorEventArgs
    {
        public Uri Uri { get; set; }

        public Exception Exception { get; set; }

        public OnErrorEventArgs(Uri uri, Exception exception)
        {
            this.Uri = uri;
            this.Exception = exception;
        }
    }
    /// <summary>
    /// 爬虫完成事件
    /// </summary>
    public class OnCompletedEventArgs
    {
        public Uri Uri { get; private set; }// 爬虫URL地址
        public int ThreadId { get; private set; }// 任务线程ID
        public string PageSource { get; private set; }// 页面源代码
        public long Milliseconds { get; private set; }// 爬虫请求执行事件
        public OnCompletedEventArgs(Uri uri, int threadId, long milliseconds, string pageSource)
        {
            this.Uri = uri;
            this.ThreadId = threadId;
            this.Milliseconds = milliseconds;
            this.PageSource = pageSource;
        }
    }


    public class SpiderRequestParam
    {
        public Dictionary<HttpRequestHeader, string> Heads { get; set; }
        public Uri Uri { get; set; }
        public string Proxy { get; set; }

        public string Cookies { get; set; }
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();
        public int Timeout { get; set; } = 5000;
    }
}
