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
        int  FileIndex;
        static string logPathRoot;
        public FlashLogWrite()
        {
            logPathRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        }
       public void Write(FlashLogMessage msg)
        {
            var level = msg.Level.ToString();
            //switch (msg.Level)
            //{
            //    case FlashLogLevel.Debug:
            //        level = "Debug";
            //        break;
            //    case FlashLogLevel.Info:
            //        _log.Info(msg.Message, msg.Service, msg.Exception);
            //        break;
            //    case FlashLogLevel.Error:
            //        _log.Error(msg.Message, msg.Service, msg.Exception);
            //        break;
            //    case FlashLogLevel.Warn:
            //        _log.Warn(msg.Message, msg.Service, msg.Exception);
            //        break;
            //    case FlashLogLevel.Fatal:
            //        _log.Fatal(msg.Message, msg.Service, msg.Exception);
            //        break;
            //}
            var path = logPathRoot;
            if (!string.IsNullOrWhiteSpace(msg.Service))
            {
                path += $"\\{msg.Service}";
            }
            path += $"\\{level}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string LogDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            string logFilePath = GetPath(path, LogDate, 0);
            if (msg.Message.Length <= 1024 * 100) //100k
            {
                WriteType1(logFilePath, msg.Message + Environment.NewLine);
            }
            else
            {
                WriteType1(logFilePath, msg.Message);
            }
        }
        void WriteType1(string path,string msg)
        {
            File.AppendAllText(path, msg, Encoding.Default);
        }
        void WriteType2(string patch,string msg)
        {
            using (TextWriter tWriter = new StreamWriter(patch, true))
            {                
                tWriter.WriteLine(msg);
            }           
        }
        string GetPath(string path , string LogDate,int index)
        {           
            string logFilePath = string.Format("{0}\\{1}.{2}.log", path, LogDate, index);
            FileInfo file = new FileInfo(logFilePath);
            if (file.Exists)
            {
                if (file.Length > FlashLogSetting.Max_LogSize)
                {
                    index++;
                    return GetPath(path, LogDate, index);
                }
                else
                {
                    return logFilePath;
                }
            }
            else
            {
                return logFilePath;
            }
        }
    }
}
