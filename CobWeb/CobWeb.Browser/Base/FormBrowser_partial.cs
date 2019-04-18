using CefSharp;
using CobWeb.Core.Model;
using CobWeb.Core.Process;
using CobWeb.Util;
using CobWeb.Util.Control;
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

namespace CobWeb.Browser
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
            if (!Mybrowser.IsDisposed())
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
        /// <summary>
        /// 设置操作类型,也即设置处理事件
        /// </summary>
        public void SetActionType(ParamModel paramModel)
        {
            //赋值为null
            this._result = null;

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
                Mybrowser.Dispose();
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
        //public Action<string> _excuteRecord;
        public void ExcuteRecord(string txt)
        {
            if (_formLog != null)
            {
                _formLog.ExcuteRecord(txt);
            }


        }



        /// <summary>
        /// 端口号
        /// </summary>
        public static int Port { get; set; }

        /// <summary>
        /// 进程编号
        /// </summary>
        public static int Number { get; set; }

        /// <summary>
        /// 初始化所属MacUrl
        /// </summary>
        public static string MacUrl { get; set; }

        //step


        public void Step1_GetSetting()
        {
            //得到端口号
            string[] cmdArgs = Environment.GetCommandLineArgs();
            if (cmdArgs.Length > 0 && cmdArgs[0].StartsWith("BiHu|"))
            {
                var param = cmdArgs[0].Split('|');
                Number = int.Parse(param[1]);
                Port = int.Parse(param[2]);
                MacUrl = param[3];

                //LogDir = MacUrl.Replace(":", string.Empty);
            }
            else
            {
                Number = 0;
                Port = 6666;
            }
        }


        Socket _serverSocket;
        void Step2_StartListen()
        {
            IPEndPoint ipe;
            _serverSocket = SocketBasic.GetSocket(out ipe, Port);
            _serverSocket.Bind(ipe);
            _serverSocket.Listen(10);
            var text = string.Format("建立调用监听完成! {0} 端口:{1} 序号:{2}", MacUrl, Port, Number);
            ExcuteRecord(text);
            ExcuteRecord("*******************************************************");
            ExcuteRecord("注:一个进程同时只会执行一个需要窗口的接口!");
            ExcuteRecord("*******************************************************");
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Socket cSocket = _serverSocket.Accept();
                        Task.Factory.StartNew((c) =>
                        {
                            var socket = c as Socket;
                            try
                            {
                                var recvStr = SocketBasic.Receive(socket, 3);
                                Thread.CurrentThread.SetThreadName("RequestInstance_Thread");
                                ExcuteRecord("接收到的信息");
                                ExcuteRecord(recvStr);
                                var result = Excute(recvStr);

                                SocketBasic.Send(socket, result, 3);
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                if (socket != null)
                                    socket.Dispose();
                            }

                        }, cSocket);
                    }
                    catch (Exception ex)
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
            });
        }

        readonly Object _objLock = new Object();
        string Excute(string dataParam)
        {
            try
            {
                var paramModel = JsonConvert.DeserializeObject<ParamModel>(dataParam);
                ExcuteRecord(string.Format("请求接口:{0} 超时时间:{1}秒 使用窗口:{2}", paramModel.Method, paramModel.Timeout, paramModel.IsUseForm));

                //是否使用窗口
                if (paramModel.IsUseForm)
                {
                    lock (_objLock)
                    {
                        //如果还在执行则直接返回,这个很重要
                        if (_isWorking)
                        {
                            return JsonConvert.SerializeObject(new ResultModel()
                            {
                                IsSuccess = false,
                                Result = ArtificialCode.A_ChangeProcess.ToString()
                            });
                        }
                        else
                        {
                            _isWorking = true;
                        }

                    }

                    return ProcessAndResult(dataParam, paramModel);
                }
                else
                {
                    var process = ProcessFactory.GetProcessByMethod(paramModel);
                    return JsonConvert.SerializeObject(new ResultModel()
                    {
                        IsSuccess = true,
                        Result = process.Excute(dataParam)
                    });
                }
            }
            catch (Exception ex)
            {
                //_log.FatalFormat("Excute()\r\n{0}", ex.Message);
                return JsonConvert.SerializeObject(new ResultModel()
                {
                    IsSuccess = false,
                    Result = ex.Message
                });
            }
        }

        string ProcessAndResult(string dataParam, ParamModel paramModel)
        {
            var resultModel = new ResultModel();
            resultModel.IsSuccess = true;
            resultModel.Result = "666";
            try
            {
                //设置并执行相应操作，核心方法
                SetActionType(paramModel);

                try
                {
                    //得到执行结果
                    while (!CommonCla.IsTimeout(paramModel.StartTime, paramModel.Timeout))
                    {
                        resultModel.Result = _result;
                        if (resultModel.Result == null)
                        {
                            if (MonitorStopProcess(paramModel.StopKey))
                            {
                                resultModel.Result = ArtificialCode.A_RequestNormalBreak.ToString();
                                break;
                            }

                            if (IsDisposed)
                                throw new Exception(ArtificialCode.A_RequestAccidentBreak.ToString());
                            Thread.Sleep(100);
                        }
                        else
                        {
                            resultModel.IsSuccess = true;
                            break;
                        }
                    }

                    if (resultModel.Result == null)
                        throw new Exception(ArtificialCode.A_TimeOutResult.ToString());
                }
                catch (Exception ex)//获取结果发生错误
                {
                    resultModel.Result = ex.Message;
                    //_log.ErrorFormat("耗时:{0}\r\n{1}\r\n{2}", CommonCla.GetMilliseconds(paramModel.StartTime), dataParam, resultModel.Result);
                }
            }
            catch (Exception ex)//解析参数发生错误
            {
                //如果Start发生异常
                _isWorking = false;

                resultModel.Result = ex.Message;
                //_log.FatalFormat("{0}\r\nStart()\r\n{1}", paramModel.Method, ex.Message);
            }
            finally
            {
                Task.Run(() =>
                {
                    //本次执行完成,退出使用  //不阻塞执行,尽快返回结果
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
                });
            }

            return JsonConvert.SerializeObject(resultModel);

        }
        /// <summary>
        /// 中断请求的监视
        /// </summary>
        bool MonitorStopProcess(string guidkey)
        {
            if (MemoryCacheHelper.CacheIsHave(guidkey))
            {
                MemoryCacheHelper.RemoveCache(guidkey);
                return true;
            }
            else
                return false;
        }



        //对外接口

        /// <summary>
        /// 执行事件完成后返回的结果,未完成时为null
        /// </summary>
        public string _result;
        public string GetResult()
        {
            return _result;
        }
        public void SetResult(string result)
        {
            this._result = result;
        }


        /// <summary>
        /// 标记是否正在执行
        /// </summary>
        public bool _isWorking = false;
        public bool IsWorking()
        {
            return this._isWorking;
        }

        public void SetWorking(bool iswork)
        {
            this._isWorking = iswork;
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

                this.Mybrowser.Navigate(address);
                
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }
       
    }

}
