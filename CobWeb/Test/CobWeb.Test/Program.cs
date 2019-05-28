using CobWeb.Browser;
using CobWeb.Core;
using CobWeb.Core.Control;
using CobWeb.Core.Model;
using CobWeb.Util;
using CobWeb.Util.FlashLog;
using CobWeb.Util.ThredHelper;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
            CodeTimer.Initialize();
            //Load load = new Load();
            //load.Init();


            LogTest.Test();
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

       
    }


}
