using CobWeb.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobWeb.Util.FlashLog;
using CobWeb.Util.Model;

namespace TaobaoSpider
{
    public class Shopping : ProcessBaseUseBrowser
    {
        public Shopping(FormBrowser form, SocketRequestModel paramModel, FlashLogger log) : base(form, paramModel, log)
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
