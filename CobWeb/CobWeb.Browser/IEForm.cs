using CobWeb.Core;
using CobWeb.Core.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebKit;


namespace CobWeb.Browser
{
    public class IEForm : FormBrowser
    {
        /// <summary>
        ///  与base中的KernelControl 为同一个对象
        ///  base.KernelControl.Equals(this.browser);
        /// </summary>
        public TridentKernelControl browser;
        public IEForm(TridentKernelControl browser, bool isShow = true) : base(browser, isShow)
        {                   
            this.browser = browser;
         
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "webBrowser1";
            this.browser.Size = new System.Drawing.Size(963, 519);
            this.browser.TabIndex = 1;
            this.browser.Navigate("http://www.baidu.com");

            this.browser.DocumentCompleted += webBrowser_DocumentCompleted;
            this.browser.WBDocHostShowUIShowMessage += webBrowser_WBDocHostShowUIShowMessage;
            this.browser.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowser_NewWindow);
            this.browser.Dock = DockStyle.Fill;
            panel1.Controls.Add(this.browser);

            ExcuteRecord("加载完成了");
           
        }
        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (isShowForm)
                this.Text = this. browser.DocumentTitle;
        }
        void webBrowser_WBDocHostShowUIShowMessage(object sender, ExtendedBrowserMessageEventArgs e)
        {
            e.Cancel = !isShowForm || IsWorking();
        }
        //强制本页面打开
        private void webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            string url = ((WebBrowser)sender).StatusText;
            this.browser.Navigate(url);
            e.Cancel = true;
        }
    }
}
