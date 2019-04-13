
using CobWeb.Core.Process;
using CobWeb.Util.FlashLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CobWeb.AProcess.Base
{
   
        public class ProcessBase
        {
            /// <summary>
            /// 标记是否在结束时关闭此窗口
            /// </summary>
            protected bool _isCloseForm = false;

            /// <summary>
            /// 标记是否在结束时关闭程序
            /// </summary>
            protected bool _isShutdown = false;
            /// <summary>
            /// 标识是否已经退出此操作
            /// </summary>
            public bool _isQuit = false;
            /// <summary>
            /// 需要交互的页面，这个需要释放
            /// </summary>
            public IBrowserBase _form;
            /// <summary>
            /// 用于限制一些潜在的并发操作
            /// </summary>
            protected Object _objLock = new Object();

            public ProcessBase(IBrowserBase form)
            {
                _form = form;
            }
            /// <summary>
            /// 用于记录操作日志
            /// </summary>
            public StringBuilder _sb = new StringBuilder();
            public void SetResult(Object result)
            {
                SetResult(JsonConvert.SerializeObject(result));
            }

            /// <summary>
            /// 设置返回结果,即执行结束
            /// </summary>
            public bool SetResult(string result)
            {


                lock (_objLock)
                {
                    if (_form != null)
                    {
                        _form.SetResult(result);
                        return true;
                    }
                }
                return false;
            }
            public bool Log(string txt)
            {
                if (string.IsNullOrWhiteSpace(txt))
                {
                    return true;
                }
                try
                {
                    lock (_objLock)
                    {
                        if (_form != null)
                        {
                            if (_form.IsShowForm())
                                _form.ExcuteRecord(txt);

                            _sb.AppendLine(string.Format(" -->[{0}] [{1},{2}]:{3}",
                                DateTime.Now.ToString("HH:mm:ss.fff"),
                                Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2, '0'),
                                 System.Diagnostics.Process.GetCurrentProcess().Id.ToString().PadLeft(5, '0'),
                                txt));
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    new FlashLogger("流程").Fatal("ProcessBase.RecordLog() 出现错误:" + ex.ToString());
                }

                return false;
            }
            internal void End(bool isWorkGoOn = false)
            {
                if (isWorkGoOn)
                {
                    
                }
                else
                {
                    //不再使用窗口
                    lock (_objLock)
                    {
                        _isQuit = true;

                        if (_form == null)
                            return;

                        //记录完整操作流程日志
                        //RecordLog("end 耗时:{0}", CommonCla.GetMilliseconds(_paramModel.StartTime));
                        _sb.AppendLine(_form.GetResult());

                        if (!_isCloseForm)
                        {
                            _form.SetWorking(false);                            
                            _form.Stop();
                        }
                        else
                        {
                            _form.Close();
                        }

                        _form = null;

                       
                    }
                }

            }
        }
    
}
