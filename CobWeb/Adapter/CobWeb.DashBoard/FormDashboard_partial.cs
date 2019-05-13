using CobWeb.Util;
using CobWeb.Util.ThredHelper;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.DashBoard
{
    public partial class FormDashboard
    {




        void Refesh_dataGridView1()
        {
            try
            {
                Process[] ps = Process.GetProcessesByName("CobWeb.exe");
                this.dataGridView1.BeginInvoke(new ThreadStart(delegate ()
                {
                    this.dataGridView1.Rows.Clear();
                }));
                DataGridViewRow row = new DataGridViewRow();
                foreach (var item in ps)
                {


                    DataGridViewTextBoxCell textboxcell = new DataGridViewTextBoxCell() { Value = item.Id };
                    DataGridViewTextBoxCell textboxcell2 = new DataGridViewTextBoxCell() { };
                    DataGridViewTextBoxCell textboxcell3 = new DataGridViewTextBoxCell() { Value = item.WorkingSet64 / (1024 * 1024) };
                    DataGridViewTextBoxCell textboxcell4 = new DataGridViewTextBoxCell();
                    DataGridViewTextBoxCell textboxcell5 = new DataGridViewTextBoxCell() { Value = item.Threads.Count };
                    //DataGridViewTextBoxCell textboxcell6 = new datagr 
                    var commands = string.Empty;
                    //using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + item.Id))
                    //using (var objects = searcher.Get())
                    //{
                    //    var @object = objects.Cast<ManagementBaseObject>().SingleOrDefault();
                    //    commands = @object?["CommandLine"]?.ToString() ?? "";
                    //}

                    textboxcell2.Value = item.ProcessName + "----" + commands;

                    //textboxcell4.Value = (item.TotalProcessorTime - TimeSpan.Zero).TotalMilliseconds / 1000 / Environment.ProcessorCount * 100;

                    row.Cells.AddRange(textboxcell, textboxcell2, textboxcell3, textboxcell4, textboxcell5);
                }
                this.dataGridView1.BeginInvoke(new ThreadStart(delegate ()
                {
                    this.dataGridView1.Rows.Add(row);
                }));
            }
            finally
            {

            }









        }



         SocketServer server;
         void Step2_InnerListen()
        {
            server = new SocketServer(6666, 4);
            server.OnRecive += Socket_OnRecive;
            server.Start();
            //server =  new SocketManager(66, 1024, this);
            //server.Start("127.0.0.1",6666);
        }


        void Step4_TimerStart()
        {
            TimerIntervial.Register(1, () =>
            {
                if (textBox2.Text.Length > 200)
                {
                    textBox2.Text = "";
                }
                textBox2.Text += DateTime.Now.ToString() + Thread.CurrentThread.ManagedThreadId + "_" + Process.GetCurrentProcess().Threads.Count + Environment.NewLine;
                Thread.Sleep(100);
                Refesh_dataGridView1();

            });

            TimerIntervial.Enabled();
        }




        //private static void httpPostRequestHandle()
        //{
        //    while (true)
        //    {
        //        HttpListenerOut.BeginGetContext(new AsyncCallback(GetContextCallBack), HttpListenerOut);
        //        HttpListenerContext context = HttpListenerOut.GetContext();
        //        if (context.Request.ContentType != "application/json")
        //        {
        //            //SetResponse(context.Response, 500, new { data = "ContentType指定 application/json" });
        //            //return;
        //        }
        //        try
        //        {
        //            var requestBody = string.Empty;
        //            using (var stream = context.Request.InputStream)
        //            using (StreamReader reader = new StreamReader(stream))
        //            {
        //                requestBody = reader.ReadToEnd();
        //            }
        //            SetResponse(context.Response, 200, new { data = "ok", msg = requestBody });
        //            HttpListenerContext_Pool.Add(new HttpListenerContextModel()
        //            {
        //                Context = context
        //            });

        //        }
        //        catch (Exception e)
        //        {
        //            SetResponse(context.Response, 500, new { data = "服务器内部错误" });
        //        }

        //    }
        //}


       

        public void Socket_OnRecive(string msg)
        {
            SetText(msg);
        }
        public void SetText(string str)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => SetText(str)));
            }
            else
            {
                textBox1.Text += "\r\n" + str;
            }
        }

    }
    public class CobWeb_ProcessList
    {
        public int Id { get; set; }
        public string StartInfo { get; set; }
        public long WorkingSet64 { get; set; }
        public long Cpu { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? LastWorkingEndTime { get; set; }
        public bool IsWorking { get; set; }
        public DateTime? CurrentWorkingStartTime { get; set; }

        public int SocketPort { get; set; }
    }




}
