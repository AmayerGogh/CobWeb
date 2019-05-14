using CobWeb.Util.FlashLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CobWeb.Core.Manager
{
    public  class LogManager
    {
        static FlashLogger _lc流程;
        public static FlashLogger lc流程
        {
            get
            {
                if (_lc流程==null)
                {
                    _lc流程 = new FlashLogger("流程");
                }
                return _lc流程;
            }
        }
        static FlashLogger _yc全局异常;
        public static FlashLogger yc全局异常
        {
            get
            {
                if (_yc全局异常 == null)
                {
                    _yc全局异常 = new FlashLogger("全局异常");
                }
                return _yc全局异常;
            }
        }
        static FlashLogger _socket通讯;
      
        public  static FlashLogger socket通讯
        {
            get
            {
                if (_socket通讯 == null)
                {
                    _socket通讯 = new FlashLogger("socket通讯");
                }
                return _socket通讯;
            }
        }
    }
}
