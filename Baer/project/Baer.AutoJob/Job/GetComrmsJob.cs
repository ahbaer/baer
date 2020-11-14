using Baer.Code;
using Baer.Data.Extensions;
using Baer.Domain.Entity.Application;
using Baer.Domain.IRepository.SystemManage;
using Baer.Repository.SystemManage;
using Quartz;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Baer.AutoJob
{
    [DisallowConcurrentExecution]
    public class GetComrmsJob : IJobTask
    {
        private IConfigRepository service = new ConfigRepository();

        public bool Start()
        {
            string host = service.GetConfigValue("ComrmsHost").ConfigValue;
            string path = "/query/comrms";
            string method = "GET";
            string appcode = service.GetConfigValue("ComrmsAppCode").ConfigValue;

            string querys = "symbols=" + service.GetConfigValue("ComrmsQuerys").ConfigValue;
            string bodys = "";
            string url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            string contract = reader.ReadToEnd();
            Comrms comrms = contract.ToObject<Comrms>();
           if (comrms.Code == 0)
            {
                foreach (Obj obj in comrms.Obj)
                {
                    ColRow cRow = new ColRow("Contract", "FS", obj.FS, false);
                    if(cRow.R_HasField)
                    {
                        cRow["Price"] = obj.P;
                        cRow["NV"] = obj.NV;
                        cRow["V"] = obj.V;
                        cRow["Time"] = new DateTime(1970, 1, 1, 0, 0, 0).AddHours(8).AddSeconds(obj.Tick);
                        cRow["ZF"] = obj.ZF;
                        cRow.Update();
                    }
                    else
                    {
                        cRow["Id"] = Guid.NewGuid().ToString();
                        cRow["ContractName"] = obj.N;
                        if (string.IsNullOrEmpty(obj.C))
                        {
                            cRow["ContractCode"] = obj.FS;
                        }
                        else
                        {
                            cRow["ContractCode"] = obj.S + obj.C;
                        }
                        cRow["Price"] = obj.P;
                        cRow["NV"] = obj.NV;
                        cRow["V"] = obj.V;
                        cRow["M"] = obj.M;
                        cRow["S"] = obj.S;
                        cRow["C"] = obj.C;
                        cRow["Time"] = new DateTime(1970, 1, 1, 0, 0, 0).AddHours(8).AddSeconds(obj.Tick);
                        cRow["ZF"] = obj.ZF;
                        cRow.Insert();
                    }
                }
            }
            return true;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
