using CobWeb.AProcess.Base;

using CobWeb.Core.Model;
using CobWeb.Core.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.AProcess
{
    public class GetVersion : ProcessBaseUseBrowser
    {
        IBrowserBase _form;
        public GetVersion(IBrowserBase form, ParamModel paramModel) : base(form, paramModel)
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
