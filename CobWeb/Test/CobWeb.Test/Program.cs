using CobWeb.Browser;
using CobWeb.Core;
using CobWeb.Core.Control;
using CobWeb.Core.Model;
using CobWeb.Util;
using CobWeb.Util.FlashLog;
using CobWeb.Util.ThredHelper;
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
    public class LogManager
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
        /// </summary>
        public static void Test()
        {
            for (int i = 0; i < 100; i++)
            {
                LogManager.lc流程.Info(i.ToString() + "--------------------------------------------");
                Console.WriteLine(i);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //Load load = new Load();
            //load.Init();
        
           
            Test.Test1();
            Console.ReadKey();
        }
    }
    public class Load
    {
        public static Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
        public void Init()
        {
        }
    }
    public static class Test
    {
        /// <summary>
        /// 测试定时器
        /// </summary>
        public static void Test1()
        {
            var ra= new Random();
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
                    sb +=item.Id+"-" +item.ThreadState +",";
                }
                Console.WriteLine(sb);
                Thread.Sleep(ra.Next(1000,10000));
                StringHelper.ConsoleWriteLineWithTime($"我是2秒执行1次的 end ");
            },1000);
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
    }


}
