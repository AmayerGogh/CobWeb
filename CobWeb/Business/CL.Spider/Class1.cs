using CobWeb.Core;
using CobWeb.Core.Manager;
using CobWeb.Util.HttpHelper;
using CobWeb.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CL
{
    public class CL_Spider : ProcessBaseNotUseBrowser
    {
        public CL_Spider(SocketRequestModel paramModel) : base(paramModel)
        {

        }
        public List<string> dictionary = new List<string>();
        public List<string> Jobs = new List<string>();
        public object _obj = new object();
        public override string Excute(object param)
        {
            var res = string.Empty;
            try
            {
                var cityUrl = "https://co.2tj4.icu/thread0806.php";//定义爬虫入口URL
                var cityCrawler = new Spider();//调用刚才写的爬虫程序
                var heads = new Dictionary<HttpRequestHeader, string>() { };
                heads.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3837.0 Safari/537.36 Edg/77.0.211.3");
                heads.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                heads.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                heads.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                heads.Add(HttpRequestHeader.ContentType, "text/html; charset=utf-8");
              

                var param1 = new SpiderRequestParam()
                {
                    Url = new Uri(cityUrl),
                    Heads = heads,
                    Method = Spider.MethodType.Get,
                    AllowAutoRedirect = false,
                    
                };
                param1.GetParam = "fid=22&search=&page=2";
                cityCrawler.OnCompleted += (s, e) =>
                {

                    res = e.PageSource;

                    HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
                    hd.LoadHtml(res);
                    var nodes = hd.DocumentNode.SelectNodes("//h3//a");
                    if (nodes==null)
                    {
                        return ;
                    }
                    foreach (var item in nodes)
                    {
                        var href = item.Attributes["href"];
                        if (href.Value.Contains("htm_data/"))
                        {
                            Jobs.Add(href.Value);
                            Console.WriteLine(item.Attributes);
                        }                        
                    }
                };
                cityCrawler.SendAsync(param1).Wait();
                Do2();

            }
            catch (Exception)
            {


            }
            finally
            {

            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in dictionary)
            {
                sb.Append(item + Environment.NewLine);
            }
            LogManager.lc流程.Info(sb.ToString());
            Console.WriteLine("ok");
            return res;
        }

        private void Do2()
        {
            Task[] tasks = new Task[Jobs.Count];
            for (int i = 0; i < Jobs.Count; i++)
            {
                tasks[i] = Task.Factory.StartNew((str) =>
                {
                    var cityCrawler = new Spider();//调用刚才写的爬虫程序
                    var heads = new Dictionary<HttpRequestHeader, string>() { };
                    heads.Add(HttpRequestHeader.UserAgent, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    heads.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    heads.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                    heads.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                    heads.Add(HttpRequestHeader.ContentType, "text/html; charset=utf-8");

                    var url = "http://co.2tj4.icu/" + str as string ;
                    var param1 = new SpiderRequestParam()
                    {
                        Url = new Uri(url),
                        Heads = heads,
                        Method = Spider.MethodType.Get,
                        AllowAutoRedirect = true
                    };
                    cityCrawler.OnCompleted += (s, e) =>
                    {
                        HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
                        hd.LoadHtml(e.PageSource);
                        var nodes = hd.DocumentNode.SelectNodes("//div[@class='tpc_content do_not_catch']//a[@style='cursor:pointer']").FirstOrDefault();
                        if (nodes==null)
                        {
                            return;
                        }
                        var art = nodes.Attributes["onclick"]?.Value;
                        //getElementById('iframe1').src='http://www.f2dmb.me/player/823/#iframeload'
                        art = art.Replace("getElementById('iframe1').src='", string.Empty).Replace("'", string.Empty);
                        lock (_obj)
                        {
                            Console.WriteLine(art);
                            dictionary.Add(art);
                        }
                    };
                    cityCrawler.Send(param1);
                }, Jobs[i]);

            }
            Task.WaitAll(tasks);
        }
    }
}
