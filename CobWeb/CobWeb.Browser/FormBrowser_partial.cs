﻿using CobWeb.Core.Model;
using CobWeb.Core.Process;
using CobWeb.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Browser
{
    public partial class FormBrowser
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize(bool isShow, Action<string> record)
        {
            _excuteRecord = record;
            IsShowForm = isShow;           
        }
        /// <summary>
        /// 需要辅助方法执行的代码
        /// </summary>
        List<Action> _assistActions = new List<Action>();
        /// <summary>
        /// 标记是否正在执行
        /// </summary>
       // public bool IsWorking = false;
        /// <summary>
        /// 是否显示窗口
        /// </summary>
        public bool IsShowForm { get; set; }
        public bool IsWorking { get; set; }
        public new bool IsDisposed = false;
        public Action<string> _excuteRecord;
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
            if (!browser.IsDisposed)
            {
                StartAssist();
            }
            else
            {
                //打个日志
            }


        }
        #endregion

        /// <summary>
        /// 执行事件完成后返回的结果,未完成时为null
        /// </summary>
        public string Result { get; set; }

        #region 执行
        /// <summary>
        /// 处理程序,每次须重新创建
        /// </summary>
        IProcessBase _process = null;
        /// <summary>
        /// 设置操作类型,也即设置处理事件
        /// </summary>
        public void SetActionType(ParamModel paramModel)
        {
            //赋值为null
            this.Result = null;

            //得到处理程序,若有异常直接抛出
            _process = ProcessFactory.GetProcessByMethod(this, paramModel);

            //开始执行
            _process?.Begin();
        }
        #endregion

        public void ClearWebBrowser()
        {
            try
            {
              
                browser.Dispose();
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
        /// <summary>
        /// 执行完成之后的退出
        /// </summary>
        public void Quit()
        {
            try
            {
                //TODO这个地方有空引用，正常这是不可能的，暂时不处理
                _process?.End();
                _process = null;
            }
            catch (Exception ee)
            {
                //var position = nameof(ProcessForm) + "--" + nameof(Quit);                            
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        public MyWebBrowser WebBrowser
        {
            get
            {
                return this.WebBrowser;
            }
        }
    }
}