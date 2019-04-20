using CefSharp;
using CobWeb.Core.Model;
using CobWeb.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.Core
{
    public partial class FormBrowser
    {




        /// <summary>
        /// 需要辅助方法执行的代码
        /// </summary>
        List<Action> _assistActions = new List<Action>();
        public new bool IsDisposed = false;



        #region _assistActions
        /// <summary>
        /// 辅助方法,当需要借助主线程进行某种操作时
        /// </summary>
        public void StartAssist()
        {
            var timer = new TimerHelp();
            timer.ExcuteCompleted += Assist_timer_ExcuteCompleted;
            timer.Start();
        }
        /// <summary>
        /// 增加方法到辅助方法中进行执行,执行完之后会自动清除
        /// </summary>
        public void AddAssistAction(Action action)
        {
            lock (_assistActions)
            {
                _assistActions.Add(action);
            }
        }

        public void ClearAssistAction()
        {
            lock (_assistActions)
            {
                if (_assistActions.Count > 0)
                {
                    try
                    {
                        _assistActions.Clear();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        void Assist_timer_ExcuteCompleted(TimerHelp timer, object param, bool isCancel)
        {
            lock (_assistActions)
            {
                if (_assistActions.Count > 0)
                {
                    try
                    {
                        foreach (var action in _assistActions)
                        {
                            try
                            {
                                action();
                            }
                            catch (Exception ee)
                            {
                                // var position = nameof(ProcessForm) + "--" + nameof(Assist_timer_ExcuteCompleted);
                                //var errMsg = ee.ErrMessage(position);
                                var errTitle = "【ProcessForm 定时方法执行异常】：";
                                //LogManager.全局异常重点关注_Error(errTitle + errMsg.DetailErrMessage);
                            }
                        }
                    }
                    catch { }
                    finally
                    {
                        _assistActions.Clear();
                    }
                }
            }

            //webBrowser销毁表示程序退出了
            if (!KernelControl.IsDisposed())
            {
                StartAssist();
            }
            else
            {
                //打个日志
            }


        }
        #endregion

        #region 执行
        /// <summary>
        /// 处理程序,每次须重新创建
        /// </summary>
        IProcessBase _process = null;
       
        #endregion

        public void ClearWebBrowser()
        {
            try
            {
                KernelControl.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public void Stop()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        FormLog _formLog;
        FormLog FormLog
        {
            get
            {
                if (_formLog == null || _formLog.IsDisposed)
                {
                    _formLog = new FormLog(true);
                }
                return _formLog;
            }
        }
        void ShowLogForm()
        {
            Thread newThread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    var form = FormLog;
                    if (form.Visible == true)
                    {
                        form.Activate();
                    }
                    else
                    {
                        FormLog.ShowDialog();
                    }

                }
                finally
                {
                    //dForm.Close();
                }
            }));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.IsBackground = true; //随主线程一同退出
            newThread.Start();
        }
        public void ExcuteRecord(string txt)
        {
            if (_formLog != null)
            {
                _formLog.ExcuteRecord(txt);
            }
        }


        //public Action<string> _excuteRecord;





        //对外接口

        /// <summary>
        /// 执行事件完成后返回的结果,未完成时为null
        /// </summary>
        string _result;
        public string GetResult()
        {
            if (_result ==null)
            {
                return null;
            }
            //todo 深拷贝
            var result = _result;
            _result = null;
            return result;
        }
        public void SetResult(string result)
        {
            this._result = result;
        }


        /// <summary>
        /// 标记是否正在执行
        /// </summary>
        bool _isWorking = false;
        public bool IsWorking()
        {
            return this._isWorking;
        }

        public void SetWorking(IProcessBase process)
        {
            this._isWorking = true;

        }
        public void SetWorkingStop()
        {
            this._isWorking = false;
        }

        /// <summary>
        /// 是否显示窗口
        /// </summary>
        public bool isShowForm { get; set; }
        public bool IsShowForm()
        {
            return isShowForm;
        }
        public void Navigate(string address)
        {
            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            try
            {

                this.KernelControl.Navigate(address);
                
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }
       
    }

}
