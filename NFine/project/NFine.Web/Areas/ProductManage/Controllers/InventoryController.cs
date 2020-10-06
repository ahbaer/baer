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
            if(string.IsNullOrEmpty(inventoryEntity.WareName) && !string.IsNullOrEmpty(inventoryEntity.WareId))
            {
                inventoryEntity.WareName = DbHelper.ExecuteToString("select WareName from WareHouse where F_Id='" + inventoryEntity.WareId + "'");
            }

            if (string.IsNullOrEmpty(keyValue))
            {
                Fuctions.ChangeStep(
                    inventoryEntity.ProductName + "入库" + inventoryEntity.Weight + "吨",
                    "入库");

                inventoryEntity.Weight = inventoryEntity.InitWeight;
            }
            else
            {
                double lastInitWeight = Convert.ToDouble(DbHelper.ExecuteToString("select InitWeight from Inventory where F_Id='" + keyValue + "'"));
                if(lastInitWeight != inventoryEntity.InitWeight)//库存变动
                {
                    double outWeight = Convert.ToDouble(DbHelper.ExecuteToString("select isnull(sum(Weight),0) Weight from InventoryOut where InventoryId='" + keyValue + "'"));
                    inventoryEntity.Weight = inventoryEntity.InitWeight - outWeight;

                    if (outWeight > inventoryEntity.InitWeight)
                    {
                        return Error("已出库库存大于当前修改库存，请确认！");
                    }
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
