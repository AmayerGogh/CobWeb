using CobWeb.AProcess.Base;

using CobWeb.Core.Model;
using CobWeb.Core.Process;
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
        IBrowserBase _form;
        ParamModel _request;
        public GetVersion(IBrowserBase form, ParamModel paramModel)
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
            
            _form.ExcuteRecord("test");
            dynamic c = JsonConvert.DeserializeObject<object>(_request.Param.ToString());
           
            if (c.Type == 1)
            {
                try
                {
                    var cc = (string)c.Js;
                    _form.ExecuteScript(cc);
                    
                }
                catch (Exception e)
                {
                    processBase.SetResult(new ResultModel()
                    {
                        IsSuccess = false,
                        Result = e.ToString()
                    });
                    return;
                }
            }
            else if (c.Type == 2)
            {
                try
                {
                    var cc = _form.EvaluateScriptAsync((string)c.Js);
                   
                }
                catch (Exception e)
                {
                    processBase.SetResult(new ResultModel()
                    {
                        IsSuccess = false,
                        Result = e.ToString()
                    });
                    return;
                }
            }
            else if (c.Type == 3)
            {
                try
                {
                    var cc = _form.GetCurrentCookie((string)c.Url);
                    processBase.SetResult(new ResultModel()
                    {
                        IsSuccess = false,
                        Result = cc
                    });
                    return;
                }
                catch (Exception e)
                {
                    processBase.SetResult(new ResultModel()
                    {
                        IsSuccess = false,
                        Result = e.ToString()
                    });
                    return;
                }
            }
            else if (c.Type == 4)
            {
                try
                {
                    var url = (string)c.Url;
                    var cookie = (string)c.Cookie;
                    _form.SetCookie(url,cookie);
                    processBase.SetResult(new ResultModel()
                    {
                        IsSuccess = false,
                        Result = "成功"
                    });
                    return;
                }
                catch (Exception e)
                {
                    processBase.SetResult(new ResultModel()
                    {
                        IsSuccess = false,
                        Result = e.ToString()
                    });
                    return;
                }
            }

            processBase.SetResult(new ResultModel()
            {
                IsSuccess = false,
                Result = "{\"test\":\"登陆失败\"}"
            });
        }
    }
}
