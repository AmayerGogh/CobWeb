using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util
{
   public  class MemoryMappedHelper
    {
        /// <summary>
        /// 将内容写入共享内存
        /// </summary>
        /// <param name="result"></param>
        public static void WriteIntoMMF(string key, string result)
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(key + "mmf"))
            {
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(result);
                }
            }
        }

        public static string ReadIntoMMF(string key)
        {
            string result = null;
            using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(key + "mmf"))
            {
                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                using (var reader = new BinaryReader(stream))
                {
                    result = reader.ReadString();
                }
            }
            return result;
        }
    }
}
