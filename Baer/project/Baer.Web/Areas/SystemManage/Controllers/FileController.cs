using Baer.Application.SystemManage;
using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Baer.Web.Areas.SystemManage.Controllers
{
    public class FileController : ControllerBase
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
            var data = fileApp.GetList(related_Id);
            if(data.Count > 3)
            {
                return Error("最多上传四张图片！");
            }

            //文件也存一份
            var filePath = Server.MapPath(string.Format("~/{0}{1}", "UploadFile\\", related_Id));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);//创建该文件夹
            }
            file.SaveAs(Path.Combine(filePath, file.FileName));

            Stream stream = file.InputStream;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            string base64 = Convert.ToBase64String(bytes);
            string base64Url = "data:" + file.ContentType + ";base64," + base64;

            FileEntity fileEntity = new FileEntity()
            {
                Related_Id = related_Id,
                FileName = file.FileName,
                FileSize = file.ContentLength,
                FileType = file.ContentType,
                FilePath = "UploadFile\\" + related_Id + "\\" + file.FileName,
                FileContent = base64Url
            };
            string f_Id = fileApp.SaveFile(fileEntity);

            return Success("操作成功。", f_Id);
        }

        [HttpPost]
        public ActionResult DeleteFile(string f_Id)
        {
            fileApp.DeleteFile(f_Id);
            return Success("删除成功。");
        }
    }
}
