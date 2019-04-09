using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util
{
    public static class StringHelper
    {
        /// <summary>
        /// 有 否 运算的的地方，不能直接使用
        /// </summary>
        public static bool NullContains(this string s, string value)
        {
            if (s == null) return false;

            return s.Contains(value);
        }
        public static bool IsNotNullOrEmpty(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }
    }
}
