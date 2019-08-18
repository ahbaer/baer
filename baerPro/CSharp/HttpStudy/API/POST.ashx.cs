using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpStudy.API
{
    /// <summary>
    /// POST 的摘要说明
    /// </summary>
    public class POST : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string recive = HttpContext.Current.Request["json"];
            string filePath = HttpRuntime.AppDomainAppPath.ToString();
            BaersTool.Log.LogOperate.WriteLog(recive, "PostText", filePath + "\\logfiles");
            context.Response.ContentType = "text/plain";
            context.Response.Write(recive);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}