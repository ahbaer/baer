using NFine.Application.Application;
using NFine.Code;
using NFine.Domain.Entity.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.ProductManage.Controllers
{
    public class InventoryController : ControllerBase
    {
        private InventoryApp inventoryApp = new InventoryApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string ware_Id, string keyword)
        {
            var data = inventoryApp.GetList(ware_Id, keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = inventoryApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(InventoryEntity inventoryEntity, string keyValue)
        {
            inventoryApp.SubmitForm(inventoryEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            inventoryApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
