using System;
using System.Configuration;
using System.Web;

namespace Baer.Code
{
    public sealed class Licence
    {
        public static bool IsLicence(string key, out string tip)
        {
            try
            {
                string licence = key;
                string machineCode = DESEncrypt.Decrypt(licence);//先解密

                long longUnixTime = new long();
                if (machineCode.Split('1').Length > 1)
                {
                    long.TryParse(machineCode.Split('|')[1], out longUnixTime);//获取授权时间戳
                }

                DateTime initTime = new DateTime(1970, 1, 1);
                TimeSpan ts = DateTime.Now - initTime;//获取当前时间戳

                if (ts.Ticks > longUnixTime)
                {
                    tip = "授权码过期";
                    return false;
                }
                else
                {
                    if (machineCode.Split('|')[0] == MachineCode.GetMachineCode(false, false))
                    {
                        tip = "授权通过";
                        return true;
                    }
                }

                tip = "授权码错误";
                return false;
            }
            catch (Exception)
            {
                tip = "授权码错误";
                return false;
            }
        }

        public static string GetLicence()
        {
            var licence = Configs.GetValue("LicenceKey");
            if (string.IsNullOrEmpty(licence))
            {
                licence = Common.GuId();
                Configs.SetValue("LicenceKey", licence);
            }
            return Md5.md5(licence, 32);
        }
    }
}
