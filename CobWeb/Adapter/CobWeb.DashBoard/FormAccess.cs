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
            Excute();
            //MessageBox.Show(cc);
            //btn_Excute.Enabled = false;

        }





        private void FormAccess_Load(object sender, EventArgs e)
        {
            foreach (var item in SocketRequestHeader.Head)
            {
                cob_RequestCode.Items.Add(item);
            }
            if (cob_kernel.Items.Count > 0)
            {
                cob_kernel.SelectedIndex = 0;
            }
            if (cob_RequestCode.Items.Count > 0)
            {
                cob_RequestCode.SelectedIndex = 0;
            }
            if (cob_Type.Items.Count>0)
            {
                cob_Type.SelectedIndex = 0;
            }
            if (cob_clients.Items.Count>0)
            {
                cob_clients.SelectedIndex = 0;
            }           
        }
        protected override void OnClosed(EventArgs e)
        {
            this.Dispose();
            base.OnClosed(e);
        }
        private void Btn_rm_send_Click(object sender, EventArgs e)
        {
            rtxt_send.ResetText();
        }

        private void Btn_rm_recive_Click(object sender, EventArgs e)
        {
            rtxt_revice.ResetText();
        }

        private void btn_ref_Click(object sender, EventArgs e)
        {
            Refresh_ClientList();
        }
    }
}
