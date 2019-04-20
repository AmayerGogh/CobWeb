using CobWeb.Browser;
using CobWeb.Core;
using CobWeb.Core.Model;
using CobWeb.Util;
using CobWeb.Util.FlashLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.AProcess
{
    public class GetVersion : ProcessBaseUseBrowser
    {
        //WebKitForm(new WebKitKernelControl());
        IEForm _form;
        ParamModel _request;
        /// <summary>
        /// 
        /// baseform.Equals(_form); 输出true
        /// baseform.KernelControl.Equals(_form.browser); 输出true
        /// </summary>
        /// <param name="form"></param>
        /// <param name="paramModel"></param>
        public GetVersion(FormBrowser form, ParamModel paramModel)
            : base(form, paramModel, new FlashLogger("GetVersion"))
        {
            _request = paramModel;
            _form = form as IEForm;
            var baseform = base.processBase._form;

        }
        public override void Login()
        {
            LoginProcessEndInvoke(new LoginResult() { IsSuccess = true });
        }

        public override void StartRequest()
        {
            _form.ExcuteRecord("start");
            _form.browser.Navigate("http://ithome.com");
            TimerHelp_Start(o1, null, 300);

        }
        void o1(TimerHelp timer, object param1, bool isCancel)
        {
            _form.ExcuteRecord("o1");
            if (_form.browser.Document.Body.InnerHtml == null)
            {
                TimerHelp_Start(o1, null, 300);
                return;
            }
            else
            {
                _form.browser.Navigate("https://my.ruanmei.com/?page=login");//
                TimerHelp_Start(o2, null, 300);
            }
        }
        void o2(TimerHelp timer, object param1, bool isCancel)
        {
            _form.ExcuteRecord("o2");
            if (_form.browser.Document.Body.InnerHtml == null)
            {
                TimerHelp_Start(o2, null, 300);
                return;
            }
            else
            {
                //Browser.Document.GetElementByAttribute
                processBase.SetResult(new ResultModel()
                {
                    IsSuccess = false,
                    Result = "{\"test\":\"登陆失败\"}"
                });
            }
        }

    }
}
