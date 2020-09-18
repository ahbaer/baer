using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaersTool.VerifyCode
{
    public class VerifyCodeHelper
    {
        /// <summary>
        /// 生成验证码并返回
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetVerifyCode(string filePath)
        {
            int codeW = 80;
            int codeH = 30;
            int fontSize = 16;
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] font = { "Times New Roman" };
            //验证码的字符集，去掉了一些容易混淆的字符 
            char[] character = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };

            //获取时间种子
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            //生成验证码字符串 
            string chkCode = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }

            //创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线 
            for (int i = 0; i < 3; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18, (float)0);
            }

            MemoryStream ms = new MemoryStream();
            try
            {
                filePath += (@"\" + DateTime.Now.Year + @"\" + DateTime.Now.Month);
                //检验生成路径
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                //生成验证码文件名
                string fileName = DateTime.Now.ToString("ddHHmmss") + ".png";
                int fileSort = 1;
                while (File.Exists(filePath + @"\" + fileName))
                {
                    fileName = (fileName.Split('.')[0].Split('_')[0] + "_" + fileSort + ".png");
                    fileSort++;
                }

                bmp.Save(ms, ImageFormat.Png);
                File.WriteAllBytes(filePath + @"\" + fileName, ms.ToArray());
                return chkCode;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }
    }
}
