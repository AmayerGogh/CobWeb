using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.DashBoard
{
    public partial class FormDashboard : Form
    {
        public FormDashboard()
        {
            InitializeComponent();
        }
        private void FormDashboard_Load(object sender, EventArgs e)
        {
            Refesh_dataGridView1();
        }
        private void btn_test_debug_Click(object sender, EventArgs e)
        {
            //FormAdapter form = new FormAdapter();
            //form.Show();
        }
        private void btn_bin_debug_Click(object sender, EventArgs e)
        {
            //FormAdapter form = new FormAdapter();
            //form.Show();
        }
    }
}
