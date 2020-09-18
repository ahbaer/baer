using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaersTool.Mail;
using BaersTool.VerifyCode;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 测试生成验证码

            for(int i = 0; i < 10; i++)
            {
                VerifyCodeHelper.GetVerifyCode(@"D:\User\Desktop\images");
            }
            
            #endregion

            #region 压缩/解压缩测试
            System.IO.Compression.ZipFile.CreateFromDirectory(@"D:\User\Desktop\test", @"D:\User\Desktop\test.zip"); //压缩
            #endregion

            #region 邮寄发送测试
            MailHelper mail = new MailHelper();
            mail.FromHost = "smtp.qq.com";
            mail.FromAddress = "ahbaer@foxmail.com";
            mail.FromDisplayName = "巴二";
            mail.FromPassword = "jxtwchrvwtrfbhjf";

            mail.Send("1006081825@qq.com", "测试邮件发送", "测试foxmail");
            #endregion
        }
    }
}
