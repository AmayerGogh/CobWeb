using CobWeb.Util;
using CobWeb.Util.SocketHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.Server
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class FormAdapter : Form
    {
        public FormAdapter()
        {
            
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Enabled = true;
            // this.Opacity = 0.1;
           // this.BackColor = Color.White; this.TransparencyKey = Color.White;
        }
      
        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
     
        public void Hello(string t)
        {
            MessageBox.Show("OK,html在调用wf中的函数");
        }
        public string Hello2(string t)
        {
           // MessageBox.Show("OK,html在调用wf中的函数");
            return "你输入了"+ t;
        }
        private void FormAdapter_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.ObjectForScripting = this;
            // this.webBrowser1.Url = new System.Uri(Application.StartupPath + "/test.html", System.UriKind.Absolute);
            //this.webBrowser1.Url = new System.Uri(@"D:\Amayer\learn_FrontEnd\_learnVueJs\11Element容器.html", System.UriKind.Absolute);

            //this.webBrowser1.Url = new System.Uri(@"http://cdn.ccode.com.cn/11Element%E5%AE%B9%E5%99%A8.html");
            this.webBrowser1.Url = new System.Uri(@"http://localhost:10086");
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //this.webBrowser1.Refresh();没用
        }
    }
}
