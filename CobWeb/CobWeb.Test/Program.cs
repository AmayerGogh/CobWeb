using CobWeb.Util;
using CobWeb.Util.FlashLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
           
            
            MemoryMappedHelper.WriteIntoMMF("ok", "test:1234");

           var 从=  MemoryMappedHelper.ReadIntoMMF("ok");
            Console.ReadKey();
        }
    }
    
    
}
