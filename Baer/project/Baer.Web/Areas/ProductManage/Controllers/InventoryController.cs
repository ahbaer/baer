using Baer.Application.Application;
using Baer.Code;
using Baer.Data.Extensions;
using Baer.Domain.Entity.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Baer.Web.Areas.ProductManage.Controllers
{
    public class InventoryController : ControllerBase
    {
        #region ==========库存==========
        private InventoryApp inventoryApp = new InventoryApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = inventoryApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = inventoryApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTotalWeight(string queryJson)
        {
            var data = inventoryApp.GetTotalWeight(queryJson);
            return Content(data);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(InventoryEntity inventoryEntity, string keyValue)
        {
            string id = inventoryApp.SubmitForm(inventoryEntity, keyValue);
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
                inventoryApp.DeleteForm(id);
            }
            return Success("删除成功。");
        }
        #endregion

        #region ==========出库==========
        private InventoryOutApp inventoryOutApp = new InventoryOutApp();

        [HttpGet]
        public ActionResult OutForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OutIndex()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOutGridJson(string inventoryId)
        {
            DataView dvOut = DbHelper.ExecuteToDataView("select b.F_RealName,a.CreatorTime,a.Weight from InventoryOut a left join Sys_User b on a.CreatorUserId=b.Id where a.InventoryId='" + inventoryId + "' order by a.CreatorTime desc");
            List<InventoryOutEntity> inventoryOutEntitys = new List<InventoryOutEntity>();
            foreach (DataRowView drv in dvOut)
            {
                InventoryOutEntity inventoryOutEntity = new InventoryOutEntity()
                {
                    CreatorUserId = Convert.ToString(drv["F_RealName"]),
                    CreatorTime = Convert.ToDateTime(drv["CreatorTime"]),
                    Weight = Convert.ToDouble(drv["Weight"])
                };

                inventoryOutEntitys.Add(inventoryOutEntity);
            }
            return Content(inventoryOutEntitys.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOutFormJson(string id)
        {
            var data = inventoryOutApp.GetForm(id);
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
        public ActionResult SubmitOutForm(InventoryOutEntity inventoryOutEntity, string id = "")
        {
            double remainder = Convert.ToDouble(DbHelper.ExecuteToString("select isnull((select isnull(cast(Weight as decimal),0) from Inventory where Id = '" + inventoryOutEntity.InventoryId + "') - (select isnull(sum(cast(Weight as decimal)),0) from InventoryOut where InventoryId = '" + inventoryOutEntity.InventoryId + "'),0)"));
            if (remainder < inventoryOutEntity.Weight)
            {
                return Error("库存不足！");
            }

            inventoryOutApp.SubmitForm(inventoryOutEntity, id);
            return Success("出库成功！");
        }
        #endregion
    }
}
