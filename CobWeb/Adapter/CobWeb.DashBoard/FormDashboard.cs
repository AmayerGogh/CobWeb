
using CobWeb.Util.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.DashBoard
{
    public partial class FormDashboard : Form
    {
        public delegate void SetListBoxCallBack(string str);
        public SetListBoxCallBack setlistboxcallback;
       
        public FormDashboard()
        {           
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;           
                                              
        }
        private void FormDashboard_Load(object sender, EventArgs e)
        {

            //iocp.Init();
            //iocp.Start("127.0.0.1", 6666);
            Step4_TimerStart();
        }
        private void btn_test_debug_Click(object sender, EventArgs e)
        {
            if (Program.FormAccess != null && !Program.FormAccess.IsDisposed)
            {
                if (Program.FormAccess.Visible == true)
                {
                    Program.FormAccess.Activate();
                    return;
                }
            }
            Thread newThread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    Program.FormAccess.ShowDialog();
                }
                finally
                {
                    Program.FormAccess.Close();
                }
            }));
            newThread.Name = "FormAccess";
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.IsBackground = true;
            newThread.Start();
        }
        private void btn_bin_debug_Click(object sender, EventArgs e)
        {
            StartBrowserProcess(SocketKernelType.IE);
        }

        private void btn_socketStart_Click(object sender, EventArgs e)
        {
            FormVirtualWeb formVritualWeb = new FormVirtualWeb();
            formVritualWeb.Show();
        }
  
     
       
        private void btn_bin_ref_Click(object sender, EventArgs e)
        {
            StartBrowserProcess(SocketKernelType.CefSharp);
        }
    }

}
