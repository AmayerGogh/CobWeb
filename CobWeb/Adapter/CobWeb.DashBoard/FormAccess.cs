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
namespace CobWeb.DashBoard
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
