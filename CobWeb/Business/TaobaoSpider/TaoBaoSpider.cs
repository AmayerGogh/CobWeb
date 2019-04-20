using CobWeb.Browser;
using CobWeb.Core;
using CobWeb.Core.Model;
using CobWeb.Util.FlashLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoSpider
{
    public class TaoBaoSpider : ProcessBaseUseBrowser
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
        public TaoBaoSpider(FormBrowser form, ParamModel paramModel)
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
        }
    }
}
