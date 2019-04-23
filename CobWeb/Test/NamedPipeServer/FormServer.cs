using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NamedPipeServer
{
    public  class FormServer:Form
    {
        private RichTextBox richTextBox1;
        public FormServer()
        {
            InitializeComponent();
        }
        NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(274, 223);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // FormServer
            // 
            this.ClientSize = new System.Drawing.Size(298, 247);
            this.Controls.Add(this.richTextBox1);
            this.Name = "FormServer";
            this.Load += new System.EventHandler(this.FormServer_Load);
            this.ResumeLayout(false);

        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                pipeServer.BeginWaitForConnection((o) =>
                {
                    NamedPipeServerStream pServer = (NamedPipeServerStream)o.AsyncState;
                    pServer.EndWaitForConnection(o);
                    StreamReader sr = new StreamReader(pServer);
                    while (true)
                    {
                        this.Invoke((MethodInvoker)delegate { richTextBox1.Text += (Environment.NewLine + sr.ReadLine()); });
                    }

                },pipeServer);
            });
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
