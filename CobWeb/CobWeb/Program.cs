﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobWeb
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread] 
        static void Main()
        {

            AppDomain.CurrentDomain.AssemblyResolve += Init.OnResolveAssembly;
            Init.Step1_InitializeCefSetting();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormSpider());
           




            Application.Run(new FormBrowser());

        }


        private static void ExcuteRecord(string msg)
        {
            
        }



      

    }
}
