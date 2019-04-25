using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CobWeb.Util.NamedPipe
{
    /// <summary>
    /// pipe命名管道通讯辅助类
    /// </summary>
    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;
        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }
        public string ReadString()
        {
            try
            {
                int len;
                len = ioStream.ReadByte() * 256;
                len += ioStream.ReadByte();
                byte[] inBuffer = new byte[len];
                ioStream.Read(inBuffer, 0, len);
                return streamEncoding.GetString(inBuffer);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();
            return outBuffer.Length + 2;
        }
    }
    /// <summary>
    /// 自定义包装类
    /// </summary>
    public class StringToStream
    {
        private string Contents;
        private StreamString streamString;
        public StringToStream(StreamString ss, string contents)
        {
            Contents = contents;
            streamString = ss;
        }
        public void Start()
        {
            //string contents = File.ReadAllText(fn);
            streamString.WriteString(Contents);
        }
    }
}
