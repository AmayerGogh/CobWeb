using CobWeb.AProcess.Base;
using CobWeb.Browser;
using CobWeb.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.AProcess
{
    public class GetVersion : ProcessBaseUseBrowser
    {
        FormBrowser _form;
        public GetVersion(FormBrowser form, ParamModel paramModel) : base(form, paramModel)
        {
            this._form = form;
        }
        public override void Login()
        {
            LoginProcessEndInvoke(new LoginResult() { IsSuccess = true });
        }

        public override void StartRequest()
        {
            processBase.SetResult(new ResultModel()
            {
                IsSuccess = false,
                Result = "{\"test\":\"登陆失败\"}"
            });
        }
    }
}
