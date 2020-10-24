using NFine.Application.Application;
using NFine.Application.SystemSecurity;
using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity.Application;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.ProductManage.Controllers
{
    public class ProductController : ControllerBase
    {
        private ProductApp app = new ProductApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = app.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ProductEntity productEntity, string keyValue)
        {
            string f_Id = app.SubmitForm(productEntity, keyValue);
            return Success("操作成功。", f_Id);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string f_Ids)
        {
            foreach (string f_Id in f_Ids.Split(','))
            {
                app.DeleteForm(f_Id);
            }
            
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult UploadImg(HttpPostedFileBase file, string f_Id)
        {
            Stream stream = file.InputStream;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            string base64 = Convert.ToBase64String(bytes);
            string base64Url = "data:" + file.ContentType + ";base64," + base64;

            DbHelper.ExecuteNonQuery("update Product set ImgContent='" + base64Url + "' where F_Id='" + f_Id + "'");
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult DeleteImg(string f_Id)
        {
            DbHelper.ExecuteNonQuery("update Product set ImgContent='' where F_Id='" + f_Id + "'");
            return View();
        }
    }
}
