using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util.Control
{
    public class CefBrowser : ChromiumWebBrowser, IBrowserBase
    {
        public CefBrowser(string address, IRequestContext requestContext = null) : base(address, requestContext)
        {
            this.LifeSpanHandler = new CefLifeSpanHandler();
            this.LoadHandler = new LoadHandler();
            //MenuHandler = new MenuHandler();
            
        }
        public event EventHandler<NewWindowEventArgs> StartNewWindow;

        public new IBrowserBase GetBrowser()
        {
            return this;
        }

        public void OnNewWindow(NewWindowEventArgs e)
        {
            StartNewWindow?.Invoke(this, e);
        }

        public void Navigate(string url)
        {
            this.Load(url);
        }
        public void RunJs(string js)
        {
            if (InvokeRequired) { Invoke(new runJSDelegate(RunJs), new object[] { js }); return; }
            if (IsBrowserInitialized || IsDisposed || Disposing) { return; }
            //ExecuteScriptAsync(js); //此为扩展方法
            
        }
        delegate void runJSDelegate(string jsCodeStr);
        /// <summary>
        /// 浏览器执行JS代码获取返回值
        /// </summary>
        /// <param name="script"></param>
        /// <param name="defaultValue"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <example>同步：EvaluateScript("5555 * 19999 + 88888", 0, TimeSpan.FromSeconds(3)).GetAwaiter().GetResult();</example>
        public async Task<object> EvaluateScript(string script, object defaultValue, TimeSpan timeout)
        {
            object result = defaultValue;
            if (IsBrowserInitialized && !IsDisposed && !Disposing)
            {
                try
                {
                    //此为扩展方法
                    //var task = EvaluateScriptAsync(script, timeout);
                    //await task.ContinueWith(res =>
                    //{
                    //    if (!res.IsFaulted)
                    //    {
                    //        var response = res.Result;
                    //        result = response.Success ? (response.Result ?? "null") : response.Message;
                    //    }
                    //}).ConfigureAwait(false); // <-- This makes the task to synchronize on a different context
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
            return result;
        }

        public void SetCookie(string url, string cookiesString)
        {
            //if (string.IsNullOrWhiteSpace(cookiesString))
            //{
            //    return;
            //}
            //var cookieAarray = cookiesString.Split(';');
            //var cookieManager = GetCookieManager(); //此为扩展方法

            //try
            //{
            //    foreach (var cookie in cookieAarray)
            //    {

            //        //var temp = cookie.Split('=');
            //        var i = cookie.IndexOf('=');
            //        if (i != 0)
            //        {
            //            var single = new CefSharp.Cookie()
            //            {
            //                Name = cookie.Substring(0, i).Trim(),
            //                Value = cookie.Substring(i + 1),
            //                Domain = url,
            //                Path = "/",
            //                Expires = DateTime.MinValue
            //            };
            //            cookieManager.SetCookie("http://" + url, single);
            //        }
            //    }
            //}
            //catch (Exception e)
            //{

            //}
        }
        public void SetCookie(string url, List<CookiePseudo> cookies)
        {
            //var cookieManager = browser.GetCookieManager();
            //foreach (var item in cookies)
            //{
            //    var cookie = new CefSharp.Cookie()
            //    {
            //        Creation = item.Creation,
            //        Domain = item.Domain,
            //        Expires = item.Expires,
            //        HttpOnly = item.HttpOnly,
            //        LastAccess = item.LastAccess,
            //        Name = item.Name,
            //        Path = item.Path,
            //        Secure = item.Secure,
            //        Value = item.Value
            //    };
            //    cookieManager.SetCookie("http://" + url, cookie);
            //}
        }
        public List<CookiePseudo> GetCurrentCookie(string url)
        {
            cookieList = new List<CookiePseudo>();
            //var cookieManager = browser.GetCookieManager();
            //var cook = new CookieVisitor();
            ////var c = cookieManager.VisitAllCookies(cook);//url,true,cook
            //cookieManager.VisitUrlCookies("http://" + url, true, cook);
            ////cookieList = null;
            //while (!cook.IsDispose)
            //{
            //    Thread.Sleep(10);
            //}
            return cookieList;
        }
        public static List<CookiePseudo> cookieList;
        class CookieVisitor : ICookieVisitor
        {
            public bool IsDispose;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="cookie"></param>
            /// <param name="count">第几个</param>
            /// <param name="total"></param>
            /// <param name="deleteCookie"></param>
            /// <returns></returns>
            public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool deleteCookie)
            {
                cookieList.Add(new CookiePseudo()
                {
                    Creation = cookie.Creation,
                    Domain = cookie.Domain,
                    Expires = cookie.Expires,
                    HttpOnly = cookie.HttpOnly,
                    LastAccess = cookie.LastAccess,
                    Name = cookie.Name,
                    Path = cookie.Path,
                    Secure = cookie.Secure,
                    Value = cookie.Value
                });
                return true;
            }
            public void Dispose()
            {
                IsDispose = true;
            }
        }


        public void Bound()
        {
            ////前端注册js方法
            //this.browser.RegisterJsObject("bound",new object ());
            ////js中调用
            ////bound.xxx
        }
    }

    #region 新窗口打开
    public class CefLifeSpanHandler : CefSharp.ILifeSpanHandler
    {


        public bool DoClose(IWebBrowser browserControl, CefSharp.IBrowser browser)
        {
            if (browser.IsDisposed || browser.IsPopup)
            {
                return false;
            }

            return true;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            var chromiumWebBrowser = (CefBrowser)browserControl;

            chromiumWebBrowser.Invoke(new Action(() =>
            {
                NewWindowEventArgs e = new NewWindowEventArgs(windowInfo, targetUrl);
                chromiumWebBrowser.OnNewWindow(e);
            }));

            newBrowser = null;
            return true;
        }
    }
    public class NewWindowEventArgs : EventArgs
    {
        private IWindowInfo _windowInfo;
        public IWindowInfo WindowInfo
        {
            get { return _windowInfo; }
            set { value = _windowInfo; }
        }
        public string url { get; set; }
        public NewWindowEventArgs(IWindowInfo windowInfo, string url)
        {
            _windowInfo = windowInfo;
            this.url = url;
        }
    }
    #endregion


    #region 右键
    class MenuHandler : CefSharp.IContextMenuHandler
    {
        void CefSharp.IContextMenuHandler.OnBeforeContextMenu(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame, CefSharp.IContextMenuParams parameters, CefSharp.IMenuModel model)
        {
            model.Clear();
        }

        bool CefSharp.IContextMenuHandler.OnContextMenuCommand(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame, CefSharp.IContextMenuParams parameters, CefSharp.CefMenuCommand commandId, CefSharp.CefEventFlags eventFlags)
        {
            //throw new NotImplementedException();
            return false;
        }

        void CefSharp.IContextMenuHandler.OnContextMenuDismissed(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame)
        {
            //throw new NotImplementedException();
        }

        bool CefSharp.IContextMenuHandler.RunContextMenu(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame, CefSharp.IContextMenuParams parameters, CefSharp.IMenuModel model, CefSharp.IRunContextMenuCallback callback)
        {
            return false;
        }
    }
    #endregion




    public class LoadHandler : ILoadHandler
    {
        public void OnFrameLoadEnd(IWebBrowser browserControl, FrameLoadEndEventArgs frameLoadEndArgs)
        {
            // browserControl.ExecuteScriptAsync("");
        }

        public void OnFrameLoadStart(IWebBrowser browserControl, FrameLoadStartEventArgs frameLoadStartArgs)
        {
            // Console.WriteLine("Start Load: " + browserControl.Address);
        }

        public void OnLoadError(IWebBrowser browserControl, LoadErrorEventArgs loadErrorArgs)
        {

        }

        public void OnLoadingStateChange(IWebBrowser browserControl, LoadingStateChangedEventArgs loadingStateChangedArgs)
        {

        }
    }



}
