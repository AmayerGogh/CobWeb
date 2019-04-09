using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CobWeb.Util
{
    public static class ThreadExtension
    {
        public static string SetThreadName(this Thread t, string name)
        {
            if (string.IsNullOrEmpty(t.Name))
            {
                t.Name = string.Format("[{0}-{1}]",
                    t.ManagedThreadId.ToString().PadLeft(3, '0'),
                    name);
                //t.IsBackground,
                //DateTime.Now.ToString("mm:ss.fff"));
            }
            return t.Name;
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
    }
}
