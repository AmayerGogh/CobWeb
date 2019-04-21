using mshtml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.Util.TridentHelper
{
    public static class WebBrowserExtensions
    {
        //IHTMLDocument2
        //IHTMLDocument1

        #region   执行命令
        public static void ExecCommandUtil(this HtmlDocument htmlDocument, string command)
        {
            // htmlDocument.ExecCommand("ClearAuthenticationCache", false, null);
            htmlDocument.ExecCommand(command, false, null);
        }
        #endregion

        //获取页面源码
        //webBrowser.Document.Body.InnerHtml
        //webBrowser.Document.GetElementById
        //htmlWindow = webBrowser.Document.Window.Frames[frameId];      //iframe内部元素为  htmlWindow 这个有点意思
        //htmlWindow.Document;
        //HtmlElement commForm = _document.Forms["commForm"];

        #region   获取页面源码
        public static string PageSource(this WebBrowser webBrowser)
        {
            try
            {
                return (webBrowser?.Document?.PageSource()) ?? string.Empty;
            }
            catch (Exception ee)
            {
                return string.Empty;
            }
        }
        public static string PageSource(this HtmlWindow htmlWindow)
        {
            try
            {
                return (htmlWindow?.Document?.PageSource()) ?? string.Empty;
            }
            catch (Exception ee)
            {
                return string.Empty;
            }
        }
        public static string PageSource(this HtmlDocument htmlDocument)
        {
            try
            {
                return (htmlDocument?.Body?.OuterHtml) ?? string.Empty;
            }
            catch (Exception ee)
            {
                return string.Empty;
            }
        }
        #endregion

        #region  获取元素

        /// <summary>
        ///  从当前集合中搜索符合条件的元素,返回第一个
        /// </summary>
        /// <param name="htmlElementCollection"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static HtmlElement GetElementByClassName(this HtmlElementCollection htmlElementCollection, string className)
        {
            if (htmlElementCollection == null || htmlElementCollection.Count <= 0)
                return null;

            foreach (HtmlElement htmlElement in htmlElementCollection)
            {
                try
                {
                    string stringClass = htmlElement.GetAttribute("className");

                    if (stringClass.NullContains(className))
                        return htmlElement;
                }
                catch (Exception ee)
                { }
            }

            return null;
        }

        /// <summary>
        /// 从元素htmlElement 的子元素中，搜索  name 元素集合
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HtmlElementCollection GetElementsByName(this HtmlElement htmlElement, string name)
        {
            return htmlElement?.All?.GetElementsByName(name);
        }

        /// <summary>
        ///  从当前集合中搜索符合条件的元素
        /// </summary>
        /// <param name="htmlElementCollection"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        public static HtmlElement GetElementByInnerText(this HtmlElementCollection htmlElementCollection, string innerText)
        {
            if (htmlElementCollection == null || htmlElementCollection.Count <= 0)
                return null;

            foreach (HtmlElement htmlElement in htmlElementCollection)
            {
                if (htmlElement.InnerText.IsNotNullOrEmpty() && htmlElement.InnerText == innerText)
                    return htmlElement;
            }

            return null;

        }

        /// <summary>
        ///  从当前集合中搜索符合条件的元素
        /// </summary>
        /// <param name="htmlElementCollection"></param>
        /// <returns></returns>
        public static HtmlElement FirstOrDefault(this HtmlElementCollection htmlElementCollection)
        {
            if (htmlElementCollection== null || htmlElementCollection.Count == 0) return null;

            return htmlElementCollection[0];

        }

        /// <summary>
        ///  从当前集合中搜索符合条件的元素
        /// </summary>
        /// <param name="htmlElementCollection"></param>
        /// <returns></returns>
        public static HtmlElement LastOrDefault(this HtmlElementCollection htmlElementCollection)
        {
            if (htmlElementCollection== null || htmlElementCollection.Count == 0) return null;

            return htmlElementCollection[htmlElementCollection.Count - 1];

        }

        /// <summary>
        /// 从当前集合中搜索符合条件的元素
        /// </summary>
        /// <param name="htmlElementCollection"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public static HtmlElement GetElementByAttribute(this HtmlElementCollection htmlElementCollection, string attributeName, string attributeValue)
        {
            if (htmlElementCollection== null || htmlElementCollection.Count <= 0)
                return null;

            foreach (HtmlElement item in htmlElementCollection)
            {
                if (item.HaveAttribute(attributeName, attributeValue))
                    return item;
            }

            return null;
        }

        /// <summary>
        /// 从当前集合中搜索符合条件的元素
        /// </summary>
        /// <param name="htmlElementCollection"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public static HtmlElement GetElementByValue(this HtmlElementCollection htmlElementCollection, string attributeValue)
        {
            return htmlElementCollection.GetElementByAttribute("value", attributeValue);
        }

        /// <summary>
        /// 从当前元素，或者子元素中寻找某一属性值的元素
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public static HtmlElement GetElementByAttribute(this HtmlElement htmlElement, string attributeName, string attributeValue)
        {
            if (htmlElement== null)
                return null;

            if (htmlElement.HaveAttribute(attributeName, attributeValue))
                return htmlElement;

            return htmlElement.All.GetElementByAttribute(attributeName, attributeValue);
        }

        public static HtmlElement GetElementByAttribute(this HtmlElement ele, string value)
        {
            return ele.GetElementByAttribute("ng-model", value);
        }

        #endregion

        #region 获取属性，获取值

        /// <summary>
        /// 当前元素是否拥有某一属性值
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public static bool HaveAttribute(this HtmlElement htmlElement, string attributeName, string attributeValue)
        {
            var value = htmlElement?.GetAttribute(attributeName);
            return value == attributeValue;
        }

        public static string GetAttributeNullOrEmpty(this HtmlElement ele, string keyattributeName)
        {
            if (ele == null)
            {
                return string.Empty;
            }
            return ele.GetAttribute(keyattributeName);
        }

        /// <summary>
        /// 选择是第几项
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int IndexOption(HtmlElement htmlElement, string s)
        {
            int num = -1;
            var options = htmlElement?.GetElementsByTagName("option");
            if (options!= null && options.Count > 0)
            {
                for (int i = 0; i < options.Count; i++)
                {
                    if (options[i].InnerText.NullContains(s))
                    {
                        num = i;
                        break;
                    }
                }
            }
            return num;
        }

        public static string GetValue(this HtmlElement htmlElement)
        {
            var value = htmlElement?.GetAttribute("value");
            return value;
        }

        public static bool Checked(this HtmlElement htmlElement)
        {
            return htmlElement.GetAttribute("checked").ToLower() == "true";
        }

        public static bool NoChecked(this HtmlElement htmlElement)
        {
            return htmlElement.GetAttribute("checked").ToLower() == "false";
        }

        #endregion

        #region  设置元素值

        public static void SetChecked(this HtmlElement htmlElement)
        {
            if (htmlElement.NoChecked())
                htmlElement.ClickE();
        }

        public static void SetNoChecked(this HtmlElement htmlElement)
        {
            if (htmlElement.Checked())
                htmlElement.ClickE();
        }

        /// <summary>
        /// 能设置 子元素的 内容
        /// </summary>
        public static void SetText(this HtmlElement htmlElement, string key, string value, string data, string eventName = "")
        {
            HtmlElement result = null;

            if (htmlElement.HaveAttribute(key, value))
            {
                result = htmlElement;
            }
            else
            {
                foreach (HtmlElement item in htmlElement.All)
                {
                    if (item.HaveAttribute(key, value))
                    {
                        result = item;
                        break;
                    }
                }
            }

            if (result!= null)
            {
                result.SetText(data, eventName);
            }
        }

        /// <summary>
        /// 优先使用这个
        /// </summary>
        public static void SetText(this HtmlElement ele, string data, string eventName = "")
        {
            if (ele!= null)
            {
                ele.InnerText = data;
                string excuteJSKey = string.IsNullOrEmpty(ele.GetAttributeNullOrEmpty("ui-valid-id").Trim()) ? "ng-model" : "ui-valid-id";

                //string excuteJSValue = string.IsNullOrEmpty(ele.GetAttribute("ui-valid-id").Trim()) ? ele.GetAttributeNullOrEmpty("ng-model") : ele.GetAttribute("ui-valid-id").Trim();
                string excuteJSValue = ele.GetAttribute(excuteJSKey).Trim();

                WebBrowserExtensions.JSFireEvent((IHTMLDocument2)ele.Document.DomDocument, ele.TagName, excuteJSKey, excuteJSValue, "change");
                WebBrowserExtensions.JSFireEvent((IHTMLDocument2)ele.Document.DomDocument, ele.TagName, excuteJSKey, excuteJSValue, "blur");

                if (eventName.IsNotNullOrEmpty())
                    WebBrowserExtensions.JSFireEvent((IHTMLDocument2)ele.Document.DomDocument, ele.TagName, excuteJSKey, excuteJSValue, eventName);

            }
        }

        /// <summary>
        /// 不会触发事件
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <param name="value"></param>
        public static void SetValue(this HtmlElement htmlElement, string value)
        {
            htmlElement.SetAttribute("value", value);
        }
        #endregion

        #region 点击元素
        public static void ClickE(this HtmlElement htmlElement)
        {
            if (htmlElement!= null)
            {
                htmlElement.InvokeMember("click");
            }
        }

        /// <summary>
        /// 用于点击radioButton
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="value"></param>
        public static void ClickE(this HtmlElement htmlElement, string value)
        {
            string js = @"        
var elementNames = document.getElementsByName('" + htmlElement.Name + @"');
for (var i = 0; i < elementNames.length; i++)
{
    if (elementNames[i].value == '" + value + @"')
    {
        elementNames[i].checked = true
    }
}";

            htmlElement.Document.ExecScript(js);
        }
        #endregion

        #region   获取js变量的值
        public const string outValueId = "outValueId";

        /// <summary>
        /// 目前仅支持一层的frame,如果需多层，需要外部调用
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="jsStr"></param>
        /// <param name="frameId"></param>
        /// <returns></returns>
        public static string GetValue(this WebBrowser webBrowser, string valueId)
        {
            var jsString = string.Empty;

            HtmlElement outValueIdHtmlElement = webBrowser.Document.GetElementById(outValueId);

            if (outValueIdHtmlElement== null)
            {
                jsString = @" 
    var outValueId = document.createElement('div');
    outValueId.innerText = '" + outValueId + @"';
    outValueId.id = '" + outValueId + @"';
    document.body.children[0].insertBefore(outValueId);
";
                ExecScript(webBrowser, jsString);
            }

            jsString = @"document.getElementById('" + outValueId + "').innerText=" + valueId;

            ExecScript(webBrowser, jsString);


            outValueIdHtmlElement = webBrowser.Document.GetElementById(outValueId);
            var result = outValueIdHtmlElement.InnerText;
            return result;
        }

        public static void SetValue(this WebBrowser webBrowser, string valueId, string value)
        {
            var jsString = valueId + "='" + value + "'";

            ExecScript(webBrowser, jsString);
        }
        #endregion

        #region 执行js

        #region js 触发事件  已整理


        /// <summary>
        /// 触发元素的事件
        /// </summary>
        /// <param name="doc">当前文档</param>
        /// <param name="tag">标签类型</param>
        /// <param name="key">元素属性名</param>
        /// <param name="value">元素属性值</param>
        /// <param name="eventStr">要触发的事件</param>
        public static void JSFireEvent(IHTMLDocument2 doc, string tag, string key, string value, string eventStr)
        {
            string proc = "(function () { var aElements = document.getElementsByTagName('" + tag + "'); for (var i = 0; i < aElements.length; i++) { if (aElements[i].getAttribute('" + key + "') == '" + value + "') { if ('createEvent' in document) { var evt = document.createEvent('HTMLEvents'); evt.initEvent('" + eventStr + "', false, true); aElements[i].dispatchEvent(evt); } else { aElements[i].fireEvent('on" + eventStr + "'); } break; } } })();";
            CommonWb.ExcuteJS(doc, proc);
        }

        /// <summary>
        /// 触发元素的事件
        /// </summary>
        public static void JSFireEvent(IHTMLDocument2 doc, string tag, string key, string value, string key2, string value2, string eventStr)
        {
            string proc = "(function () { var aElements = document.getElementsByTagName('" + tag + "'); for (var i = 0; i < aElements.length; i++) { if (aElements[i].getAttribute('" + key + "') == '" + value + "' &&  aElements[i].getAttribute('" + key2 + "') == '" + value2 + "') { if ('createEvent' in document) { var evt = document.createEvent('HTMLEvents'); evt.initEvent('" + eventStr + "', false, true); aElements[i].dispatchEvent(evt); } else { aElements[i].fireEvent('on" + eventStr + "'); } break; } } })();";
            CommonWb.ExcuteJS(doc, proc);
        }

        /// <summary>
        /// 触发元素的事件
        /// </summary>
        public static void JSFireEvent(IHTMLDocument2 doc, string parentTag, string parentKey, string parentValue, string tag, string key, string value, string eventStr)
        {
            CommonWb.ExcuteJS(doc, "(function () { var pEle = null; var pElements = document.getElementsByTagName('" + parentTag + "'); for (var i = 0; i < pElements.length; i++) { if (pElements[i].getAttribute('" + parentKey + "') == '" + parentValue + "') { pEle = pElements[i]; break; } } if (pEle == null) return; var aElements = pEle.getElementsByTagName('" + tag + "'); for (var i = 0; i < aElements.length; i++) { if (aElements[i].getAttribute('" + key + "') == '" + value + "') { if ('createEvent' in document) { var evt = document.createEvent('HTMLEvents'); evt.initEvent('" + eventStr + "', false, true); aElements[i].dispatchEvent(evt); } else { aElements[i].fireEvent('on" + eventStr + "'); } break; } } })();");
        }

        /// <summary>
        /// 触发元素的事件
        /// </summary>
        public static void JSFireEvent(IHTMLDocument2 doc, string id, string eventStr)
        {
            CommonWb.ExcuteJS(doc, "(function () { var ele = document.getElementById('" + id + "'); if ('createEvent' in document) { var evt = document.createEvent('HTMLEvents'); evt.initEvent('" + eventStr + "', false, true); ele.dispatchEvent(evt); } else { ele.fireEvent('on" + eventStr + "'); } })();");
        }

        /// <summary>
        /// 触发元素的事件
        /// </summary>
        public static void JSFireEventIE8(IHTMLDocument2 doc, string id, string eventStr)
        {
            CommonWb.ExcuteJS(doc, "document.getElementById('" + id + "')." + eventStr + "()");
        }

        /// <summary>
        /// 触发元素的事件
        /// </summary>
        public static void JSFireEvent_Name(IHTMLDocument2 doc, string name, string eventStr)
        {
            CommonWb.ExcuteJS(doc, "(function () { var ele = document.getElementsByName('" + name + "'); if ('createEvent' in document) { var evt = document.createEvent('HTMLEvents'); evt.initEvent('" + eventStr + "', false, true); ele.dispatchEvent(evt); } else { ele.fireEvent('on" + eventStr + "'); } })();");
        }


        /// <summary>
        ///触发元素的事件
        /// </summary>
        public static void JSFireEvent_Name(IHTMLDocument2 doc, string name, string value, string eventStr)
        {
            CommonWb.ExcuteJS(doc, "(function(){var arr = document.getElementsByName('" + name + "');var a;for(var i=0;i<arr.length;i++){if(arr[i].value=='" + value + "'){var a=arr[i]}};a." + eventStr + "();})()");
        }
        #endregion

        /// <summary>
        /// 执行jQuery (注意更新,不保证长期有效)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="jq"></param>
        public static void ExcutejQuery(HtmlDocument doc, string jq)
        {
            CommonWb.ExcuteJS((IHTMLDocument2)doc.DomDocument, jq);
        }

        public static void NavigateJs(this WebBrowser webBrowser, string jsStr)
        {
            string js = @"window.location.href='" + jsStr + "'";

            webBrowser.ExecScript(js);
        }

        #region 执行js 
        /// <summary>
        /// 目前仅支持一层的frame,如果需多层，需要外部调用
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="jsStr"></param>
        /// <param name="frameId"></param>
        /// <returns></returns>
        public static void ExecScript(this WebBrowser webBrowser, string jsStr, string frameId = null)
        {
            //HtmlElement commForm = htmlDocument.Forms["commForm"];
            //webBrowser.Document.Window.Frames["main"].Document

            HtmlDocument htmlDocument = webBrowser.Document;

            if (frameId!= null)
            {
                HtmlWindow htmlWindow;
                try
                {
                    htmlWindow = webBrowser.Document.Window.Frames[frameId];
                    htmlWindow.ExecScript(jsStr);
                }
                catch (Exception ee)
                {
                   
                }
            }
            else
            {
                htmlDocument.ExecScript(jsStr);
            }
        }

        public static void ExecScript(this HtmlWindow htmlWindow, string jsStr)
        {
            htmlWindow.Document.ExecScript(jsStr); ;

        }

        public static void ExecScript(this HtmlDocument htmlDocument, string jsStr)
        {
            IHTMLDocument2 iHTMLDocument2 = (IHTMLDocument2)htmlDocument.DomDocument;
            //CommonCla.WriteLogFile(jsStr);
            iHTMLDocument2.ExecScript(jsStr);
            //CommonCla.WriteLogFile("执行完成");
        }

        public static void ExecScript(this IHTMLDocument2 document, string jsStr)
        {
            document.parentWindow.ExecScript(jsStr);
        }

        static void ExecScript(this IHTMLWindow2 iHTMLWindow2, string jsStr)
        {
            iHTMLWindow2.execScript(jsStr, "javascript");
        }


        #endregion

        #endregion

        #region 删除IE临时文件
        /// <summary> 
        /// 删除临时文件
        /// </summary>
        /// <param name="name">完整或缺省的文件名称</param>
        public static void CleanTempFiles(string name = "")
        {
            FolderClear(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), name);
        }

        static void FolderClear(string path, string name = "")
        {
            DirectoryInfo diPath = new DirectoryInfo(path);
            foreach (FileInfo fiCurrFile in diPath.GetFiles())
            {
                FileDelete(fiCurrFile.FullName, name);
            }
            foreach (DirectoryInfo diSubFolder in diPath.GetDirectories())
            {
                FolderClear(diSubFolder.FullName, name);
            }
        }

        static bool FileDelete(string path, string name = "")
        {
            FileInfo file = new FileInfo(path);
            FileAttributes att = 0;
            bool attModified = false;

            try
            {
                if (string.IsNullOrWhiteSpace(name) || file.Name.Contains(name))
                {
                    att = file.Attributes;
                    file.Attributes &= (~FileAttributes.ReadOnly);
                    attModified = true;
                    file.Delete();
                }
            }
            catch (Exception e)
            {
                if (attModified)
                    file.Attributes = att;
                return false;
            }
            return true;
        }

        ///// <summary>
        ///// 删除所有临时文件
        ///// </summary>
        //public static void CleanAllTempFiles()
        //{
        //    //命令行清除
        //    RunCmd("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8");
        //}

        //static void RunCmd(string cmd)
        //{
        //    ProcessStartInfo process = new ProcessStartInfo();
        //    process.CreateNoWindow = false;
        //    process.UseShellExecute = false;
        //    process.WindowStyle = ProcessWindowStyle.Hidden;
        //    process.FileName = "cmd.exe";
        //    process.Arguments = "/c " + cmd;
        //    Process.Start(process);
        //}



        #endregion

        #region cookie
        private const int INTERNET_COOKIE_HTTPONLY = 0x00002000;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            int flags,
            IntPtr pReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        /// <summary>
        /// 设置cookie
        /// </summary>
        public static void SetCookie(string url, Dictionary<string, string> cookies)
        {
            foreach (var cookie in cookies)
            {
                InternetSetCookie(url, cookie.Key, cookie.Value);
            }
        }

        /// <summary>
        /// 设置cookie
        /// </summary>
        public static void SetCookie(string url, string cookiesString)
        {
            var cookieAarray = cookiesString.Split(';');

            foreach (var cookie in cookieAarray)
            {
                var temp = cookie.Split('=');

                if (temp.Length == 2)
                {
                    InternetSetCookie(url, temp[0], temp[1]);
                }
            }
        }

        /// <summary>
        /// 获取完整cookie
        /// </summary>
        /// <param name="url">当前处于登录状态</param>
        /// <returns></returns>
        public static string GetCookie(string url)
        {
            int size = 512;
            StringBuilder sb = new StringBuilder(size);
            if (!InternetGetCookieEx(url, null, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero))
            {
                if (size < 0)
                {
                    return null;
                }
                sb = new StringBuilder(size);
                if (!InternetGetCookieEx(url, null, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero))
                {
                    return null;
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
