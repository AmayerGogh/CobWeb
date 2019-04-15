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
        /// <summary>
        /// 一般都是/
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// false
        /// </summary>
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? Expires { get; set; }
        public DateTime Creation { get; set; }
        public DateTime LastAccess { get; set; }
    }
}
