using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobWeb.Core.Model;
using CobWeb.Core.Process;
using CobWeb.Util.FlashLog;
using CobWeb.Core;

namespace CobWeb.AProcess
{
    public class JsTest : ProcessBaseNotUseBrowser
    {
        public JsTest(ParamModel paramModel) : base(paramModel)
        {
            _log = new FlashLogger("JsTest");
        }
        public override string Excute(object param)
        {
            base.Excute(param);
            var Result = "{\"test\":\"登陆失败\"}";
            return Result;
        }

    }
}
