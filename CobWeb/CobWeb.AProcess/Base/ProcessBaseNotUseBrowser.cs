﻿using CobWeb.Core.Model;
using CobWeb.Core.Process;
using CobWeb.Util.FlashLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.AProcess.Base
{
    public class ProcessBaseNotUseBrowser : IProcessBase2
    {
        public ParamModel RequestData;
        protected ProcessBase processBase;

        protected FlashLogger _log;
        public ProcessBaseNotUseBrowser(ParamModel paramModel)
        {
            processBase = new ProcessBase(null, _log);
        }
        public virtual string Excute(object param)
        {
            RequestData = JsonConvert.DeserializeObject<ParamModel>(param as string);

            if (RequestData.Param is string)
            {

                //RecordLog("请求串ProcessBaseNotUseBrowser:" + RequestData.Param);
            }
            else
            {
                string temp = JsonConvert.SerializeObject(RequestData.Param);

                //RecordLog("请求串ProcessBaseNotUseBrowser:" + temp);
            }

            return string.Empty;
        }
        private bool IsQuit()
        {
            return processBase._isQuit;
        }
        public void Log(string txt)
        {
            processBase.Log(txt);
        }
    }
}
