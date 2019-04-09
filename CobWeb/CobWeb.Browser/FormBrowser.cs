using CefSharp;
using CefSharp.WinForms;
using CobWeb.Browser;
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
    public partial class FormBrowser : Form
    {

        /// <summary>
        /// 窗口初始化
        /// </summary>
        /// <param name="isShow">是否显示</param>
        public FormBrowser(bool isShow, Action<string> record,Form form)
        {
            _excuteRecord = record;
            _mainForm = form;
            IsShowForm = isShow;
            BrowserInit();
            InitializeComponent();
        }
       
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.返回ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.前进ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.首页ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通信ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.返回ToolStripMenuItem,
            this.前进ToolStripMenuItem,
            this.刷新ToolStripMenuItem,
            this.首页ToolStripMenuItem,
            this.toolStripTextBox1,
            this.调试ToolStripMenuItem,
            this.通信ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 返回ToolStripMenuItem
            // 
            this.返回ToolStripMenuItem.Name = "返回ToolStripMenuItem";
            this.返回ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.返回ToolStripMenuItem.Text = "返回";
            this.返回ToolStripMenuItem.Click += new System.EventHandler(this.返回ToolStripMenuItem_Click);
            // 
            // 前进ToolStripMenuItem
            // 
            this.前进ToolStripMenuItem.Name = "前进ToolStripMenuItem";
            this.前进ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.前进ToolStripMenuItem.Text = "前进";
            this.前进ToolStripMenuItem.Click += new System.EventHandler(this.前进ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 首页ToolStripMenuItem
            // 
            this.首页ToolStripMenuItem.Name = "首页ToolStripMenuItem";
            this.首页ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.首页ToolStripMenuItem.Text = "首页";
            this.首页ToolStripMenuItem.Click += new System.EventHandler(this.首页ToolStripMenuItem_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(250, 23);
            this.toolStripTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ToolStripTextBox1_KeyDown);
            // 
            // 调试ToolStripMenuItem
            // 
            this.调试ToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.调试ToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.调试ToolStripMenuItem.Name = "调试ToolStripMenuItem";
            this.调试ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.调试ToolStripMenuItem.Text = "调试";
            this.调试ToolStripMenuItem.Click += new System.EventHandler(this.调试ToolStripMenuItem_Click);
            // 
            // 通信ToolStripMenuItem
            // 
            this.通信ToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.通信ToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.通信ToolStripMenuItem.Name = "通信ToolStripMenuItem";
            this.通信ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.通信ToolStripMenuItem.Text = "通信";
            this.通信ToolStripMenuItem.Click += new System.EventHandler(this.通信ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 634);
            this.panel1.TabIndex = 1;
            // 
            // FormBrowser
            // 
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "FormBrowser";
            this.Load += new System.EventHandler(this.FormBrowser_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Browser.MyWebBrowser browser;
        private void FormBrowser_Load(object sender, EventArgs e)
        {            
            this.browser.Load(@"file:///D:\Code\CobWeb\CobWeb\CobWeb\bin\Debug\html\test.html");//file:///D:/Amayer/CobWeb/CobWeb/CobWeb/bin/Debug/html/05.html
            this.panel1.Controls.Add(browser);
            //this.webBrowser1.Navigate("https://www.baidu.com");                               
        }
        private void OnClosing(object sender, CancelEventArgs e)
        {
            Cef.Shutdown();
        }

       
        private void Browser_StartNewWindow(object sender, NewWindowEventArgs e)
        {
            Navigate(e.url);
        }
        private void Browser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            //this.Text =this.browser
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.browser.Refresh();
        }

        private void 返回ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.browser.Undo();
        }

        private void 前进ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.browser.Forward();
        }
        private void 首页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.browser.Load(@"file:///D:\Code\CobWeb\CobWeb\CobWeb\bin\Debug\html\test.html");//file:///D:/Amayer/CobWeb/CobWeb/CobWeb/bin/Debug/html/05.html
        }
        private void ToolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Navigate(toolStripTextBox1.Text);
            }
        }

        private void Navigate(String address)
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
                this.browser.Load(address);
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }      
       
        private void 通信ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMainForm();
        }
        private void 调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            browser.ShowDevTools();
        }

        Form _mainForm;


        void ShowMainForm()
        {            
            _mainForm.Show();
        }
        void BrowserInit()
        {
            this.browser = new Browser.MyWebBrowser("about:blank");
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "webBrowser1";
            this.browser.Size = new System.Drawing.Size(963, 519);
            this.browser.TabIndex = 1;
            this.Closing += OnClosing;
            this.browser.StartNewWindow += Browser_StartNewWindow;
            this.browser.TitleChanged += Browser_TitleChanged; //new EventHandler<TitleChangedEventArgs> 
        }
    }
}
