using CobWeb.Core;
using CobWeb.Core.Model;
using CobWeb.Core.Process;
using CobWeb.Util;
using CobWeb.Util.Model;
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
namespace CobWeb.AProcess
{
    public  class ProcessControl
    {
        public  ProcessControl()
        {
        }
        public void Init()
        {
        }
        public static FormBrowser FormBrowser;
        /// <summary>
        /// 端口号
        /// </summary>
        public  int Port { get; set; }
        /// <summary>
        /// 进程编号
        /// </summary>
        public  int Number { get; set; }
        /// <summary>
        /// 初始化所属MacUrl
        /// </summary>
        public  string MacUrl { get; set; }
        public string KernelType { get; set; }
        //step
        public void Step1_GetSetting()
        {
        }
        Socket _serverSocket;
      public  void StartListen()
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
                Thread.CurrentThread.Name = "socketBase";
                while (true)
                {
                    try
                    {
                        Socket cSocket = _serverSocket.Accept();
                        Task.Factory.StartNew((c) =>
                        {
                            Thread.CurrentThread.Name = "socketListen";
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
                var paramModel = JsonConvert.DeserializeObject<SocketRequestModel>(dataParam);
                ExcuteRecord(string.Format("请求接口:{0} 超时时间:{1}秒 使用窗口:{2}", paramModel.Method, paramModel.Timeout, paramModel.IsUseForm));
                //是否使用窗口
                if (paramModel.IsUseForm)
                {
                    if (paramModel.KernelType != null && paramModel.KernelType != KernelType)
                    {
                        return JsonConvert.SerializeObject(new SocketResponseModel()
                        {                            
                            StateCode = 0,
                            Result = SocketResponseCode.A_KernelError.ToString()
                        });
                    }
                    lock (_objLock)
                    {
                        //如果还在执行则直接返回,这个很重要
                        if (FormBrowser.IsWorking())
                        {
                            return JsonConvert.SerializeObject(new SocketResponseModel()
                            {
                                StateCode = 0,
                                Result = SocketResponseCode.A_ChangeProcess.ToString()
                            });
                        }                       
                    }
                    return ProcessAndResult(dataParam, paramModel);
                }
                else
                {
                    var process = ProcessFactory.GetProcessByMethod(paramModel);
                    return JsonConvert.SerializeObject(new SocketResponseModel()
                    {
                        StateCode = 0,
                        Result = process.Excute(dataParam)
                    });
                }
            }
            catch (Exception ex)
            {
                //_log.FatalFormat("Excute()\r\n{0}", ex.Message);
                return JsonConvert.SerializeObject(new SocketResponseModel()
                {
                    StateCode = 0,
                    Result = ex.Message
                });
            }
        }
        string ProcessAndResult(string dataParam, SocketRequestModel paramModel)
        {
            var resultModel = new SocketResponseModel();
            resultModel.StateCode = 0;
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
                        resultModel.Result = FormBrowser.GetResult();
                        if (resultModel.Result == null)
                        {
                            if (MonitorStopProcess(paramModel.Key))
                            {
                                resultModel.Result = SocketResponseCode.A_RequestNormalBreak.ToString();
                                break;
                            }
                            if (FormBrowser.IsDisposed)
                                throw new Exception(SocketResponseCode.A_RequestAccidentBreak.ToString());
                            Thread.Sleep(100);
                        }
                        else
                        {                            
                            resultModel.StateCode = 0;
                            break;
                        }
                    }
                    if (resultModel.Result == null)
                        throw new Exception(SocketResponseCode.A_TimeOutResult.ToString());
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
                FormBrowser.SetWorkingStop();                
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
        #region 执行
        /// <summary>
        /// 处理程序,每次须重新创建
        /// </summary>
        IProcessBase _process = null;
        /// <summary>
        /// 设置操作类型,也即设置处理事件
        /// </summary>
        public void SetActionType(SocketRequestModel paramModel)
        {
            //赋值为null
           // this._result = null;
            //得到处理程序,若有异常直接抛出
            _process = ProcessFactory.GetProcessByMethod(ProcessControl.FormBrowser, paramModel);
            FormBrowser.SetWorking(_process);
            //开始执行
            _process?.Begin();
        }
        #endregion
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
        public void ExcuteRecord(string txt)
        {
            FormBrowser.ExcuteRecord(txt);            
        }
        //监听
    }
}
