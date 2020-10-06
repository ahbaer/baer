using NFine.Application.Application;
using NFine.Code;
using NFine.Data;
using NFine.Data.Extensions;
using NFine.Domain.Entity.Application;
using System;
using System.Collections.Generic;
using System.Data;
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
        public ActionResult GetGridJson(string wareId, string keyword="")
        {
            var data = inventoryApp.GetList(wareId, keyword);
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
            if (string.IsNullOrEmpty(keyValue))
            {
                Fuctions.ChangeStep(
                    inventoryEntity.ProductName + "入库" + inventoryEntity.Weight + "吨",
                    "入库");

                inventoryEntity.InitWeight = inventoryEntity.Weight;
            }
            else
            {
                double nowWeight = Convert.ToDouble(DbHelper.ExecuteToString("select Weight from Inventory where F_Id='" + keyValue + "'"));
                if(nowWeight != inventoryEntity.Weight)
                {
                    Fuctions.ChangeStep(
                        inventoryEntity.ProductName + "出库" + (nowWeight - inventoryEntity.Weight) + "吨", 
                        "出库");

                    FRow fRow = new FRow("InventoryOut");
                    fRow["InventoryId"] = keyValue;
                    fRow["Weight"] = nowWeight - inventoryEntity.Weight;
                    fRow.Insert();
                }
            }

            string f_Id = inventoryApp.SubmitForm(inventoryEntity, keyValue);
            return Success("操作成功。", f_Id);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            Fuctions.ChangeStep(
                DbHelper.ExecuteToString("select ProductName from Inventory where F_Id='" + keyValue + "'") + "商品下架",
                "商品下架");

            inventoryApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
