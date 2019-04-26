using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb.DashBoard
{
    public partial class FormDashboard
    {
        /// <summary>
        /// 当前的 进程对应类
        /// </summary>
        static List<CobWeb_ProcessList> CobWeb_ProcessList = new List<CobWeb_ProcessList>();

        static List<Socket> sockets = new List<Socket>();
        void Refesh_dataGridView1()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Process[] ps = Process.GetProcesses();
                        this.dataGridView1.BeginInvoke(new ThreadStart(delegate ()
                        {
                            this.dataGridView1.Rows.Clear();
                        }));


                        foreach (var item in ps)
                        {                          

                            DataGridViewRow row = new DataGridViewRow();
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


                            this.dataGridView1.BeginInvoke(new ThreadStart(delegate ()
                            {
                                this.dataGridView1.Rows.Add(row);
                            }));

                        }
                    }
                    finally
                    {

                    }

                    Thread.Sleep(1000);
                }

            });




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
