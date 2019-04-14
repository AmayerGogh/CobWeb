using CobWeb.Core.Model;
using CobWeb.Core.Process;
using CobWeb.Util;
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

namespace CobWeb.Browser
{
    public partial class FormLog : Form
    {

        /// <summary>
        /// 标记是否关闭程序
        /// </summary>
        public static bool IsShutdown = false;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShowForm;

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
           //var browser = GetProcessForm();
           // browser.Show();
        }
        /// <summary>
        /// 窗口初始化
        /// </summary>
        /// <param name="isShow">是否显示</param>
        public FormLog(bool isShow = true)
        {
            InitializeComponent();
            this.IsShowForm = isShow;
            if (!IsShowForm)
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
                var text = string.Format("建立调用监听完成! {0} 端口:{1} 序号:{2}", MacUrl, Port, Number);               
                ExcuteRecord(text);
           

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
        public void ExcuteRecord(string msg)
        {            
            if (!IsDisposed && IsShowForm)
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

        FormBrowser _excuteForm = null;//主业务窗口最终来源
        readonly Object _objLock = new Object();



        protected override void OnClosed(EventArgs e)
        {
            IsShowForm = false;
            this.Dispose();
            base.OnClosed(e);
        }

    }
}
