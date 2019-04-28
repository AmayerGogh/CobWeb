using CobWeb.Util;
using CobWeb.Util.Model;
using CobWeb.Util.SocketHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.Core
{
    public partial class FormAccess : Form
    {
        public FormAccess()
        {
            InitializeComponent();
        }
        private void btn_Excute_Click(object sender, EventArgs e)
        {
        
            //MessageBox.Show(cc);
            //btn_Excute.Enabled = false;
            var stopwatch = Stopwatch.StartNew();
            Task.Run(() =>
            {
                Excute(stopwatch);
                //btn_Excute.Enabled = true;
            });
        }

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
            string result = SocketAccess.Access<string, string>(
                cmb_Type.Text,
                param,
                DateTime.Now.Ticks,
                int.Parse(numeric_Timeout.Value.ToString()),
                stopkey,
                int.Parse(txt_port.Text), false
                );
        }


        private void FormAccess_Load(object sender, EventArgs e)
        {
            var names = Enum.GetNames(typeof(SocketRequestCode));
            cob_RequestCode.Items.AddRange(names);
        }

        private void Btn_rm_send_Click(object sender, EventArgs e)
        {
            rtxt_send.ResetText();
        }

        private void Btn_rm_recive_Click(object sender, EventArgs e)
        {
            rtxt_revice.ResetText();
        }
    }
}
