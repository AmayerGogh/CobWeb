using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.IO;
using System.ComponentModel;

namespace NamedPipeClient
{
    public class FormClient : Form
    {
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(266, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 21);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(236, 21);
            this.textBox1.TabIndex = 1;
            // 
            // FormClient
            // 
            this.ClientSize = new System.Drawing.Size(372, 46);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "FormClient";
            this.Load += new System.EventHandler(this.FormClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public FormClient()
        {
            InitializeComponent();
        }
        private Button button1;
        private TextBox textBox1;
        NamedPipeClientStream pipeClient = new NamedPipeClientStream("localhost", "testpipe", PipeDirection.InOut, PipeOptions.Asynchronous,System.Security.Principal.TokenImpersonationLevel.None);
        StreamWriter streamWriter;
        private void FormClient_Load(object sender, EventArgs e)
        {
            InitializeComponent();
            streamWriter = new StreamWriter(pipeClient);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            pipeClient.Connect(5000);
           
            streamWriter.AutoFlush = true;
            if (streamWriter!=null)
            {
                streamWriter.WriteLine(this.textBox1.Text);
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            pipeClient.Dispose();
            streamWriter.Dispose();
            base.OnClosing(e);
        }
    }
}
