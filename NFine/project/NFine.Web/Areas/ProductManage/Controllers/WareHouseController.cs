using NFine.Application.Application;
using NFine.Code;
using NFine.Data;
using NFine.Data.Extensions;
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
            if(string.IsNullOrEmpty(keyValue))
            {
                Fuctions.ChangeStep(
                    "新增" + wareHouseEntity.WareName,
                    "新增仓库");
            }

            wareHouseApp.SubmitForm(wareHouseEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            Fuctions.ChangeStep(
                "仓库" + DbHelper.ExecuteToString("select WareName from WareHouse where F_Id='" + keyValue + "'"),
                "删除仓库");

            wareHouseApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
