using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class FileController : Controller
    {
        private FileApp fileApp = new FileApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetListJson(string related_Id)
        {
            var data = fileApp.GetList(related_Id);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult UploadFile(HttpPostedFileBase file, string related_Id)
        {
            var filePath = Server.MapPath(string.Format("~/{0}{1}", "UploadFile\\", related_Id));
            if(!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);//创建该文件夹
            }
            file.SaveAs(Path.Combine(filePath, file.FileName));

            FileEntity fileEntity = new FileEntity()
            {
                Related_Id = related_Id,
                FilePath = "UploadFile/" + related_Id + "/" + file.FileName,
                FileName = file.FileName,
                FileSize = file.ContentLength,
                FileType = file.ContentType
            };

            fileApp.SaveFile(fileEntity);
            return View();
        }

        [HttpPost]
        public ActionResult UpdateRelatedId(string guid, string related_Id)
        {
            string strSql = "update Frame_File set Related_Id='" + guid + "' where Related_Id='" + related_Id + "'";
            DbHelper.ExecuteSqlCommand(strSql);
            return View();
        }
    }
}
