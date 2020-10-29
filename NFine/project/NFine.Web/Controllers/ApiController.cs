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
                    retDetails.Add(new { itemCode = itemDetail.F_ItemCode, itemName = itemDetail.F_ItemName});
                }
                itemAndDetails.Add(new { typeName = itemType.F_FullName, typeEntitys = retDetails });
            }
            return Content(itemAndDetails.ToJson());
        }
        #endregion

        #region ==========商品==========
        [HttpPost]
        public ActionResult GetGridJson(InventoryReq inventoryReq)
        {
            Pagination pagination = inventoryReq.pagination;
            InventoryQry qry = inventoryReq.inventoryQry;

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
            string strQry = "";
            string strOrder = " order by " + pagination.sidx + " " + pagination.sord + " offset " + (pagination.page - 1) * pagination.rows + " rows fetch next " + pagination.rows + " rows only";

            DataView dvInventory = DbHelper.ExecuteToDataView(strSql + strQry + strOrder);

            List<object> inventorys = new List<object>();
            foreach (DataRowView drv in dvInventory)
            {
                inventorys.Add(new {
                    WareName = Convert.ToString(drv["WareName"]),
                    OrderNo = Convert.ToString(drv["OrderNo"]),
                    ProductType = Convert.ToString(drv["ProductType"]),
                    Grade = Convert.ToString(drv["Grade"]),
                    Strength = Convert.ToString(drv["Strength"]),
                    Length = Convert.ToString(drv["Length"]),
                    HorseValue = Convert.ToString(drv["HorseValue"]),
                    Status = Convert.ToString(drv["Status"]),
                    QuoteType = Convert.ToString(drv["QuoteType"]),
                    Price = Convert.ToString(drv["Price"]),
                    Contract = Convert.ToString(drv["Contract"]),
                    Basis = Convert.ToString(drv["Basis"]),
                    Year = Convert.ToString(drv["Year"]),
                    SailingSchedule = Convert.ToString(drv["SailingSchedule"]),
                    Weight = Convert.ToString(drv["Weight"]),
                    IsRecommend = Convert.ToString(drv["IsRecommend"])
                });
            }    
            return Content(inventorys.ToJson());
        }
        #endregion

        #region ==========分类==========

        #endregion

        #region ==========仓库==========

        #endregion
    }
}
