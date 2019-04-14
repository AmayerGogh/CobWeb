using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CobWeb.Util
{
    public class CommonCla
    {


        /// <summary>  
        /// 获取枚举值的描述文本  
        /// </summary>  
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
        /// <summary>
        /// 格式化JSon串
        /// </summary>
        public static string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 获取该接口的所有实现类
        /// </summary>
        public static List<Type> FindAllClassByInterface<T>()
        {
            var types = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var t in type.GetInterfaces())
                    {
                        if (t == typeof(T))
                        {
                            types.Add(type);
                            break;
                        }
                    }
                }
            }

            return types;
        }

        /// <summary>
        /// 是否已超时
        /// </summary>
        /// <param name="startTime">DateTime Ticks</param>
        /// <param name="timeout">单位秒</param>
        /// <returns></returns>
        public static bool IsTimeout(long startTime, int timeout)
        {
            var left = ((DateTime.Now.Ticks - startTime) / 10000 / 1000);
            if (left > (timeout - 1) || left < 0)
            {
                return true;
            }
            return false;
        }

       
        public static int ProcessId
        {
            get
            {
                if (_processId == 0)
                {
                    _processId = System.Diagnostics.Process.GetCurrentProcess().Id;
                }
                return _processId;
            }
        }
        static int _processId = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public static void WriteLogFile(string content, string name = "立即写的日志")
        {
            try
            {
                var dir = Path.Combine(Environment.CurrentDirectory, "log", name);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                using (TextWriter tWriter = new StreamWriter(Path.Combine(dir, DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt"), true))
                {
                    var str = string.Format("[{0}] P{1} T{2}{3}:{4}", DateTime.Now.ToString("HH:mm:ss.fff"),
                                     ProcessId.ToString().PadLeft(5, '0'),
                                     AppDomain.GetCurrentThreadId().ToString(),
                                     Thread.CurrentThread.SetThreadName(""),
                                     content);
                    tWriter.WriteLine(str);
                }
            }
            catch { }
        }
    }
}
