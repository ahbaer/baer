using Baer.Application.Application;
using Baer.Code;
using Baer.Data.Extensions;
using Baer.Domain.Entity.Application;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Baer.Web.Areas.ProductManage.Controllers
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
            string id = app.SubmitForm(productEntity, keyValue);
            return Success("操作成功。", id);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string ids)
        {
            foreach (string id in ids.Split(','))
            {
                app.DeleteForm(id);
            }
            
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult UploadImg(HttpPostedFileBase file, string id)
        {
            Stream stream = file.InputStream;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            string base64 = Convert.ToBase64String(bytes);
            string base64Url = "data:" + file.ContentType + ";base64," + base64;

            DbHelper.ExecuteNonQuery("update Product set ImgContent='" + base64Url + "' where Id='" + id + "'");
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult DeleteImg(string id)
        {
            DbHelper.ExecuteNonQuery("update Product set ImgContent='' where Id='" + id + "'");
            return View();
        }
    }
}
