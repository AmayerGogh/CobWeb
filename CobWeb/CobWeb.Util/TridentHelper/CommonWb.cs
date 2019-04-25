using mshtml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace CobWeb.Util.TridentHelper
{
    public class CommonWb
    {
        #region 删除IE临时文件
        /// <summary> 
        /// 删除临时文件
        /// </summary>
        /// <param name="name">完整或缺省的文件名称</param>
        public static void CleanTempFiles(string name = "")
        {
            FolderClear(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), name);
        }
        /// <summary>
        /// 删除所有临时文件
        /// </summary>
        public static void CleanAllTempFiles()
        {
            //命令行清除
            RunCmd("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8");
        }
        static void RunCmd(string cmd)
        {
            ProcessStartInfo process = new ProcessStartInfo();
            process.CreateNoWindow = false;
            process.UseShellExecute = false;
            process.WindowStyle = ProcessWindowStyle.Hidden;
            process.FileName = "cmd.exe";
            process.Arguments = "/c " + cmd;
            Process.Start(process);
        }
        static void FolderClear(string path, string name = "")
        {
            DirectoryInfo diPath = new DirectoryInfo(path);
            try
            {
                foreach (FileInfo fiCurrFile in diPath.GetFiles())
                {
                    FileDelete(fiCurrFile.FullName, name);
                }
            }
            catch
            {
            }
            try
            {
                foreach (DirectoryInfo diSubFolder in diPath.GetDirectories())
                {
                    FolderClear(diSubFolder.FullName, name);
                }
            }
            catch (UnauthorizedAccessException)
            {
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
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            {
                if (attModified)
                    file.Attributes = att;
                return false;
            }
            return true;
        }
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
        /// <summary>
        /// 根据name找到元素
        /// </summary>
        public static IHTMLElement GetElementByName(IHTMLElement element, string name)
        {
            foreach (IHTMLElement ele in element.all)
            {
                if (ele.getAttribute("name") != null && ele.getAttribute("name").ToString() == name)
                {
                    return ele;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据name找到元素
        /// </summary>
        public static IHTMLElement GetElementByName(IHTMLDocument2 doc2, string name)
        {
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (ele.getAttribute("name") != null && ele.getAttribute("name").ToString() == name)
                {
                    return ele;
                }
            }
            return null;
        }
        /// <summary>
        /// 设置value值
        /// </summary>
        public static void SetValue(IHTMLElement ele, string value)
        {
            ele.setAttribute("value", value);
        }
        /// <summary>
        /// 得到value值
        /// </summary>
        public static string GetValue(IHTMLElement ele)
        {
            return ele.getAttribute("value");
        }
        /// <summary>
        /// 触发ondblclick事件
        /// </summary>
        public static void OnDbClick(IHTMLElement ele)
        {
            (ele as IHTMLElement3).FireEvent("ondblclick");
        }
        /// <summary>
        /// 触发onclick事件
        /// </summary>
        public static void OnClick(IHTMLElement ele)
        {
            (ele as IHTMLElement3).FireEvent("onclick");
        }
        /// <summary>
        /// 触发onchange事件
        /// </summary>
        public static void OnChange(IHTMLElement ele)
        {
            (ele as IHTMLElement3).FireEvent("onchange");
        }
        /// <summary>
        /// 触发onblur事件
        /// </summary>
        public static void OnBlur(IHTMLElement ele)
        {
            (ele as IHTMLElement3).FireEvent("onblur");
        }
        /// <summary>
        /// 设置值并触发onchange事件
        /// </summary>
        public static void SetValueOnChange(IHTMLElement ele, string value)
        {
            SetValue(ele, value);
            OnChange(ele);
        }
        /// <summary>
        /// 设置值并触发onblur事件
        /// </summary>
        public static void SetValueOnBlur(IHTMLElement ele, string value)
        {
            SetValue(ele, value);
            OnBlur(ele);
        }
        /// <summary>
        /// 得到下拉框text值
        /// </summary>
        public static string GetSelectText(dynamic select)
        {
            foreach (var option in select.getElementsByTagName("option"))
            {
                if (option.getAttribute("selected"))
                {
                    return option.InnerText;
                }
            }
            return null;
        }
        /// <summary>
        /// 得到下拉框value值
        /// </summary>
        public static string GetSelectValue(dynamic select)
        {
            foreach (var option in select.getElementsByTagName("option"))
            {
                if (option.getAttribute("selected"))
                {
                    return option.value;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据InnerText设置下拉框
        /// </summary>
        public static bool SetSelectByInnerText(dynamic select, string innerText)
        {
            bool isOk = false;
            foreach (var option in select.getElementsByTagName("option"))
            {
                if (option.InnerText == innerText)
                {
                    option.SetAttribute("selected", "selected");
                    isOk = true;
                }
                else
                    option.SetAttribute("selected", "");
            }
            select.FireEvent("onchange");
            return isOk;
        }
        /// <summary>
        /// 根据value设置下拉框
        /// </summary>
        public static bool SetSelectByValue(dynamic select, string value)
        {
            bool isOk = false;
            foreach (var option in select.getElementsByTagName("option"))
            {
                if (option.value == value)
                {
                    option.SetAttribute("selected", "selected");
                    isOk = true;
                }
                else
                    option.SetAttribute("selected", "");
            }
            select.FireEvent("onchange");
            return isOk;
        }
        /// <summary>
        /// 下载图片
        /// 超时间单位:秒
        /// </summary>
        public static Bitmap DownloadBitmap(string url, int timeout = 15, string cookie = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 1000 * timeout;
                request.Headers["Cookie"] = cookie;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream requestStream = response.GetResponseStream();
                if (requestStream != null)
                {
                    Bitmap sourceBm = new Bitmap(requestStream);
                    requestStream.Dispose();
                    response.Dispose();
                    return sourceBm;
                }
            }
            catch { }
            return null;
        }
        /// <summary>
        /// 执行js
        /// </summary>
        public static void ExcuteJS(IHTMLDocument2 doc, string jsStr)
        {
            //CommonCla.WriteLogFile("CommonWb.ExcuteJS" + jsStr);
            doc.parentWindow.execScript(jsStr, "javascript");
            //CommonCla.WriteLogFile("执行完成");
        }
        /// <summary>
        /// 执行js
        /// </summary>
        public static void ExcuteJS(HTMLWindow2 win, string jsStr)
        {
            win.document.parentWindow.execScript(jsStr, "javascript");
        }
        /// <summary>
        /// 获取checked 属性
        /// </summary>
        /// <param name="ele"></param>
        /// <returns></returns>
        public static bool GetCheckboxSelected(IHTMLElement ele)
        {
            if (ele != null && ele.getAttribute("checked") == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 设置复选框 选中与否
        /// </summary>
        /// <param name="ele">复选框</param>
        /// <param name="check">ture选中 ,false 取消选中</param>
        public static void CheckOrUncheck(IHTMLElement ele, bool check)
        {
            try
            {
                //不会触发选中事件,可能有问题
                //((HTMLInputElement)ele).@checked = check;
                if (check)
                {
                    //没有选中,则选中
                    if (!GetCheckboxSelected(ele))
                    {
                        ele.click();
                    }
                }
                else
                {
                    //选中,则取消
                    if (GetCheckboxSelected(ele))
                    {
                        ele.click();
                    }
                }
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
            }
        }
        #region 扩展
        /// <summary>
        /// 获取radio的选中值
        /// </summary>
        /// <returns></returns>
        public static string GetRadioCheckValue(IHTMLDocument2 doc2, string radioName)
        {
            var result = string.Empty;
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (ele.getAttribute("name") != null && ele.getAttribute("name").ToString() == radioName)
                {
                    if (ele.getAttribute("Checked"))
                    {
                        result = ele.getAttribute("value");
                        break;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 根据class找元素
        /// </summary>
        public static IHTMLElement GetElementByClass(IHTMLElement element, string className, bool isContain = false)
        {
            foreach (IHTMLElement ele in element.all)
            {
                if (isContain)
                {
                    if (ele.className != null && ele.className.Contains(className))
                    {
                        return ele;
                    }
                }
                else
                {
                    if (ele.className != null && ele.className == className)
                    {
                        return ele;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 根据class找元素
        /// </summary>
        public static IHTMLElement GetElementByClass(IHTMLDocument2 doc2, string className, bool isContain = false)
        {
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (isContain)
                {
                    if (ele.className != null && ele.className.Contains(className))
                    {
                        return ele;
                    }
                }
                else
                {
                    if (ele.className != null && ele.className == className)
                    {
                        return ele;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 根据className 找元素列表
        /// </summary>
        public static List<IHTMLElement> GetElementListByClassName(IHTMLDocument2 doc2, string className, bool isCantain = false)
        {
            List<IHTMLElement> eList = new List<IHTMLElement>();
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (isCantain)
                {
                    if (ele != null && ele.className.Contains(className))
                    {
                        eList.Add(ele);
                    }
                }
                else
                {
                    if (ele != null && ele.className == className)
                    {
                        eList.Add(ele);
                    }
                }
            }
            return eList;
        }
        /// <summary>
        /// 根据id找元素
        /// </summary>
        public static IHTMLElement GetElementById(IHTMLElement element, string id)
        {
            foreach (IHTMLElement ele in element.all)
            {
                if (ele.id != null && ele.id == id)
                {
                    return ele;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据id找元素
        /// </summary>
        public static IHTMLElement GetElementById(IHTMLDocument2 doc2, string id)
        {
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (ele.id != null && ele.id == id)
                {
                    return ele;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据属性和属性值找元素
        /// </summary>
        public static IHTMLElement GetElementByAttribute(IHTMLDocument2 doc2, string attributeName, string attributeValue)
        {
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (ele.getAttribute(attributeName) != null && ele.getAttribute(attributeName).ToString() == attributeValue)
                {
                    return ele;
                }
            }
            return null;
        }
        static System.Text.RegularExpressions.Regex TrimSpaceRegex = new System.Text.RegularExpressions.Regex(@"\s+", System.Text.RegularExpressions.RegexOptions.Compiled);
        /// <summary>
        /// 根据Value值 找元素 
        /// </summary>
        /// <param name="doc2"></param> 
        /// <param name="valueName"> 直接传 value值 (可以是未trim的字符串) </param>
        /// <returns></returns>
        public static IHTMLElement GetElementByTrimedValueName(IHTMLDocument2 doc2, string valueName)
        {
            valueName = TrimSpaceRegex.Replace(valueName, "");
            //定义正则
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (ele.getAttribute("value") != null && TrimSpaceRegex.Replace(ele.getAttribute("value").ToString(), "") == valueName)
                {
                    return ele;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据Value值 找元素 
        /// </summary>
        /// <param name="doc2"></param> 
        /// <param name="valueName"> 直接传 value值 (可以是未trim的字符串) </param>
        /// <returns></returns>
        public static IHTMLElement GetElementByTrimedValueName(IHTMLElement doc2, string valueName)
        {
            valueName = TrimSpaceRegex.Replace(valueName, "");
            //定义正则
            foreach (IHTMLElement ele in doc2.all)
            {
                if (ele.getAttribute("value") != null && TrimSpaceRegex.Replace(ele.getAttribute("value").ToString(), "") == valueName)
                {
                    return ele;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据InnerText 找元素 
        /// </summary>
        /// <param name="doc2"></param> 
        /// <param name="valueName"> 直接传 value值 (可以是未trim的字符串) </param>
        /// <returns></returns>
        public static IHTMLElement GetElementByTrimedInnerTextName(IHTMLElement doc2, string valueName)
        {
            valueName = TrimSpaceRegex.Replace(valueName, "");
            //定义正则
            foreach (IHTMLElement ele in doc2.all)
            {
                if (ele.getAttribute("innerText") != null && TrimSpaceRegex.Replace(ele.getAttribute("innerText").ToString(), "") == valueName)
                {
                    return ele;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据InnerText 找元素 
        /// </summary>
        /// <param name="doc2"></param> 
        /// <param name="valueName"> 直接传 value值 (可以是未trim的字符串) </param>
        /// <returns></returns>
        public static IHTMLElement GetElementByTrimedInnerTextName(IHTMLDocument2 doc2, string valueName)
        {
            valueName = TrimSpaceRegex.Replace(valueName, "");
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (ele.innerText != null && TrimSpaceRegex.Replace(ele.innerText, "") == valueName)
                {
                    return ele;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据id返回元素value
        /// </summary>
        public static string GetValueById(IHTMLElement element, string id)
        {
            foreach (IHTMLElement ele in element.all)
            {
                if (ele.id == id)
                {
                    return ele.getAttribute("value");
                }
            }
            return null;
        }
        /// <summary>
        /// 根据id返回元素value
        /// </summary>
        public static string GetValueById(IHTMLDocument2 doc2, string id)
        {
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (ele.id == id)
                {
                    return ele.getAttribute("value");
                }
            }
            return null;
        }
        /// <summary>
        /// 根据name返回元素value
        /// </summary>
        public static string GetValueByName(IHTMLElement element, string name)
        {
            foreach (IHTMLElement ele in element.all)
            {
                if (ele.getAttribute("name") != null && ele.getAttribute("name").ToString() == name)
                {
                    return ele.getAttribute("value");
                }
            }
            return null;
        }
        /// <summary>
        /// 根据name返回元素value
        /// </summary>
        public static string GetValueByName(IHTMLDocument2 doc2, string name)
        {
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (ele.getAttribute("name") != null && ele.getAttribute("name").ToString() == name)
                {
                    return ele.getAttribute("value");
                }
            }
            return null;
        }
        /// <summary>
        /// 根据className返回元素value
        /// </summary>
        public static string GetValueByClass(IHTMLElement element, string className)
        {
            foreach (IHTMLElement ele in element.all)
            {
                if (ele.className != null && ele.className == className)
                {
                    return ele.getAttribute("value");
                }
            }
            return null;
        }
        /// <summary>
        /// 根据className返回元素value
        /// </summary>
        public static string GetValueByClass(IHTMLDocument2 doc2, string className)
        {
            foreach (IHTMLElement ele in doc2.parentWindow.document.all)
            {
                if (ele.className != null && ele.className == className)
                {
                    return ele.getAttribute("value");
                }
            }
            return null;
        }
        public static Bitmap DownloadBitmap(string url, int timeout = 15, string cookie = "", bool isTest = false)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 1000 * timeout;
                request.Headers["Cookie"] = cookie;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream requestStream = response.GetResponseStream();
                if (requestStream != null)
                {
                    Bitmap sourceBm = new Bitmap(requestStream);
                    requestStream.Dispose();
                    response.Dispose();
                    if (isTest)
                    {
                        var testname = string.Format("test_{0}.png", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                        sourceBm.Save("test.png", ImageFormat.Png);
                    }
                    return sourceBm;
                }
            }
            catch (Exception e) { }
            return null;
        }
        #endregion
    }
}
