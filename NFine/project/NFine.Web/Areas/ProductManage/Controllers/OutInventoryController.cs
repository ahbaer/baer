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
    public class OutInventoryController : ControllerBase
    {
        private InventoryOutApp inventoryOutApp = new InventoryOutApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string inventoryId)
        {
            DataView dvOut = DbHelper.ExecuteToDataView("select b.F_RealName,a.F_CreatorTime,a.Weight from InventoryOut a left join Sys_User b on a.F_CreatorUserId=b.F_Id where a.InventoryId='" + inventoryId + "' order by a.F_CreatorTime desc");
            List<InventoryOutEntity> inventoryOutEntitys = new List<InventoryOutEntity>();
            foreach (DataRowView drv in dvOut)
            {
                InventoryOutEntity inventoryOutEntity = new InventoryOutEntity()
                {
                    F_CreatorUserId = Convert.ToString(drv["F_RealName"]),
                    F_CreatorTime = Convert.ToDateTime(drv["F_CreatorTime"]),
                    Weight = Convert.ToDouble(drv["Weight"])
                };

                inventoryOutEntitys.Add(inventoryOutEntity);
            }
            return Content(inventoryOutEntitys.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string f_Id)
        {
            var data = inventoryOutApp.GetForm(f_Id);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 暂时按照出库不允许修改来操作
        /// </summary>
        /// <param name="inventoryOutEntity"></param>
        /// <param name="f_Id"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(InventoryOutEntity inventoryOutEntity, string f_Id = "")
        {
            DataView dv = DbHelper.ExecuteToDataView("select ProductName,Weight from Inventory where F_Id='" + inventoryOutEntity.InventoryId + "'");
            if(dv.Count == 0)
            {
                return Error("参数错误！");
            }
            if(Convert.ToDouble(dv[0]["Weight"]) < inventoryOutEntity.Weight)
            {
                return Error("库存不足！");
            }
            Fuctions.ChangeStep(
                DbHelper.ExecuteToString("select ProductName from Inventory where F_Id='" + inventoryOutEntity.InventoryId + "'") + "出库" + inventoryOutEntity.Weight + "吨",
                "出库");

            inventoryOutApp.SubmitForm(inventoryOutEntity, f_Id);
            DbHelper.ExecuteNonQuery("update Inventory set Weight='" + (Convert.ToDouble(dv[0]["Weight"]) - inventoryOutEntity.Weight) + "' where F_Id='" + inventoryOutEntity.InventoryId + "'");
            return Success("出库成功！");
        }
    }
}
