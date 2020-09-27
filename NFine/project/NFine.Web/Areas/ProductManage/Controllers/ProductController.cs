using NFine.Application.Application;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.Application;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.ProductManage.Controllers
{
    public class ProductController : ControllerBase
    {
        private ProductApp productApp = new ProductApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string pro_Id)
        {
            var data = productApp.GetList(pro_Id);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string pro_Id)
        {
            var data = productApp.GetForm(pro_Id);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ProductEntity productEntity, string pro_Id)
        {
            pro_Id = productApp.SubmitForm(productEntity, pro_Id);
            return Success("操作成功。", pro_Id);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string pro_Id)
        {
            productApp.DeleteForm(pro_Id);
            return Success("删除成功。");
        }
    }
}
