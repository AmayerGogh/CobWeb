using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util
{
    public  class MemoryCacheHelper
    {
        /// <summary>
        /// 缓存中是否有
        /// </summary>
        public static bool CacheIsHave(string key)
        {
            return MemoryCache.Default.Contains(key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        public static object RemoveCache(string key)
        {
            return MemoryCache.Default.Remove(key);
        }
    }
}
