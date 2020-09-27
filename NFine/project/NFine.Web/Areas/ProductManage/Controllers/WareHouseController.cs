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
    public class WareHouseController : ControllerBase
    {
        private WareHouseApp wareHouseApp = new WareHouseApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = wareHouseApp.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = wareHouseApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WareHouseEntity wareHouseEntity, string keyValue)
        {
            wareHouseApp.SubmitForm(wareHouseEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            wareHouseApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
