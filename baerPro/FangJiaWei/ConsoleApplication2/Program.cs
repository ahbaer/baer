using DianZiFaPiaoApi.Model_MS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            string aa = "1FBEB3B1E044E892653B37B0575D23093A830B9D3E96FD5D182E9146124DAF9694065F771D531522535FEF88735CE6A6055F5A92AFEC812146FF1328A9EA5EBC35CFA0F1FBDC588D92DFDC979D565FE7A3554775AEAC162EC71CFE69DD02CDB56A465ADD2B55A1B5C1B05BAFB7100108931F34D058013F579AAAA8E115632833F6F687F0B80A7CFF389EAA0A3CA3B5356E7EA4E1D896F2E82376F578EBF916BB203A6F89F1EEB0A685EE28344F1BD9B5DEE0F9EAADAA3AE6389B19F10057B1E792C9C3476233405100F5C349C8E30DB12CC5C1A414BF9F3E3C141862F2A27CC11E2AA84499AA3E6D844A68BEFB4D900370D184910B2D2DCD74523DED0434A6F5E4EBA1A47A093498A87186AF1AB697644DB704AA9B2C5E68BAE6CC45A49C583AFE0701D461B588748E09F9BBCFAAE6206120F74E0BB77C6A022A658C3A84EE174F62ACE4E175554AE96E0056FA07A3D94699F0F06A65A9295E4234A79779DE42";
            string bb = DB_Security.DESDeCode(aa, "qwejyewq", "GBK");
        }

        public static bool HttpDownload(string url, string path, string fileGuid, out string filePath, out string msg)
        {
            msg = "";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);//创建临时文件目录
            }
            string tempFile = path + @"\" + fileGuid + ".pdf";//临时文件
            filePath = tempFile;
            if (System.IO.File.Exists(tempFile))
            {
                System.IO.File.Delete(tempFile);//存在则删除
            }
            try
            {
                FileStream fs = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();

                //int len = (int)responseStream.Length;

                //创建本地文件写入流
                //Stream stream = new FileStream(tempFile, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                if(size == 0)
                {
                    fs.Close();
                    responseStream.Close();
                    msg = "获取文件为空";
                    return false;
                }

                while (size > 0)
                {
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                fs.Close();
                responseStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return false;
            }
        }
    }
}
