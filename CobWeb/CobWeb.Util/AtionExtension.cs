using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CobWeb.Util
{
   public static  class AtionExtension
    {
        public static void InvokeWithTimeout(this Action action,int timeout)
        {
            //Thread tokill = null;
            //Action inv = new Action(() =>
            //{
            //    tokill = Thread.CurrentThread;
            //    action();
            //});
            //var res = inv.BeginInvoke(null, null);
            //if (!res.AsyncWaitHandle.WaitOne(timeout))
            //{
            //    tokill.Abort();
            //    Console.WriteLine("超时了");               
            //}
            //inv.EndInvoke(res);

            //var res = action.BeginInvoke(null, null);
            //if (!res.AsyncWaitHandle.WaitOne(timeout))
            //{
            //    //item.Action.EndInvoke(res);
            //    StringHelper.ConsoleWriteLineWithTime("超时了 强行结束");
            //}
            //action.
            //StringHelper.ConsoleWriteLineWithTime("已经结束了");
        }

       
    }

    
}
