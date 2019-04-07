using CefSharp;
using CefSharp.WinForms;
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
    public partial class FormBrowser : Form
    {

        /// <summary>
        /// 窗口初始化
        /// </summary>
        /// <param name="isShow">是否显示</param>
        public FormBrowser(bool isShow = true)
        {
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
            this.调试ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(963, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 返回ToolStripMenuItem
            // 
            this.返回ToolStripMenuItem.Name = "返回ToolStripMenuItem";
            this.返回ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.返回ToolStripMenuItem.Text = "返回";
            // 
            // 前进ToolStripMenuItem
            // 
            this.前进ToolStripMenuItem.Name = "前进ToolStripMenuItem";
            this.前进ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.前进ToolStripMenuItem.Text = "前进";
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.刷新ToolStripMenuItem.Text = "刷新";
            // 
            // 首页ToolStripMenuItem
            // 
            this.首页ToolStripMenuItem.Name = "首页ToolStripMenuItem";
            this.首页ToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.首页ToolStripMenuItem.Text = "首页";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(250, 23);
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
            // FormBrowser
            // 
            this.ClientSize = new System.Drawing.Size(963, 546);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
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
            //this.webBrowser1.Navigate("https://www.baidu.com");
           this.browser = new Browser.MyWebBrowser("https://www.baidu.com")
            {
                
            };
            //this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Margin = new System.Windows.Forms.Padding(0,0,0,0);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "webBrowser1";
            this.browser.Size = new System.Drawing.Size(963, 519);
            this.browser.TabIndex = 1;
            this.Closing += OnClosing;
            this.browser.Load("https://www.ithome.com");
            this.Controls.Add(browser);
        }
        private void OnClosing(object sender, CancelEventArgs e)
        {
            Cef.Shutdown();
        }

        private void 调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
