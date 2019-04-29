﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            FormAccessTest form = new FormAccessTest();
            form.Show();
        }
        private void btn_bin_debug_Click(object sender, EventArgs e)
        {
            
            var path = Path.GetFullPath(Program.cobwebPath);
            var process = Process.Start(path + "CobWeb.exe");

            //FormAdapter form = new FormAdapter();
            //form.Show();
        }
    }
}
