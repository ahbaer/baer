using Baer.Application.Application;
using Baer.Application.SystemManage;
using Baer.Code;
using Baer.Data.Extensions;
using Baer.Domain.Entity.Application;
using Baer.Domain.Entity.SystemManage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Baer.Web.Controllers
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
                //baners.Add(new { name = item.FileName, base64 = item.FileContent });
                if (!string.IsNullOrEmpty(item.FilePath))
                {
                    baners.Add(new { name = item.FileName, base64 = item.FilePath.Replace("\\", "/") });
                }
                
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
                products.Add(new {
                    productName = item.ProductName,
                    productCode = item.ProductCode,
                    baer64 = item.ImgContent });
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
                List<ItemsDetailEntity> itemDetails = itemsDetailApp.GetList(itemType.Id, "");
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
            //sql
            string strSql = @"
                select
                    a.Id,
	                WareName,
	                OrderNo,
	                c.ProductName as ProductType,
	                c.ImgContent as ProductImg,
	                (select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0202' and F_ItemCode=a.Grade) as Grade,
	                Strength,
	                (select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0208' and F_ItemCode=a.Length) as Length,
	                (select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0203' and F_ItemCode=a.HorseValue) as HorseValue,
	                (select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0207' and F_ItemCode=a.Status) as Status,
	                (select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0206' and F_ItemCode=a.QuoteType) as QuoteType,
	                (case when a.QuoteType<>'Futures' then a.Price else null end) as Price,
	                (case when a.QuoteType<>'Futures' then null else a.Contract end) as Contract,
	                (case when a.QuoteType<>'Futures' then null else cast(b.Price as decimal(18,2)) end) as ContractPrice,
	                (case when a.QuoteType<>'Futures' then null else cast(a.Basis+b.Price as decimal(18,2)) end) as TotalPrice,
	                (case when a.QuoteType<>'Futures' then null else Basis end) as Basis,
	                Year,
	                SailingSchedule,
	                (Weight-isnull((select sum(Weight) from InventoryOut where InventoryId=a.Id),0)) as Weight,
	                IsRecommend,
	                b.FS,b.M,b.S,b.C,b.V
                from Inventory a
	                left join Contract b on a.Contract=b.ContractCode
                    left join Product c on c.ProductCode=a.ProductType
                where 1=1 ";

            //where
            string strQry = "";
            if (inventoryReq.inventoryQry != null)
            {
                InventoryQry[] inventoryQrys = inventoryReq.inventoryQry;
                foreach (InventoryQry inventoryQry in inventoryQrys)
                {
                    if (inventoryQry.code.ToLower() == "strength")//强力是范围，其他都是选项
                    {
                        foreach (List selected in inventoryQry.list)
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
                    else if(inventoryQry.code.ToLower() == "producttype")//分类中，其他是除美棉/澳棉/巴西棉/印度棉以外的所有
                    {
                        string condition = "";
                        foreach (List selected in inventoryQry.list)
                        {
                            if(selected.code.ToLower() != "qt")
                            {
                                if (selected.selected)
                                {
                                    condition += "'" + selected.code + "',";
                                }
                            }
                            else
                            {
                                if (selected.selected)
                                {
                                    DataView dvQT = DbHelper.ExecuteToDataView("select ProductCode from Product where ProductCode not in ('mm','bxm','ydm','om')");
                                    foreach (DataRowView drv in dvQT)
                                    {
                                        condition += "'" + drv["ProductCode"] + "',";
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(condition))
                        {
                            strQry += " and " + inventoryQry.code + " in (" + condition.Trim(',') + ") ";
                        }
                    }
                    else
                    {
                        string condition = "";
                        foreach (List selected in inventoryQry.list)
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
            }

            //order
            string strOrder = " order by IsRecommend desc,a.CreatorTime desc ";

            //paging
            Pagination pagination = inventoryReq.pagination;
            string strPaging = " offset " + (pagination.page - 1) * pagination.rows + " rows fetch next " + pagination.rows + " rows only";

            DataView dvInventory = DbHelper.ExecuteToDataView(strSql + strQry + strOrder + strPaging);
            List<object> inventorys = new List<object>();
            foreach (DataRowView drv in dvInventory)
            {
                FileApp fileApp = new FileApp();
                var files = fileApp.GetList(Convert.ToString(drv["Id"]));
                List<object> img = new List<object>();
                foreach (FileEntity file in files)
                {
                    //baer64改成url，参数名不变了
                    //img.Add(new { base64 = file.FileContent });
                    if (!string.IsNullOrEmpty(file.FilePath))
                    {
                        img.Add(new { base64 = file.FilePath.Replace("\\", "/") });
                    }
                }
                inventorys.Add(new
                {
                    f_Id = Convert.ToString(drv["Id"]),
                    wareName = Convert.ToString(drv["WareName"]),
                    orderNo = Convert.ToString(drv["OrderNo"]),
                    productType = Convert.ToString(drv["ProductType"]),
                    productImg = Convert.ToString(drv["ProductImg"]),
                    grade = Convert.ToString(drv["Grade"]),
                    strength = Convert.ToString(drv["Strength"]),
                    length = Convert.ToString(drv["Length"]),
                    horseValue = Convert.ToString(drv["HorseValue"]),
                    status = Convert.ToString(drv["Status"]),
                    quoteType = Convert.ToString(drv["QuoteType"]),
                    price = Convert.ToString(drv["Price"]),
                    contract = Convert.ToString(drv["Contract"]),
                    contractPrice = Convert.ToString(drv["ContractPrice"]),
                    basis = Convert.ToString(drv["Basis"]),
                    totalPrice = Convert.ToString(drv["TotalPrice"]),
                    year = Convert.ToString(drv["Year"]),
                    sailingSchedule = Convert.ToString(drv["SailingSchedule"]),
                    weight = Convert.ToString(drv["Weight"]),
                    isRecommend = Convert.ToString(drv["IsRecommend"]),
                    fs = Convert.ToString(drv["FS"]),
                    m = Convert.ToString(drv["M"]),
                    s = Convert.ToString(drv["S"]),
                    c = Convert.ToString(drv["C"]),
                    v = Convert.ToString(drv["V"]),
                    imgList = img
                });
            }

            return Content(inventorys.ToJson());
        }
        #endregion

        #region ==========详情==========
        [HttpGet]
        public ActionResult GetInventory(string f_Id, string longitude, string latitude)
        {
            string strSql = @"select a.Id,a.WareId,a.WareName,d.Address,d.Longitude,d.Latitude,Distince=6378137.0*acos(sin(cast('" + latitude + "' as decimal(18,6))/180*pi())*sin(Latitude/180*pi())+cos(cast('" + latitude + "' as decimal(18,6))/180*pi())*cos(Latitude/180*pi())*cos((cast('" + longitude + "' as decimal(18,6))-Longitude)/180*pi())),a.OrderNo,c.ProductName as ProductType,(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0202' and F_ItemCode=a.Grade) as Grade,a.Strength,(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0208' and F_ItemCode=a.Length) as Length,(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0203' and F_ItemCode=a.HorseValue) as HorseValue,(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0207' and F_ItemCode=a.Status) as Status,(select F_ItemName from View_ItemType_ItemDetail where F_EnCode='0206' and F_ItemCode=a.QuoteType) as QuoteType,(case when a.QuoteType<>'Futures' then a.Price else null end) as Price,(case when a.QuoteType<>'Futures' then null else a.Contract end) as Contract,(case when a.QuoteType<>'Futures' then null else cast(b.Price as decimal(18,2)) end) as ContractPrice,(case when a.QuoteType<>'Futures' then null else cast(a.Basis+b.Price as decimal(18,2)) end) as TotalPrice,(case when a.QuoteType<>'Futures' then null else Basis end) as Basis,a.Year,a.SailingSchedule,(a.Weight-isnull((select sum(Weight) from InventoryOut where InventoryId=a.Id),0)) as Weight,a.Description from Inventory a left join Contract b on a.Contract=b.ContractCode left join Product c on c.ProductCode=a.ProductType inner join WareHouse d on a.WareId=d.Id where a.Id='" + f_Id + "'";
            DataView dv = DbHelper.ExecuteToDataView(strSql);
            FileApp fileApp = new FileApp();
            var files = fileApp.GetList(f_Id);
            List<object> img = new List<object>();
            foreach (FileEntity file in files)
            {
                //img.Add(new { base64 = file.FileContent });
                if (!string.IsNullOrEmpty(file.FilePath))
                {
                    img.Add(new { base64 = file.FilePath.Replace("\\", "/") });
                }
            }

            object obj = new object();
            obj = new
            {
                f_Id = Convert.ToString(dv[0]["Id"]),
                wareId = Convert.ToString(dv[0]["WareId"]),
                wareName = Convert.ToString(dv[0]["WareName"]),
                address = Convert.ToString(dv[0]["Address"]),
                distance = (Convert.ToDouble(dv[0]["Distince"]) / 1000).ToString("#0.00"),
                longitude = Convert.ToDouble(dv[0]["Longitude"]),
                latitude = Convert.ToDouble(dv[0]["Latitude"]),
                orderNo = Convert.ToString(dv[0]["OrderNo"]),
                productType = Convert.ToString(dv[0]["ProductType"]),
                grade = Convert.ToString(dv[0]["Grade"]),
                strength = Convert.ToString(dv[0]["Strength"]),
                length = Convert.ToString(dv[0]["Length"]),
                horseValue = Convert.ToString(dv[0]["HorseValue"]),
                status = Convert.ToString(dv[0]["Status"]),
                quoteType = Convert.ToString(dv[0]["QuoteType"]),
                price = Convert.ToString(dv[0]["Price"]),
                contract = Convert.ToString(dv[0]["Contract"]),
                contractPrice = Convert.ToString(dv[0]["ContractPrice"]),
                totalPrice = Convert.ToString(dv[0]["TotalPrice"]),
                basis = Convert.ToString(dv[0]["Basis"]),
                year = Convert.ToString(dv[0]["Year"]),
                sailingSchedule = Convert.ToString(dv[0]["SailingSchedule"]),
                weight = Convert.ToString(dv[0]["Weight"]),
                description = Convert.ToString(dv[0]["Description"]),
                imgList = img
            };

            return Content(obj.ToJson());
        }
        #endregion

        #region ==========期货==========
        [HttpGet]
        public ActionResult GetComrms()
        {
            List<object> comrms = new List<object>();

            DataView dv = DbHelper.ExecuteToDataView("select * from Contract where FS not in ('USDCNH','CZCECF','ICECT') order by V desc");
            double v = 0;
            int nvCount = 0;
            if (dv.Count > 0)
            {
                v = Convert.ToDouble(dv[0]["V"]);
            }
            foreach (DataRowView drv in dv)
            {
                if (Convert.ToDouble(drv["V"]) == v)
                {
                    nvCount++;
                }
                comrms.Add(new
                {
                    contractName = Convert.ToString(drv["ContractName"]),
                    contractCode = Convert.ToString(drv["ContractCode"]),
                    price = Convert.ToString(drv["Price"]),
                    isTop = Convert.ToDouble(drv["V"]) == v,
                    m = Convert.ToString(drv["M"]),
                    s = Convert.ToString(drv["S"]),
                    c = Convert.ToString(drv["C"]),
                    fs = Convert.ToString(drv["FS"]),
                    time = Convert.ToDateTime(drv["Time"]).ToString("yyyy年MM月dd日HH:mm:ss"),
                    zf = Convert.ToString(drv["ZF"])
                });
            }

            //插入美国主棉
            DataView dvNoSort = DbHelper.ExecuteToDataView("select * from Contract where FS in ('USDCNH','CZCECF','ICECT') order by Sort desc");
            for (int i = 0; i < dvNoSort.Count; i++)
            {
                int insertIndex = nvCount;
                if (i == dvNoSort.Count - 1)
                {
                    insertIndex = 0;
                }
                comrms.Insert(insertIndex, new
                {
                    contractName = Convert.ToString(dvNoSort[i]["ContractName"]),
                    contractCode = Convert.ToString(dvNoSort[i]["ContractCode"]),
                    price = Convert.ToString(dvNoSort[i]["Price"]),
                    isTop = false,
                    m = Convert.ToString(dvNoSort[i]["M"]),
                    s = Convert.ToString(dvNoSort[i]["S"]),
                    c = Convert.ToString(dvNoSort[i]["C"]),
                    fs = Convert.ToString(dvNoSort[i]["FS"]),
                    time = Convert.ToDateTime(dvNoSort[i]["Time"]).ToString("yyyy年MM月dd日HH:mm:ss"),
                    zf = Convert.ToString(dvNoSort[i]["ZF"])
                });
            }


            return Content(comrms.ToJson());
        }
        #endregion
    }
}
