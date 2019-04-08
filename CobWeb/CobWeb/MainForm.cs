using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb
{
    public partial class MainForm : Form
    {
       
        /// <summary>
        /// 标记是否关闭程序
        /// </summary>
        public static bool IsShutdown = false;

        /// <summary>
        /// 是否显示
        /// </summary>
        readonly bool _isShowForm;

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

        /// <summary>
        /// 用于日志目录
        /// </summary>
        public static string LogDir { get; set; }

        /// <summary>
        /// 调试窗口
        /// </summary>
        private void btn_test_Click(object sender, EventArgs e)
        {
           
        }
        /// <summary>
        /// 窗口初始化
        /// </summary>
        /// <param name="isShow">是否显示</param>
        public MainForm(bool isShow = true)
        {
            //Thread.CurrentThread.SetThreadName("Main_Thread");
            //自动点击需要这个标题
            this.Text = Process.GetCurrentProcess().ProcessName;

            InitializeComponent();

            _isShowForm = isShow;

            //得到端口号
            string[] cmdArgs = Environment.GetCommandLineArgs();
            if (cmdArgs.Length > 0 && cmdArgs[0].StartsWith("BiHu|"))
            {
                var param = cmdArgs[0].Split('|');
                Number = int.Parse(param[1]);
                Port = int.Parse(param[2]);
                MacUrl = param[3];

                LogDir = MacUrl.Replace(":", string.Empty);
            }
            else
            {
                Number = 0;
                Port = 6666;
            }

            if (!_isShowForm)
            {
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;

               
            }

            Control.CheckForIllegalCrossThreadCalls = false;
        }
        /// <summary>
        /// 主页初始化,只触发一次
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
               

                //开启监听调用
               // StartListen();
                var text = string.Format("建立调用监听完成! {0} 端口:{1} 序号:{2}", MacUrl, Port, Number);               
                ExcuteRecord(text);

                //开启登录监听
                //StartLoginMonitor();

                //try
                //{
                //    var wb = new MyWebBrowser();
                //    ExcuteRecord("当前IE版本:" + wb.Version);
                //    wb.Dispose();
                //}
                //catch
                //{
                //    //
                //}

                ExcuteRecord("*******************************************************");
                ExcuteRecord("注:一个进程同时只会执行一个需要窗口的接口!");
                ExcuteRecord("*******************************************************");
            }
            catch (Exception ex)
            {
            
                ExcuteRecord("初始化发生异常:" + ex.Message);
                ExcuteRecord("5秒后自动关闭...");

                Task.Run(() =>
                {
                    Thread.Sleep(1000 * 5);
                    Process.GetCurrentProcess().Kill();
                });
            }
        }
        /// <summary>
        /// 显示执行时会造成程序无法响应,隐藏时没有问题
        /// </summary>
        private void ExcuteRecord(string msg)
        {
            if (_isShowForm)
            {
                lock (rtb_record)
                {
                    if (rtb_record.Text.Length > 10000)
                        rtb_record.Clear();

                    rtb_record.AppendText(string.Format("T[{0}][{1}] {2}\r\n", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToLongTimeString(), msg));
                    rtb_record.ScrollToCaret();
                }
            }
        }

        /// <summary>
        /// 开启监听调用
        /// </summary>
        Socket _serverSocket;
        //void StartListen()
        //{
        //    IPEndPoint ipe;
        //    _serverSocket = SocketBasic.GetSocket(out ipe, Port);
        //    _serverSocket.Bind(ipe);
        //    _serverSocket.Listen(10);

        //    Task.Run(() =>
        //    {
        //        while (true)
        //        {
        //            try
        //            {
        //                Socket cSocket = _serverSocket.Accept();
        //                Task.Factory.StartNew((c) =>
        //                {
        //                    var socket = c as Socket;
        //                    try
        //                    {
        //                        var recvStr = SocketBasic.Receive(socket, 3);
        //                        Thread.CurrentThread.SetThreadName("RequestInstance_Thread");
        //                        ExcuteRecord("接收到的信息");
        //                        ExcuteRecord(recvStr);
        //                        var result = Excute(recvStr);

        //                        SocketBasic.Send(socket, result, 3);
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }
        //                    finally
        //                    {
        //                        if (IsShutdown)
        //                        {
        //                            _serverSocket.Dispose();
        //                            Process.GetCurrentProcess().Kill();
        //                        }

        //                        if (socket != null)
        //                            socket.Dispose();
        //                    }

        //                }, cSocket);
        //            }
        //            catch (Exception ex)
        //            {

        //                Process.GetCurrentProcess().Kill();
        //            }
        //        }
        //    });
        //}

        ////FormSpider _excuteForm = null;
        //readonly Object _objLock = new Object();
        //string Excute(string dataParam)
        //{
        //    try
        //    {
        //        var paramModel = JsonConvert.DeserializeObject<ParamModel>(dataParam);
        //        ExcuteRecord(string.Format("请求接口:{0} 超时时间:{1}秒 使用窗口:{2}", paramModel.Method, paramModel.Timeout, paramModel.IsUseForm));
        //        if (paramModel.IsUseForm)
        //        {
        //            lock (_objLock)
        //            {
        //                if (_excuteForm != null)
        //                {
        //                    if (!_excuteForm.IsDisposed)
        //                    {
        //                        //如果还在执行则直接返回,这个很重要
        //                        if (_excuteForm.IsWorking)
        //                        {
        //                            return JsonConvert.SerializeObject(new ResultModel()
        //                            {
        //                                IsSuccess = false,
        //                                Result = ArtificialCode.A_ChangeProcess.ToString()
        //                            });
        //                        }
        //                        else
        //                            _excuteForm.IsWorking = true;
        //                    }
        //                    else
        //                    {
        //                        _excuteForm = GetProcessForm();
        //                        _excuteForm.IsWorking = true;
        //                    }
        //                }
        //                else
        //                {
        //                    _excuteForm = GetProcessForm();
        //                    _excuteForm.IsWorking = true;
        //                }
        //            }

        //            return ProcessAndResult(_excuteForm, dataParam, paramModel);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    //try
        //    //{
        //    //    var paramModel = JsonConvert.DeserializeObject<ParamModel>(dataParam);
        //    //    ExcuteRecord(string.Format("请求接口:{0} 超时时间:{1}秒 使用窗口:{2}", paramModel.Method, paramModel.Timeout, paramModel.IsUseForm));

        //    //    //是否使用窗口
        //    //    if (paramModel.IsUseForm)
        //    //    {
        //    //        lock (_objLock)
        //    //        {
        //    //            if (_excuteForm != null)
        //    //            {
        //    //                if (!_excuteForm.IsDisposed)
        //    //                {
        //    //                    //如果还在执行则直接返回
        //    //                    if (_excuteForm.IsWorking)
        //    //                    {
        //    //                        return JsonConvert.SerializeObject(new ResultModel()
        //    //                        {
        //    //                            IsSuccess = false,
        //    //                            Result = ArtificialCode.A_ChangeProcess.ToString()
        //    //                        });
        //    //                    }
        //    //                    else
        //    //                        _excuteForm.IsWorking = true;
        //    //                }
        //    //                else
        //    //                {
        //    //                    _excuteForm = GetProcessForm();
        //    //                    _excuteForm.IsWorking = true;
        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                _excuteForm = GetProcessForm();
        //    //                _excuteForm.IsWorking = true;
        //    //            }
        //    //        }

        //    //        return ProcessAndResult(_excuteForm, dataParam, paramModel);
        //    //    }
        //    //    else
        //    //    {
        //    //        var process = ProcessFactory.GetProcessByMethod(paramModel);
        //    //        return JsonConvert.SerializeObject(new ArtificialResultModel()
        //    //        {
        //    //            IsSuccess = true,
        //    //            Result = process.Excute(dataParam)
        //    //        });
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    _log.FatalFormat("Excute()\r\n{0}", ex.Message);
        //    //    return JsonConvert.SerializeObject(new ArtificialResultModel()
        //    //    {
        //    //        IsSuccess = false,
        //    //        Result = ex.Message
        //    //    });
        //    //}
        //    return null;
        //}

        ///// <summary>
        ///// 得到一个在线程中的处理窗口
        ///// </summary>
        //FormSpider GetProcessForm()
        //{
        //    FormSpider form = null;
        //    Thread newThread = new Thread(new ThreadStart(() =>
        //    {

        //        form = new FormSpider(_isShowForm, ExcuteRecord);
        //        try
        //        {
        //            form.StartAssist();
        //            form.ShowDialog();
        //        }
        //        catch (Exception ex)
        //        {
        //            //_log.FatalFormat("处理窗口异常:{0}", ex.Message);
        //        }
        //        finally
        //        {
        //            form.Close();
        //        }
        //    }));
        //    newThread.SetApartmentState(ApartmentState.STA);
        //    newThread.IsBackground = true; //随主线程一同退出
        //    newThread.Start();

        //    while (form == null)//等待form在异步线程中创建完毕
        //        Thread.Sleep(100);

        //    return form;
        //}

        /// <summary>
        /// 中断请求的监视
        /// </summary>
        //bool MonitorStopProcess(string guidkey)
        //{
        //    if (CommonCla.CacheIsHave(guidkey))
        //    {
        //        CommonCla.RemoveCache(guidkey);
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        //string ProcessAndResult(FormSpider form, string dataParam, ParamModel paramModel)
        //{
        //    var resultModel = new ResultModel();
        //    resultModel.IsSuccess = true;
        //    resultModel.Result = "666";
        //    try
        //    {
        //        //设置并执行相应操作，核心方法
        //        form.SetActionType(paramModel);

        //        try
        //        {
        //            //得到执行结果
        //            while (!CommonCla.IsTimeout(paramModel.StartTime, paramModel.Timeout))
        //            {
        //                resultModel.Result = form.Result;
        //                if (resultModel.Result == null)
        //                {
        //                    if (MonitorStopProcess(paramModel.StopKey))
        //                    {
        //                        resultModel.Result = ArtificialCode.A_RequestNormalBreak.ToString();
        //                        break;
        //                    }

        //                    if (form.IsDisposed)
        //                        throw new Exception(ArtificialCode.A_RequestAccidentBreak.ToString());
        //                    Thread.Sleep(100);
        //                }
        //                else
        //                {
        //                    resultModel.IsSuccess = true;
        //                    break;
        //                }
        //            }

        //            if (resultModel.Result == null)
        //                throw new Exception(ArtificialCode.A_TimeOutResult.ToString());
        //        }
        //        catch (Exception ex)//获取结果发生错误
        //        {
        //            resultModel.Result = ex.Message;
        //            //_log.ErrorFormat("耗时:{0}\r\n{1}\r\n{2}", CommonCla.GetMilliseconds(paramModel.StartTime), dataParam, resultModel.Result);
        //        }
        //    }
        //    catch (Exception ex)//解析参数发生错误
        //    {
        //        //如果Start发生异常
        //        form.IsWorking = false;

        //        resultModel.Result = ex.Message;
        //        //_log.FatalFormat("{0}\r\nStart()\r\n{1}", paramModel.Method, ex.Message);
        //    }
        //    finally
        //    {
        //        Task.Run(() =>
        //        {
        //            //本次执行完成,退出使用  //不阻塞执行,尽快返回结果
        //            form.Quit();
        //        });
        //    }

        //    return JsonConvert.SerializeObject(resultModel);

        //}
        protected override void OnClosed(EventArgs e)
        {
            this.Dispose();
            base.OnClosed(e);
        }

    }
}
