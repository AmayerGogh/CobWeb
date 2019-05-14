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
        bool ParamValid()
        {
            if (string.IsNullOrWhiteSpace(txt_port.Text))
            {
                lbl_Msg.Text = "端口号为空";
                return false;
            }

            if (string.IsNullOrWhiteSpace(cob_kernel.Text))
            {
                lbl_Msg.Text = "请指定内核";
                return false;
            }
            if (string.IsNullOrWhiteSpace(cob_RequestCode.Text))
            {
                lbl_Msg.Text = "请指定操作类型";
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmb_Type.Text))
            {
                lbl_Msg.Text = "请指定方法名";
                return false;
            }
            if (string.IsNullOrWhiteSpace(numeric_Timeout.Text))
            {
                lbl_Msg.Text = "请指定超时时间";
                return false;
            }
            if (string.IsNullOrWhiteSpace(rtxt_send.Text))
            {
                lbl_Msg.Text = "请指定请求体";
                return false;
            }
            if (!rtxt_send.Text.IsJson())
            {
                lbl_Msg.Text = "请求体必须为json";
                return false;
            }
            return true;
        }
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
            //SocketRequestHeader head;
            //if (!Enum.TryParse<SocketRequestHeader>(cob_RequestCode.Text, out head))
            //{
            //    lbl_Msg.Text = "请求head无效";
            //    return null;
            //}
            SocketRequestModel model = new SocketRequestModel();
            model.FileName = txt_fileName.Text;
            model.Port = txt_port.Text;
            model.Key = txt_stopkey.Text;
            model.KernelType = cob_kernel.Text;
            model.Method = cmb_Type.Text;
            model.Timeout = timeout;
            model.Header = SocketRequestHeader.UserFormSpider;
            return model;
        }
        void Excute(Stopwatch stopwatch = null)
        {
            var model = BuildRequest();
            if (model==null)
            {
                return;
            }
            lbl_Msg.Text = string.Empty;
            var stopkey = Guid.NewGuid().ToString();
            model.Key = stopkey;
            txt_stopkey.Text = stopkey;
            //dyn读取 &&更改
            string param = rtxt_send.Text;
            dynamic dyn = param.DeserializeObject<ExpandoObject>();            
            param = JsonHelper.Serialize(dyn);

            model.Context = param;
            var req= model.Header + model.SerializeObject();
            var selectedSocket = comboBox1.SelectedItem.ToString();
            if (comboBox1.SelectedIndex != -1 && Program.SocketClient.ContainsKey(selectedSocket))
            {
                var sock = Program.SocketClient[selectedSocket];
                Program.server.Send(req, sock.Socket);
            }
            else
            {
                lbl_Msg.Text = "调用者未找到";
            }

            MessageBox.Show("可以调用了");
        }
        public void SetText(string str)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => SetText(str)));
            }
            else
            {
                if (rtxt_revice.Text.Length>10000)
                {
                    rtxt_revice.Text = string.Empty;
                }
                rtxt_revice.Text += "\r\n" + str;
            }
        }
        public void Refesh_ClientList()
        {
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add("由系统决定");
            var list = new List<string>();
            foreach (var item in Program.SocketClient)
            {
                this.comboBox1.Items.Add(item.Key);
            }            
        }

    }
}
