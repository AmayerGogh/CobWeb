using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util
{
    public  class CookieHelper
    {
    }
    /// <summary>
    /// cookie伪类
    /// </summary>
    public class CookiePseudo
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime Creation { get; set; }
        public DateTime LastAccess { get; set; }
    }
}
