using System.Configuration;
using System.IO;
using System.Security.Permissions;
namespace CobWeb.Util.LocalHelper
{
    public class MonitorConfig
    {
        /// <summary>
        /// 监测config配置文件变化
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Monitor(string path, string[] settings)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.config";
            watcher.Changed += new FileSystemEventHandler((source, e) =>
            {
                try
                {
                    foreach (var setting in settings)
                    {
                        ConfigurationManager.RefreshSection(setting);
                    }
                }
                catch
                {
                    //
                }
            });
            watcher.EnableRaisingEvents = true;
        }
    }
}
