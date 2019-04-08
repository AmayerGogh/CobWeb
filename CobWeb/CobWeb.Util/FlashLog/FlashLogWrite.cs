using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util.FlashLog
{
    public class FlashLogWrite
    {
        string FileIndex;
        static string logPathRoot;
        public FlashLogWrite()
        {
            logPathRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        }
        public void Info(string log,string serviceName=null,Exception ex =null)
        {
            Write(log, "Info", serviceName);
        }
        public void Debug(string log, string serviceName = null, Exception ex = null)
        {
            Write(log, "Debug", serviceName);
        }
        public void Warn(string log, string serviceName = null, Exception ex = null)
        {
            Write(log, "Warn", serviceName);
        }
        public void Error(string log, string serviceName = null, Exception ex = null)
        {
            Write(log, "Error", serviceName);
        }
        public void Fatal(string log, string serviceName = null, Exception ex = null)
        {
            Write(log, "Fatal", serviceName);
        }
        void Write(string msg,string level,string serviceName = null, Exception ex = null)
        {
            var path = logPathRoot;
            if (!string.IsNullOrWhiteSpace(serviceName))
            {
                path += $"\\{serviceName}";
            }
            path += $"\\{level}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string LogDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            string logFile = string.Format("{0}\\{1}.log", path, LogDate);
            
            if (msg.Length <= 1000)
            {
                WriteType1(logFile,msg);
            }
            else
            {
                WriteType1(logFile, msg);
            }
        }
        void WriteType1(string path,string msg)
        {
            File.AppendAllText(path, msg, Encoding.Default);
        }
        void WriteType2(string patch,string msg)
        {
            using (FileStream fs = new FileStream(patch, FileMode.Append, FileAccess.Write))
            {
                byte[] data = System.Text.Encoding.Default.GetBytes(msg);
                //开始写入
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
        }

    }
}
