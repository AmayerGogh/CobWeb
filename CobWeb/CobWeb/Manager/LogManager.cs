using CobWeb.Util.FlashLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CobWeb.Web.Manager
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

    }
}
