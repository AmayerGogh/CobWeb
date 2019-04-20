using CobWeb.Core;
using CobWeb.Core.Model;
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
        FormBrowser _form;
        ParamModel _request;
        public GetVersion(FormBrowser form, ParamModel paramModel)
            : base(form, paramModel, new FlashLogger("GetVersion"))
        {
            _request = paramModel;
            _form = form;
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
