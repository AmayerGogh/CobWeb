using CobWeb.Core;
using CobWeb.Core.Manager;
using CobWeb.Util;
using CobWeb.Util.HttpHelper;
using CobWeb.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cqa._91bihu
{
    public class CqaSpider : ProcessBaseNotUseBrowser
    {
        public CqaSpider(SocketRequestModel paramModel) : base(paramModel)
        {

        }
        public override string Excute(object param)
        {
            for (int i = 0; i < 50; i++)
            {
                var date = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd");
                var cc = GetPage(date);
                if (string.IsNullOrEmpty(cc))
                {
                    return null;
                }
                var res = Analyse(cc);
                LogManager.lc流程.Info($"{date}:{res}");
                Console.WriteLine($"{date}:{res}");
                Thread.Sleep(1000);
            }          
            return null;
        }

        string GetPage(string url)
        {
            var res = string.Empty;
            try
            {
                var cityUrl = "http://cqa.91bihu.com/chart/index2";
                var cityCrawler = new Spider();
                var heads = new Dictionary<HttpRequestHeader, string>() { };
                heads.Add(HttpRequestHeader.UserAgent, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                heads.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                heads.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                heads.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                heads.Add(HttpRequestHeader.ContentType, "text/html; charset=utf-8");
                heads.Add(HttpRequestHeader.Host, "cqa.91bihu.com");

                var param1 = new SpiderRequestParam()
                {
                    Uri = new Uri(cityUrl),
                    GetParam = $"salesregions=&province=&city=&agentId=&agentName=&searchType=customer&source=平安&noCache=false&Source_SubType=-1&excludeAssignSpErr=false&time1_s={url} 00:00:00&time1_e={url} 23:59:59&time2_s=&time2_e=&time3_s=&time3_e=",
                    Heads = heads,
                    Method = Spider.MethodType.Get,
                    AllowAutoRedirect = true,
                    Cookies = "_te_id.6f30=dd75d9c6-4bcc-48f0-87b6-302534b915b0.1555291196.46.1560738444.1560735046.932baae5-55f1-48e1-8547-d12b7615eeee; ASP.NET_SessionId=ouvo1eqtzofjj4wlcos3fp0q; _te_ses.6f30=*"
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
                    var re = Regex.Match(e.PageSource, "\"elapsedTime\":\\[.*\\]");
                    if (!string.IsNullOrWhiteSpace(re.Value))
                    {
                        res = re.Value.Replace("\"elapsedTime\":", string.Empty);
                    }

                };
                cityCrawler.PostAsync(param1).Wait();
                //Do2();

            }
            catch (Exception)
            {


            }
            finally
            {

            }

            return res;
        }

        string Analyse(string data)
        {
            try
            {
                var datas = data.DeserializeObject<List<Tv>>();
                double total = 0;
                int totalCount = 0;
                foreach (var item in datas)
                {
                    switch (item.Key)
                    {
                        case "0-2":
                            total += (item.Value != 0 ? item.Value * 1 : 0);
                            break;
                        case "2-5":
                            total += (item.Value != 0 ? item.Value * 3.5 : 0);
                            break;
                        case "5-10":
                            total += (item.Value != 0 ? item.Value * 7.5 : 0);
                            break;
                        case "10-15":
                            total += (item.Value != 0 ? item.Value * 13.5 : 0);
                            break;
                        case "15-20":
                            total += (item.Value != 0 ? item.Value * 17.5 : 0);
                            break;
                        case "20-30":
                            total += (item.Value != 0 ? item.Value * 25 : 0);
                            break;
                        case "30-40":
                            total += (item.Value != 0 ? item.Value * 35 : 0);
                            break;
                        case "40-50":
                            total += (item.Value != 0 ? item.Value * 45 : 0);
                            break;
                        case "50-60":
                            total += (item.Value != 0 ? item.Value * 55 : 0);
                            break;
                        case "60+":
                            total += (item.Value != 0 ? item.Value * 60 : 0);
                            break;
                        default:
                            break;
                    }
                    totalCount += item.Value;
                }
                var re = total / totalCount;
                return re.ToString();
            }
            catch (Exception)
            {

                return null;
            }

            return null;
        }
    }

    public class TVList
    {
        public List<TVList> List { get; set; }
    }
    public class Tv
    {
        public string Key { get; set; }
        public int Value { get; set; }
    }
}
