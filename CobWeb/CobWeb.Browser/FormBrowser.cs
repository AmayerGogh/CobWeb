using CefSharp;
using CefSharp.WinForms;
using CobWeb.Browser;
using CobWeb.Core.Process;
using CobWeb.Util.Control;
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
    public  class FormBrowser2
    {
        
        /// <summary>
        /// 窗口初始化
        /// </summary>
        /// <param name="isShow">是否显示</param>
        public FormBrowser2(bool isShow = true)
        {
            ////_excuteRecord = record;
            ////_mainForm = form;
            //isShowForm = isShow;
            //BrowserInit();
            //InitializeComponent();
            //ShowLogForm();
            //Step1_GetSetting();
            //Step2_StartListen();
            //StartAssist();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormBrowser2
            // 
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "FormBrowser2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormBrowser_FormClosed);
            this.Load += new System.EventHandler(this.FormBrowser_Load);
            this.ResumeLayout(false);

        }
        private MyWebKitBrowser browser;
        private void FormBrowser_Load(object sender, EventArgs e)
        {
            //Navigate(@"www.baidu.com");
            this.panel1.Controls.Add(browser);

        }



        private void Browser_StartNewWindow(object sender, NewWindowEventArgs e)
        {
            
        }
        private void Browser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            //this.Text =this.browser
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Refresh();
        }

        private void 返回ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // this.browser.Undo();
        }

        private void 前进ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.browser.Forward();
        }
        private void 首页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.browser.Load(@"file:///D:\Code\CobWeb\CobWeb\CobWeb\bin\Debug\html\test.html");//file:///D:/Amayer/CobWeb/CobWeb/CobWeb/bin/Debug/html/05.html
        }
        private void ToolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void 通信ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }
        private void 调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //browser.ShowDevTools();
        }


        void BrowserInit()
        {
            this.browser = new MyWebKitBrowser()
            {

            };
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "webBrowser1";
            this.browser.Size = new System.Drawing.Size(963, 519);
            this.browser.TabIndex = 1;
            this.browser.Navigate("http://www.baidu.com");

            //this.browser.StartNewWindow += Browser_StartNewWindow;
            //this.browser.TitleChanged += Browser_TitleChanged; //new EventHandler<TitleChangedEventArgs> 
            //this.browser.FrameLoadEnd += Browser_FrameLoadEnd;
            //this.browser.FrameLoadStart += Browser_FrameLoadStart;

            //this.browser.LoadHandler = new LoadHandler();
            //if (CefSharpSettings.ShutdownOnExit)
            //{
            //    Application.ApplicationExit += OnApplicationExit;
            //}
            
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Cef.Shutdown();
        }


        private void FormBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsDisposed = true;
            ClearWebBrowser();
            Dispose();
        }

        private void Browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            this.Name = "加载中" + this.Name;
        }
        /// <summary>
        /// 每一项加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            this.toolStripTextBox1.Text = e.Url;
            //this.Text = this.browser.Address;  
            
            //获取网页代码
            //var result = this.browser.GetSourceAsync().Result;

        }
    }

}
