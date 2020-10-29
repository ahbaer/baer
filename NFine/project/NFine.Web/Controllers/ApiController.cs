using NFine.Application.Application;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity.Application;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    [HandlerApiAuth]
    public class ApiController : Controller
    {
        #region ==========图片==========
        [HttpGet]
        public ActionResult GetBanerListJson()
        {
            List<object> baners = new List<object>();

            FileApp fileApp = new FileApp();
            var data = fileApp.GetList("baner");
            foreach (var item in data)
            {
                baners.Add(new { name = item.FileName, base64 = item.FileContent });
            }
            return Content(baners.ToJson());
        }
        #endregion

        #region ==========产品==========
        [HttpGet]
        public ActionResult GetProListJson()
        {
            List<object> products = new List<object>();

            ProductApp proApp = new ProductApp();
            var data = proApp.GetList("");
            foreach (var item in data)
            {
                products.Add(new { productName = item.ProductName, productCode = item.ProductCode, baer64 = item.ImgContent });
            }
            return Content(products.ToJson());
        }
        #endregion

        #region ==========分类==========
        [HttpGet]
        public ActionResult GetItemTypeAndDetails()
        {
            List<object> itemAndDetails = new List<object>();

            ItemsApp itemsApp = new ItemsApp();
            List<ItemsEntity> itemTypes = itemsApp.GetApiList("Product_Cotton");
            foreach (var itemType in itemTypes)
            {
                List<object> retDetails = new List<object>();

                ItemsDetailApp itemsDetailApp = new ItemsDetailApp();
                List<ItemsDetailEntity> itemDetails = itemsDetailApp.GetList(itemType.F_Id, "");
                foreach (var itemDetail in itemDetails)
                {
                    retDetails.Add(new { itemCode = itemDetail.F_ItemCode, itemName = itemDetail.F_ItemName });
                }
                itemAndDetails.Add(new { typeName = itemType.F_FullName, typeEntitys = retDetails });
            }
            return Content(itemAndDetails.ToJson());
        }
        #endregion

        #region ==========商品==========
        [HttpPost]
        public ActionResult GetInventoryListJson(InventoryReq inventoryReq)
        {
            Pagination pagination = inventoryReq.pagination;
            InventoryQry[] inventoryQrys = inventoryReq.inventoryQry;

            //sql
            string strSql = @"
select 
	WareName,
	OrderNo,
	(select ProductName from Product where ProductCode=a.ProductType) as ProductType,
	(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0202' and F_ItemCode=a.Grade) as Grade,
	Strength,
	(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0208' and F_ItemCode=a.Grade) as Length,
	(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0203' and F_ItemCode=a.HorseValue) as HorseValue,
	(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0207' and F_ItemCode=a.Status) as Status,
	(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0206' and F_ItemCode=a.QuoteType) as QuoteType,
	Price,
	(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0206' and F_ItemCode=a.Contract) as Contract,
	Basis,
	Year,
	SailingSchedule,
	Weight,
	IsRecommend
from Inventory a
where 1=1 ";

            //where
            string strQry = "";
            foreach (InventoryQry inventoryQry in inventoryQrys)
            {
                if (inventoryQry.code.ToLower() == "strength")//强力是范围，其他都是选项
                {
                    foreach (SelectedList selected in inventoryQry.selectedList)
                    {
                        if (selected.name.ToLower() == "min")
                        {
                            strQry += " and cast(Strength as float)>=" + selected.code + " ";
                        }
                        else if (selected.name.ToLower() == "max")
                        {
                            strQry += " and cast(Strength as float)<=" + selected.code + " ";
                        }
                    }
                }
                else
                {
                    string condition = "";
                    foreach (SelectedList selected in inventoryQry.selectedList)
                    {
                        if (selected.selected)
                        {
                            condition += "'" + selected.code + "',";
                        }
                    }
                    if (!string.IsNullOrEmpty(condition))
                    {
                        strQry += " and " + inventoryQry.code + " in (" + condition.Trim(',') + ") ";
                    }
                }
            }

            //order
            string strOrder = " order by IsRecommend desc,F_CreatorTime desc ";
            //paging
            string strPaging = " offset " + (pagination.page - 1) * pagination.rows + " rows fetch next " + pagination.rows + " rows only";

            DataView dvInventory = DbHelper.ExecuteToDataView(strSql + strQry + strOrder + strPaging);
            List<object> inventorys = new List<object>();
            foreach (DataRowView drv in dvInventory)
            {
                inventorys.Add(new
                {
                    wareName = Convert.ToString(drv["WareName"]),
                    orderNo = Convert.ToString(drv["OrderNo"]),
                    productType = Convert.ToString(drv["ProductType"]),
                    grade = Convert.ToString(drv["Grade"]),
                    strength = Convert.ToString(drv["Strength"]),
                    length = Convert.ToString(drv["Length"]),
                    horseValue = Convert.ToString(drv["HorseValue"]),
                    status = Convert.ToString(drv["Status"]),
                    quoteType = Convert.ToString(drv["QuoteType"]),
                    price = Convert.ToString(drv["Price"]),
                    contract = Convert.ToString(drv["Contract"]),
                    basis = Convert.ToString(drv["Basis"]),
                    year = Convert.ToString(drv["Year"]),
                    sailingSchedule = Convert.ToString(drv["SailingSchedule"]),
                    weight = Convert.ToString(drv["Weight"]),
                    isRecommend = Convert.ToString(drv["IsRecommend"])
                });
            }

            return Content(inventorys.ToJson());
        }
        #endregion

        #region ==========仓库==========

        #endregion
    }
}
