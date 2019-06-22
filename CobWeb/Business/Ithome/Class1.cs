using CobWeb.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobWeb.Util.Model;
using CobWeb.Util.HttpHelper;
using System.Net;
using CobWeb.Core.Manager;

namespace Ithome
{
    public class IthomeSpider : ProcessBaseNotUseBrowser
    {
        public IthomeSpider(SocketRequestModel paramModel) : base(paramModel)
        {

        }
        public Dictionary<string, int> dictionary = new Dictionary<string, int>();
        public List<string> Jobs = new List<string>();
        public object _obj = new object();
        public override string Excute(object param)
        {
            var res = string.Empty;
            try
            {
                var cityUrl = "https://www.ithome.com/list/2019-06-04.html";//定义爬虫入口URL
                var cityCrawler = new Spider();//调用刚才写的爬虫程序
                var heads = new Dictionary<HttpRequestHeader, string>() { };
                heads.Add(HttpRequestHeader.UserAgent, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                heads.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                heads.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                heads.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                heads.Add(HttpRequestHeader.ContentType, "text/html; charset=utf-8");
                heads.Add(HttpRequestHeader.Host, "www.ithome.com");

                var param1 = new SpiderRequestParam()
                {
                    Uri = new Uri(cityUrl),
                    Heads = heads,
                    Method = Spider.MethodType.Get,
                    AllowAutoRedirect = true
                };
                cityCrawler.OnStart += (s, e) =>
                {

                    LogManager.lc流程.Info("爬虫开始抓取地址：" + e.Uri.ToString());
                };
                cityCrawler.OnError += (s, e) =>
                {
                    LogManager.lc流程.Info("爬虫抓取出现错误：" + e.Uri.ToString() + "，异常消息：" + e.Exception.Message);
                };
                cityCrawler.OnCompleted += (s, e) =>
                {
                   
                    res = e.PageSource;

                    //HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
                    //hd.LoadHtml(res);
                    //var nodes = hd.DocumentNode.SelectNodes("//ul[@class='ulcl']//a[@target='_blank']");                  
                    //foreach (var item in nodes)
                    //{
                    //    Jobs.Add(item.Attributes["href"].Value);
                    //}
                };
                cityCrawler.PostAsync(param1).Wait();
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
                sb.Append($"{item.Key}:{item.Value}" +Environment.NewLine);                
            }
            LogManager.lc流程.Info(sb.ToString());
            Console.WriteLine("ok");
            return res;
        }

        public void Do2()
        {
            Task[] tasks = new Task[Jobs.Count];
            for (int i = 0; i < Jobs.Count; i++)          
            {
                tasks[i] =Task.Factory.StartNew ((str) =>
                {
                    var cityCrawler = new Spider();//调用刚才写的爬虫程序
                    var heads = new Dictionary<HttpRequestHeader, string>() { };
                    heads.Add(HttpRequestHeader.UserAgent, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    heads.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    heads.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                    heads.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                    heads.Add(HttpRequestHeader.ContentType, "text/html; charset=utf-8");
                    heads.Add(HttpRequestHeader.Host, "www.ithome.com");

                    var param1 = new SpiderRequestParam()
                    {
                        Uri = new Uri(str as string),
                        Heads = heads,
                        Method = Spider.MethodType.Get,
                        AllowAutoRedirect = true
                    };
                    cityCrawler.OnCompleted += (s, e) =>
                    {
                        //HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
                        //hd.LoadHtml(e.PageSource);
                        //var nodes = hd.DocumentNode.SelectNodes("//span[@id='editor_baidu']/strong").FirstOrDefault();
                        //if (nodes != null)
                        //{


                        //    var ar = nodes.InnerText;
                        //    Console.WriteLine(ar);
                        //    lock (_obj)
                        //    {
                        //        if (dictionary.ContainsKey(ar))
                        //        {
                        //            dictionary[ar] += 1;
                        //        }
                        //        else
                        //        {
                        //            dictionary.Add(ar, 1);
                        //        }
                        //    }
                        //}
                        //else
                        //{

                        //}
                    };
                    cityCrawler.Get_Post(param1);
                }, Jobs[i]);
           
            }
            Task.WaitAll(tasks);
        }
        
    }
}
