using CL;
using CobWeb.Browser;
using CobWeb.Core;
using CobWeb.Core.Control;
using CobWeb.Core.Model;
using CobWeb.Util;
using CobWeb.Util.FlashLog;
using CobWeb.Util.HttpHelper;
using CobWeb.Util.ThredHelper;
using Cqa._91bihu;
using Ithome;
using LiteDB;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.Test
{
    public class LogTest
    {
        static FlashLogger _lc流程;
        public static FlashLogger lc流程
        {
            get
            {
                if (_lc流程 == null)
                {
                    _lc流程 = new FlashLogger("流程");
                }
                return _lc流程;
            }
        }
        /// <summary>
        /// 日志测试
        /// 实际时间 flashlog 33s  log4net23s
        /// </summary>
        public static void Test()
        {

            log4net.Config.XmlConfigurator.Configure();
            ILog log = log4net.LogManager.GetLogger("log4net");
            _lc流程 = new FlashLogger("流程");
            var te = 0;
            var te2 = 0;
            CodeTimer.Time("flash", 10000, () =>
            {
                lc流程.Debug(te.ToString() + "--------------------------------------------");
                te++;
            });
            CodeTimer.Time("log4net", 10000, () =>
            {
                log.Debug(te2.ToString() + "--------------------------------------------");
                te2++;
            });

            /*
             flash
             Time Elapsed:   55ms
             CPU Cycles:     63,817,096
             Gen 0:          0
             Gen 1:          0
             Gen 2:          0

            log4net
             Time Elapsed:   9,428ms
             CPU Cycles:     22,985,320,396
             Gen 0:          84
             Gen 1:          0
             Gen 2:          0
              */
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
           // Test.Test4();
            //CodeTimer.Initialize();
            //Load load = new Load();
            //load.Init();


            //LogTest.Test();
            //DbTest.Test();
            //SpiderTest.Test();
            NoUseFormTest.Test();
            Console.ReadKey();
        }
    }

    public static class Test
    {
        /// <summary>
        /// 测试定时器
        /// </summary>
        public static void Test1()
        {
            var ra = new Random();
            TimerIntervial.Register(1, () =>
            {
                StringHelper.ConsoleWriteLineWithTime($"我是1秒执行1次的  当前线程数{System.Diagnostics.Process.GetCurrentProcess().Threads.Count}");
            });
            TimerIntervial.Register(2, () =>
            {
                StringHelper.ConsoleWriteLineWithTime($"我是2秒执行1次的  当前线程数{System.Diagnostics.Process.GetCurrentProcess().Threads.Count}");
                string sb = "";
                foreach (ProcessThread item in System.Diagnostics.Process.GetCurrentProcess().Threads)
                {
                    sb += item.Id + "-" + item.ThreadState + ",";
                }
                Console.WriteLine(sb);
                Thread.Sleep(ra.Next(1000, 10000));
                StringHelper.ConsoleWriteLineWithTime($"我是2秒执行1次的 end ");
            }, 1000);
            TimerIntervial.Enabled();
        }
        /// <summary>
        /// 测试 超时判断类
        /// </summary>
        public static void Test2()
        {
            new Action(() =>
            {
                StringHelper.ConsoleWriteLineWithTime("开始");
                Thread.Sleep(10000);
                StringHelper.ConsoleWriteLineWithTime("end");
            }).InvokeWithTimeout(1000);
            StringHelper.ConsoleWriteLineWithTime("这里结束了");
        }

        /// <summary>
        /// socket协议分段测试
        /// </summary>
        public static void Test3()
        {
            var msg = "般说来，如果没有分段发生， M S S还是越大越好（这也并不总是正确，参见图2 4 - 3和图2 4 - 4中的例子）。报文段越大允许每个报文段传送的数据就越多，相对I P和T C P首部有更高的网络利用率。当T C P发送一个S Y N时，或者是因为一个本地应用进程想发起一个连接，或者是因为另一端的主机收到了一个连接请求，它能将M S S值设置为外出接口上的MT U长度减去固定的I P首部和T C P首部长度。对于一个以太网， M S S值可达1 4 6 0字节。使用IEEE 802.3的封装（参见2 . 2节），它的M S S可达1 4 5 2字节。";
            var b_msg = Encoding.UTF8.GetBytes(msg);

            //string[] total = new string[b_msg.Length + 8];
            var b_t = BitConverter.GetBytes(b_msg.Length + 8);
            var header = "qqqa";
            var b_header = Encoding.UTF8.GetBytes(header);

            var info = b_t.Concat(b_header).ToArray().Concat(b_msg).ToArray();

            Console.WriteLine(Encoding.UTF8.GetString(info));

            var recive = info.ToArray();
            List<byte> ReturnPool = new List<byte>();
            byte[] array = null;
            var length = BitConverter.ToInt32(new byte[] { recive[0], recive[1], recive[2], recive[3] }, 0);
            if (recive.Length == length)
            {
                array = recive;
            }
            else if (recive.Length > length)
            {
                array = recive.Take(length).ToArray();
                ReturnPool.AddRange(recive.Skip(length));
            }
            else if (recive.Length < length)//应该不会出现了
            {

            }



            var str_header = Encoding.UTF8.GetString(new byte[] { array[4], array[5], array[6], array[7] });
            Console.WriteLine(str_header);
            var str_msg = Encoding.UTF8.GetString(array.Skip(8).Take(length).ToArray());
            Console.WriteLine(str_msg);
        }



        public static void Test4()
        {
            var b = new B();
            b.Age = 10;
            Test5(b);

        }
        static void Test5(A a)
        {
            var c = a.SerializeObject();
        }
        public class A
        {
            public string Name { get; set; }
        }
        public class B:A
        {
            public int Age { get; set; }
        }
    }
    /// <summary>
    /// 爬虫测试
    /// </summary>
    public static class SpiderTest
    {
        public static void Test()
        {
            Ithome();
            //Ithome();

        }

        /// <summary>
        /// 抓取城市列表
        /// </summary>
        public static void Ithome()
        {

            var cityUrl = "https://www.ithome.com";//定义爬虫入口URL
            var cityList = new List<City>();//定义泛型列表存放城市名称及对应的酒店URL
            var cityCrawler = new Spider();//调用刚才写的爬虫程序
            var heads = new Dictionary<HttpRequestHeader, string>() { };
            heads.Add(HttpRequestHeader.UserAgent, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            heads.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            heads.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            heads.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
            heads.Add(HttpRequestHeader.ContentType, "text/html; charset=utf-8");
            heads.Add(HttpRequestHeader.Host, "www.ithome.com");          
            var param = new SpiderRequestParam()
            {
                Url = new Uri(cityUrl),
                Heads = heads,
                Method = Spider.MethodType.Get,
            };
            cityCrawler.OnStart += (s, e) =>
            {
                Console.WriteLine("爬虫开始抓取地址：" + e.Uri.ToString());
            };
            cityCrawler.OnError += (s, e) =>
            {
                Console.WriteLine("爬虫抓取出现错误：" + e.Uri.ToString() + "，异常消息：" + e.Exception.Message);
            };
            cityCrawler.OnCompleted += (s, e) =>
            {
                Console.WriteLine(e.PageSource);
            };
            cityCrawler.SendAsync(param).Wait();//没被封锁就别使用代理：60.221.50.118:8090
        }

        /// <summary>
        /// 抓取酒店列表
        /// </summary>
        public static void HotelCrawler()
        {
            var hotelUrl = "http://hotels.ctrip.com/hotel/jinan144";
            var hotelList = new List<Hotel>();
            var hotelCrawler = new Spider();
            hotelCrawler.OnStart += (s, e) =>
            {
                Console.WriteLine("爬虫开始抓取地址：" + e.Uri.ToString());
            };
            hotelCrawler.OnError += (s, e) =>
            {
                Console.WriteLine("爬虫抓取出现错误：" + e.Uri.ToString() + "，异常消息：" + e.Exception.Message);
            };
            hotelCrawler.OnCompleted += (s, e) =>
            {
                var links = Regex.Matches(e.PageSource, @"""><a[^>]+href=""*(?<href>/hotel/[^>\s]+)""\s*data-dopost[^>]*><span[^>]+>.*?</span>(?<text>.*?)</a>", RegexOptions.IgnoreCase);
                foreach (Match match in links)
                {
                    var hotel = new Hotel
                    {
                        HotelName = match.Groups["text"].Value,
                        Uri = new Uri("http://hotels.ctrip.com" + match.Groups["href"].Value
                    )
                    };
                    if (!hotelList.Contains(hotel)) hotelList.Add(hotel);//将数据加入到泛型列表
                    Console.WriteLine(hotel.HotelName + "|" + hotel.Uri);//将酒店名称及详细页URL显示到控制台
                }

                Console.WriteLine();
                Console.WriteLine("===============================================");
                Console.WriteLine("爬虫抓取任务完成！合计 " + links.Count + " 个酒店。");
                Console.WriteLine("耗时：" + e.Milliseconds + "毫秒");
                Console.WriteLine("线程：" + e.ThreadId);
                Console.WriteLine("地址：" + e.Uri.ToString());
            };
            //hotelCrawler.StartAsync(new Uri(hotelUrl)).Wait();//没被封锁就别使用代理：60.221.50.118:8090
        }

     

        public class City
        {
            public string CityName { get; set; }

            public Uri Uri { get; set; }
        }
        public class Hotel
        {
            public string HotelName { get; set; }

            public decimal Price { get; set; }

            public Uri Uri { get; set; }


        }
    }


    public static class DbTest
    {
        public static void Test()
        {
            using (var db = new LiteDatabase(@"test.db"))
            {

                // Get customer collection
                var col = db.GetCollection<Customer>("customers");
                var results2 = col.Find(x => x.Age > 20);
                // Create your new customer instance
                var customer = new Customer
                {
                    Name = "John Doe",
                    Phones = new string[] { "8000-0000", "9000-0000" },
                    Age = 39,
                    IsActive = true,
                    Chr = new Customer()
                    {
                        Name = "Test"
                    }
                };

                // Create unique index in Name field
                col.EnsureIndex(x => x.Name, true);

                // Insert new customer document (Id will be auto-incremented)
                col.Insert(customer);

                // Update a document inside a collection
                customer.Name = "Joana Doe";

                col.Update(customer);

                // Use LINQ to query documents (with no index)
                var results = col.Find(x => x.Age > 20);
            }
        }
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string[] Phones { get; set; }
            public bool IsActive { get; set; }
            public Customer Chr { get; set; }
        }
    }

    public class NoUseFormTest
    {
        public static void Test()
        {
            Test3();
            Console.WriteLine("完工3");
        }
        public static void Test1()
        {
            IthomeSpider ithomeSpider = new IthomeSpider(null);
            var res = ithomeSpider.Excute(null);
          
        }
        public static void Test2()
        {
            CqaSpider cqaSpider = new CqaSpider(null);
            var res = cqaSpider.Excute(null);
        }

        public static void Test3()
        {
            CL_Spider cL_Spider = new CL_Spider(null);
            var res = cL_Spider.Excute(null);
        }
    }
}
