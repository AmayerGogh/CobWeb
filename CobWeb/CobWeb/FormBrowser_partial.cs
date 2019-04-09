using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CobWeb
{
    public partial class FormBrowser
    {
        private MainForm _mainForm;

        MainForm ShowMainForm()
        {
            if (_mainForm != null)
            {
                if (!_mainForm.IsDisposed)
                {
                    //_mainForm.Show();
                    _mainForm.Focus();
                    return _mainForm;
                }
            }
            //Thread newThread = new Thread(new ThreadStart(() =>
            //{
            _mainForm = new MainForm();
            _mainForm.Show();

            //}));
            //newThread.SetApartmentState(ApartmentState.STA);
            //newThread.IsBackground = true; //随主线程一同退出
            //newThread.Start();

            return _mainForm;
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
