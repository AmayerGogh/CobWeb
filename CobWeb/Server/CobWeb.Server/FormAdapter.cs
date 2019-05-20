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
namespace CobWeb.Server
{
    public partial class FormAdapter : Form
    {
        public FormAdapter()
        {
            
            InitializeComponent();
            // this.Opacity = 0.1;
            this.BackColor = Color.White; this.TransparencyKey = Color.White;
        }
      
        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
