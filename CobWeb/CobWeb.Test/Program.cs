using CobWeb.Browser;
using CobWeb.Core;
using CobWeb.Core.Control;
using CobWeb.Core.Model;
using CobWeb.Util;
using CobWeb.Util.FlashLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

            Load load = new Load();
            load.Init();
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
    
}
