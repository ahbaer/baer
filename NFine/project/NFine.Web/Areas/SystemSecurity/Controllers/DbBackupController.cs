using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Domain.Entity.SystemSecurity;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemSecurity.Controllers
{
    public class DbBackupController : ControllerBase
    {
        private DbBackupApp dbBackupApp = new DbBackupApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string queryJson)
        {
            var data = dbBackupApp.GetList(queryJson);
            return Content(data.ToJson());
        }

        public ActionResult GetDbName()
        {
           var conns = System.Configuration.ConfigurationManager.ConnectionStrings;
            List<object> list = new List<object>();
            foreach (var conn in conns)
            {
                if(conn.ToString().Contains("Server") 
                    && conn.ToString().Contains("Initial Catalog") 
                    && conn.ToString().Contains("User ID")
                    && conn.ToString().Contains("Password"))
                {
                    foreach (string dbName in conn.ToString().Split(';'))
                    {
                        if(dbName.Contains("Initial Catalog"))
                        {
                            list.Add(new { id = dbName.Split('=')[1], text = dbName.Split('=')[1] });
                        }
                    }
                }
            }
            return Content(list.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(DbBackupEntity dbBackupEntity)
        {
            if (!Directory.Exists(Server.MapPath("~/Resource/DbBackup/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Resource/DbBackup/"));
            }
            dbBackupEntity.F_FilePath = Server.MapPath("~/Resource/DbBackup/" + dbBackupEntity.F_FileName + ".bak");
            dbBackupEntity.F_FileName = dbBackupEntity.F_FileName + ".bak";
            dbBackupApp.SubmitForm(dbBackupEntity);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            dbBackupApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        public void DownloadBackup(string keyValue)
        {
            var data = dbBackupApp.GetForm(keyValue);
            string filename = Server.UrlDecode(data.F_FileName);
            string filepath = Server.MapPath(data.F_FilePath);
            if (FileDownHelper.FileExists(filepath))
            {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }
    }
}
