using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace CobWeb.Core.Control
{
    public static class IE_Setting
    {
        /// <summary>
        /// 设置当前进程用哪个IE版本打开,默认IE11
        /// </summary>
        public static void Step1_EmulationSet(int o = 0x2AF8)
        {
            //using (var localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32))
            //{
            //    var runKey = localKey.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION");
            //    runKey.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", o);
            //}

            //using (var runKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION"))
            //{
            //    runKey.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", o);
            //}
            //using (var localKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32))
            //{
            //    var runKey = localKey.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION");
            //    runKey.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", o);
            //}

            //using (var runKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION"))
            //{
            //    runKey.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", o);
            //}
        }

    }
}
