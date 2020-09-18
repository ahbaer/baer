using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {
            string content = "ESSBZZCQ2020072109221111";
            content = content.Substring(content.Length - 20, 20);
        }

        public static string GetReceiveXml(string content)
        {
            int port1 = Convert.ToInt32(9090);
            IPAddress ip1 = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipe1 = new IPEndPoint(ip1, port1);//把ip和端口转化为IPEndPoint实例
            Socket c1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
            c1.Connect(ipe1);//连接到服务器

            byte[] slength;
            //修改为取银行信息配置页面的“接收报文的编码方式”BM这一字段  modity by hc 2016-8-1
            string strEncodingType = "";
            strEncodingType = Convert.ToString("GBK");
            if (!string.IsNullOrEmpty(strEncodingType))
            {
                slength = Encoding.GetEncoding(strEncodingType).GetBytes(content);
            }
            else
            {
                slength = Encoding.Default.GetBytes(content);
            }

            //添加12位数字到报文前面
            string ss = (slength.Length + 2).ToString("D10") + "00";
            byte[] bs1;

            if (!string.IsNullOrEmpty(strEncodingType))
            {
                bs1 = Encoding.GetEncoding(strEncodingType).GetBytes(ss + content);
            }
            else
            {
                bs1 = Encoding.Default.GetBytes(ss + content);
            }

            c1.Send(bs1, bs1.Length, 0);//发送信息

            string recvStr1 = null;
            byte[] recvBytes1 = new byte[1024];
            int bytes1 = 1;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            //增加Socket.ReceiveTimeout 设置了超时值，超过超时值，Receive 调用将引发 SocketException
            c1.ReceiveTimeout = 60000;

            try
            {
                while (bytes1 > 0)
                {
                    bytes1 = c1.Receive(recvBytes1, recvBytes1.Length, 0);
                    ms.Write(recvBytes1, 0, bytes1);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }

            if (string.IsNullOrEmpty(strEncodingType))
            {
                //如果没配置参数，则采用默认的编码方式，Default方式是和当前电脑设置的语言有关的
                recvStr1 = Encoding.Default.GetString(ms.ToArray(), 0, int.Parse(ms.Length.ToString()));
            }
            else
            {
                //如果配置了参数，则采用配置的编码方式
                recvStr1 = Encoding.GetEncoding(strEncodingType).GetString(ms.ToArray(), 0, int.Parse(ms.Length.ToString()));
            }
            ms.Dispose();

            c1.Close();

            return recvStr1;
        }
    }
}
