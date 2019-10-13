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
        public enum MethodType
        {
           Get,
           Post
        }

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
        public async Task<string> SendAsync(SpiderRequestParam param)
        {
            return await Task.Run(() =>
            {
                return Send(param);               
            });
        }

        public string Send(SpiderRequestParam param)
        {
            var pageSource = string.Empty;
            try
            {
                HttpWebRequest request = null;
                if (param.Method== MethodType.Get)
                {
                    request = (HttpWebRequest)WebRequest.Create($"{param.Url.ToString()}?{param.GetParam}");
                }
                else
                {
                    request = (HttpWebRequest)WebRequest.Create(param.Url);
                }
               
                if (param.CookieContainer.Count == 0 && !string.IsNullOrWhiteSpace(param.Cookies))
                {
                    request.CookieContainer = CookieHelper.CookieStr2CookieContainer(param.Cookies, param.Url.Host);
                }
                else
                {
                    request.CookieContainer = param.CookieContainer;
                }
                if (this.OnStart != null) this.OnStart(this, new OnStartEventArgs(param.Url));
                var watch = new Stopwatch();
                watch.Start();
                request.ServicePoint.Expect100Continue = false;//加快载入速度
                request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度      
                request.AllowAutoRedirect = param.AllowAutoRedirect;//禁止自动跳转                   
                request.ProtocolVersion = HttpVersion.Version11;
                request.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数
                request.KeepAlive = true;
                request.Timeout = param.Timeout;//定义请求超时时间为5秒 
                request.Method = param.Method.ToString();
                //request.SendChunked =true;
                if (param.Proxy != null) request.Proxy = new WebProxy(param.Proxy);
                //fiddler
                //request.Proxy = new WebProxy("http://127.0.0.1:8888", true);
                if (param.Heads != null)
                {
                    foreach (var item in param.Heads)
                    {
                        switch (item.Key)
                        {
                            case HttpRequestHeader.ContentType:
                                request.ContentType = item.Value;
                                break;
                            case HttpRequestHeader.Connection: //todo
                                request.KeepAlive = true;
                                break;
                            case HttpRequestHeader.Host:
                                request.Host = item.Value;
                                break;
                            case HttpRequestHeader.Accept:
                                request.Accept = item.Value;
                                break;
                            case HttpRequestHeader.Referer:
                                request.Referer = item.Value;
                                break;
                            case HttpRequestHeader.UserAgent: //非常有必要
                                request.UserAgent = item.Value;
                                break;
                            default:
                                request.Headers[item.Key.ToString()] = item.Value;
                                break;
                        }
                    }
                }

                if (param.Url.Scheme =="https")
                {
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((sender,cen,chain,err)=> {
                        return true;
                    });
                    request.ProtocolVersion = HttpVersion.Version10;
                    
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                if (param.Method==MethodType.Post && param.PostData.IsNotNullOrEmpty())
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(param.PostData);

                    request.ContentLength = buffer.Length;
                    using (Stream req = request.GetRequestStream())
                    {
                        req.Write(buffer, 0, buffer.Length);
                    }
                }             

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    string cookie_str = response.Headers.Get("Set-Cookie");
                    string cookie2 = response.Headers.Get("SetCookie");

                    Encoding encoding = null;
                    if (string.IsNullOrEmpty(response.ContentType))
                    {
                        encoding = Encoding.Default;
                    }
                    else if (response.ContentType.Contains("utf-8"))
                    {
                        encoding = Encoding.UTF8;
                    }
                    else
                    {
                        encoding = Encoding.Default;
                    }


                    Console.WriteLine(cookie_str);
                    //foreach (Cookie cookie in response.Cookies)
                    //{
                    //    this.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态
                    //}
                    if (response.ContentEncoding.ToLower().Contains("gzip"))//解压
                    {
                        using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(stream, encoding))
                            {
                                pageSource = reader.ReadToEnd();
                            }
                        }
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))//解压
                    {
                        using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(stream, encoding))
                            {
                                pageSource = reader.ReadToEnd();
                            }

                        }
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                        {
                            pageSource = reader.ReadToEnd();
                        }
                    }
                }
                request.Abort();
                watch.Stop();
                var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                var milliseconds = watch.ElapsedMilliseconds;//获取请求执行时间
                if (this.OnCompleted != null) this.OnCompleted(this, new OnCompletedEventArgs(param.Url, threadId, milliseconds, pageSource));
            }
            catch (Exception ex)
            {
               // throw ex;
                if (this.OnError != null) this.OnError(this, new OnErrorEventArgs(param.Url, ex));
            }
            return pageSource;
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
        public Uri Url { get; set; }
        public string Proxy { get; set; }
        public Spider.MethodType Method { get; set; }
        public string Cookies { get; set; }
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();
        public int Timeout { get; set; } = 5000;
        public bool AllowAutoRedirect { get; set; }

        public string PostData { get; set; }
        public string GetParam { get; set; }
    }
}
