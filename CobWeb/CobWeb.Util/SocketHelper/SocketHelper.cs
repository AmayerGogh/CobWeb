﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CobWeb.Util.SocketHelper
{
   public  class SocketHelper
    {
        public static byte[] BuildRequest(string request)
        {
            var b_msg = Encoding.UTF8.GetBytes(request);
            var b_t = BitConverter.GetBytes(b_msg.Length + 4);
            return b_t.Concat(b_msg).ToArray();
        }

        public static string GetRequest(ref List<byte> recive)
        {
            var length = BitConverter.ToInt32(new byte[] { recive[0], recive[1], recive[2], recive[3] }, 0);
            //调试粘包
            //var s = $"(recive.Count {recive.Count} length {length}" +Environment.NewLine;
            var res = string.Empty;
            try
            {
                if (recive.Count == length)
                {
                    res = Encoding.UTF8.GetString(recive.Skip(4).Take(length - 4).ToArray());
                    recive = new List<byte>();
                }
                else if (recive.Count > length)
                {
                    res = Encoding.UTF8.GetString(recive.Skip(4).Take(length - 4).ToArray());
                    recive.RemoveRange(0, length);
                }
                else if (recive.Count < length)//应该不会出现了
                {
                    recive = new List<byte>();
                }
            }
            catch (Exception e)
            {


            }

            return res;
        }
    }
}