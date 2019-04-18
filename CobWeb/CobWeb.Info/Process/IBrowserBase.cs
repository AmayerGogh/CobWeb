using CefSharp;
using CobWeb.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Core.Process
{
    public interface IBrowserBase
    {
        void Stop();
        void Close();


        string GetResult();
        void SetResult(string result);
        bool IsWorking();
        void SetWorking(bool iswork);
        bool IsShowForm();

        void ExcuteRecord(string txt);
        void AddAssistAction(Action action);
        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="address"></param>
        void Navigate(string address);

        /// <summary>
        /// 执行js
        /// </summary>
        /// <param name="js"></param>
        void ExecuteScript(string js);
        /// <summary>
        /// 有返回值的js执行
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        Task<JavascriptResponse> EvaluateScriptAsync(string js);


        void SetCookie(string url, string cookiesString);


        void SetCookie(string url, List<CookiePseudo> cookies);


        string GetCurrentCookieStr(string url);


        List<CookiePseudo> GetCurrentCookie(string url);

        void Test(string s);


    }
}
