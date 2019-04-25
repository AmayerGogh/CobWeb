using CobWeb.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobWeb.Core.Model;
using CobWeb.Util.FlashLog;
namespace TaobaoSpider
{
    public class Shopping : ProcessBaseUseBrowser
    {
        public Shopping(FormBrowser form, ParamModel paramModel, FlashLogger log) : base(form, paramModel, log)
        {
        }
        public override void Login()
        {
            Console.WriteLine("login");
        }
        public override void StartRequest()
        {
        }
    }
}
