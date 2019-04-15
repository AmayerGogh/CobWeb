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

            var test = "user=hash=4CD4FE9BD8795F178C8903D96B54E293E06F1C968135957810F1D94F0ED1573944A45B631BE04236C08128CC9569CDE90578E451C3EA9BC1B39F24213074125B9D1B81DB7C9291F2CCA327EC4C6D668A070FC0A5A6DC893C";
            Console.ReadKey();
        }
    }
    
    
}
