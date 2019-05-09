using CobWeb.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CobWeb.DashBoard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            


            
            //var path = Path.GetFullPath(cobwebPath);

            //var process = Process.Start(path + "CobWeb.exe");
        
           
            Application.Run(new FormDashboard());
        }
     
        public static string cobwebPath = @"..\..\..\..\CobWeb\bin\Debug\";

     
    }
}
