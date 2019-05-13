using CobWeb.Core;
using CobWeb.Util;
using CobWeb.Util.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.DashBoard
{
    public partial class  FormAccess
    {
       
        Socket socket;


        //private void btn_Excute_Click(object sender, EventArgs e)
        //{ 
        //    var request = BuildRequest();
        //    if (request==null)
        //    {
        //        return;
        //    }
        //    var req_str =request.SerializeObject();
        //    SocketBasic.Send(socket, req_str, request.Timeout);

        //}

        public SocketRequestModel BuildRequest()
        {
            if (!ParamValid())
            {
                return null;
            }

            var stopkey = Guid.NewGuid().ToString();
            txt_stopkey.Text = stopkey;

            string param = rtxt_send.Text;
            if (!rtxt_send.Text.IsJson())
            {
                lbl_Msg.Text = "请求体必须为 json";
                return null;
            }

            int timeout = 0;
            if (!int.TryParse(numeric_Timeout.Text, out timeout))
            {
                timeout = 60;
            }
            SocketRequestCode head;
            if (!Enum.TryParse<SocketRequestCode>(cob_RequestCode.Text, out head))
            {
                lbl_Msg.Text = "请求head无效";
                return null;
            }
            SocketRequestModel model = new SocketRequestModel();
            model.FileName = txt_fileName.Text;
            model.Port = txt_port.Text;
            model.Key = txt_stopkey.Text;
            model.KernelType = cob_kernel.Text;
            model.Method = cmb_Type.Text;
            model.Timeout = timeout;
            model.Header = head;
            return model;
        }
        void Excute(Stopwatch stopwatch = null)
        {
            var stopkey = Guid.NewGuid().ToString();
            txt_stopkey.Text = stopkey;
            rtxt_revice.Text = string.Empty;
            string param = rtxt_send.Text;
            dynamic dyn = param.DeserializeObject<ExpandoObject>();
            //dyn读取 &&更改
            param = dyn.SerializeObject(dyn);
         
        }

    }
}
