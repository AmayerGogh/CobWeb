using CobWeb.Core.Model;
using CobWeb.Core.Process;
using CobWeb.Util;
using CobWeb.Util.FlashLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CobWeb.Util.TimerHelp;

namespace CobWeb.Core
{
    public abstract class ProcessBaseUseBrowser : IProcessBase
    {

        /// <summary>
        /// 当前加载数据前的时间戳
        /// </summary>
        protected long startTime = 0;
        protected ProcessBase processBase;

        protected FlashLogger _log;
        /// <summary>
        /// 等待的默认时间,单位:秒
        /// </summary>
        public int timeout = 30;
        public bool IsWorkGoOn { get; set; }
        public ProcessBaseUseBrowser(FormBrowser form, ParamModel paramModel, FlashLogger log)
        {
            _log = log;
            processBase = new ProcessBase(form,_log);            
        }

        public virtual void Begin()
        {
            LoginProcessEndHandler += LoginProcessEnd;
            processBase._form.AddAssistAction(() =>
            {
                Login();
            });
        }
        /// <summary>
        /// 返回结果后会主动调用
        /// </summary>
        public virtual void End()
        {
            if (IsWorkGoOn)
            {
                processBase.End(IsWorkGoOn);
            }
            else
            {
                processBase.End(IsWorkGoOn);
            }
        }
        /// <summary>
        /// 执行流程
        /// </summary>
        public abstract void StartRequest();
        public abstract void Login();
        void LoginProcessEnd(LoginResult loginResult)
        {
            LoginProcessEndHandler -= LoginProcessEnd;
            if (loginResult.IsSuccess)
            {
                StartRequest();
                //WFContext.Ins.ProcessForm.AddAssistAction(StartRequest);
            }
            else
            {
                processBase.SetResult(new ResultModel()
                {
                    IsSuccess = false,
                    Result = "{\"test\":\"登陆失败\"}"
                });
            }
        }
        public void LoginProcessEndInvoke(LoginResult loginResult)
        {
            LoginProcessEndHandler.Invoke(loginResult);
        }
        public event Action<LoginResult> LoginProcessEndHandler;




        //public MyWebBrowser Browser
        //{
        //    get
        //    {
        //        return processBase._form.WebBrowser;
        //    }
        //}
        /// <summary>
        /// 对   TimerHelp Start     进行封装，提供判断  IsQuit() 的功能 ，子类不需要再判断
        /// </summary>
        public void TimerHelp_Start(ExcuteCompletedAgency excuteCompleted, object param = null, int times = 100, int intervalTimes = 100)
        {
            TimerHelp.Start((x, y, z) =>
            {
                if (IsQuit())
                {
                    //_form. RecordLog("IsQuit() 退出：" + excuteCompleted?.Method?.Name);
                    return;
                }

                excuteCompleted(x, y, z);

            }, param, times, intervalTimes);
        }

        private bool IsQuit()
        {
            return processBase._isQuit;
        }
        public void Log(string txt)
        {
            StackTrace ss = new StackTrace(true);
            var mb = ss.GetFrame(1).GetMethod().Name;
            var mb2 = ss.GetFrame(2).GetMethod().Name;
            if (mb != mb2)
            {
                processBase.Log(txt);
            }
        }
    }
}
