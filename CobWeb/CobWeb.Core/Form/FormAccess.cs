using CobWeb.Util;
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
            btn_Excute.Enabled = false;
            var stopwatch = Stopwatch.StartNew();
            Task.Run(() =>
            {
                Excute(stopwatch);
                btn_Excute.Enabled = true;
            });
        }
        void Excute(Stopwatch stopwatch = null)
        {
            var stopkey = Guid.NewGuid().ToString();
            txt_stopkey.Text = stopkey;
            rtxt_Result.Text = string.Empty;
            string param = rtxt_Param.Text;
            dynamic dyn = param.DeserializeObject<ExpandoObject>();
            //dyn读取 &&更改
            param = dyn.SerializeObject(dyn);
            string result = SocketAccess.Access<string, string>(
                cmb_Type.Text,
                param,
                DateTime.Now.Ticks,
                int.Parse(numeric_Timeout.Value.ToString()),
                stopkey,
                int.Parse(txt_prot.Text),
                checkBox1.Checked);
        }
    }
}
