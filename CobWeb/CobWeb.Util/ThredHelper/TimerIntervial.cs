using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CobWeb.Util.ThredHelper
{
    public static class TimerIntervial
    {
        
        static System.Timers.Timer _timer;
        static List<TimerIntervial_Inner> timerIntervial_Inner;
        /// <summary>
        /// 加这个目的是为了避免重入,必须当前Timer执行完毕才能进入下一timer
        /// </summary>
        private static int inTimer = 0;
     
        static TimerIntervial()
        {
            _timer = new System.Timers.Timer()
            {
                Interval = 1000,
            };

            timerIntervial_Inner = new List<TimerIntervial_Inner>();
        }
        static int index = 0;
        private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (Interlocked.Exchange(ref inTimer, 1) == 0)
                {
                    if (index == int.MaxValue)
                    {
                        index = 0;
                    }
                    index++;
                    foreach (var item in timerIntervial_Inner)
                    {
                        if (index % item.Intervial == 0)
                        {
                            if (item.Timeout > 0)
                            {

                                var res = item.Action.BeginInvoke(null, null);
                                if (!res.AsyncWaitHandle.WaitOne(item.Timeout))
                                {
                                    Console.WriteLine("超时了");
                                    try
                                    {                                        
                                        throw new TimerIntervialException();
                                    }
                                    catch (TimerIntervialException)
                                    {
                                    }
                                    //item.Action.EndInvoke(res);
                                }
                            }
                            else
                            {
                                item.Action.Invoke();
                            }

                        }
                    }
                   
                }
            }
           finally
            {
                Interlocked.Exchange(ref inTimer, 0);
            }
        }
        /// <summary>
        /// 注册后调用Enabled
        /// </summary>
        /// <param name="time">执行间隔 秒</param>
        /// <param name="action"></param>
        /// <param name="timeout">设定超时时间强行终止 毫秒</param>
        public static void Register(int time, Action action, int timeout = 0)
        {
            if (time < 0)
            {
                return;
            }
            timerIntervial_Inner.Add(new TimerIntervial_Inner() { Action = action, Intervial = time, Timeout = timeout });
        }
        public static void Enabled()
        {
            _timer.Elapsed += timer_Elapsed;
            _timer.Enabled = true;
        }
        public static void Stop()
        {
            _timer.Stop();
        }
        class TimerIntervial_Inner
        {
            public int Intervial { get; set; }
            public Action Action { get; set; }
            public int Timeout { get; set; }
        }
        class TimerIntervialException : Exception
        {

        }
    }
}
