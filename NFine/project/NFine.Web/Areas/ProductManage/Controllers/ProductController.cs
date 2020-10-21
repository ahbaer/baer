using NFine.Application.Application;
using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity.Application;
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
        public ActionResult SaveImgPath(string f_Id)
        {
            string updateSql = "update Product set ImgPath=b.FilePath from Product a inner join Frame_File b on a.F_Id=b.Related_Id where a.F_Id='" + f_Id + "'";
            DbHelper.ExecuteNonQuery(updateSql);
            return Success("上传成功。");
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
    }
}
