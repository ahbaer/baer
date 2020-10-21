/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity;
using NFine.Domain.Entity.Application;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    [HandlerLogin]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetChangeInfoJson()
        {
            DataView dvChange = DbHelper.ExecuteToDataView("select top 11 * from ChangeInfo order by F_Id desc");
            List<ShowChange> changes = new List<ShowChange>();
            foreach (DataRowView drv in dvChange)
            {
                ShowChange change = new ShowChange()
                {
                    ChangeInfo = Convert.ToString(drv["ChangeInfo"]),
                    Type = Convert.ToString(drv["Type"]),
                    F_CreatorTime = Convert.ToDateTime(drv["F_CreatorTime"]).ToString("yyy-MM-dd")
                };

                changes.Add(change);
            }
            return Content(changes.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetInventoryJson()
        {
            string[] color = { "#F7464A", "#46BFBD", "#FDB45C", "#949FB1", "#1ABC9C" };
            string[] highlight = { "#FF5A5E", "#5AD3D1", "#FFC870", "#A8B3C5", "#1AC89C" };
            DataView dvInventory = DbHelper.ExecuteToDataView("select a.WareName,sum(b.Weight) Weight from WareHouse a inner join Inventory b on a.F_Id=b.WareId group by a.F_Id,a.WareName,a.F_CreatorTime order by Weight desc");
            List<DoughnutDataEntity> doughnutDatas = new List<DoughnutDataEntity>();

            int otherValue = 0;
            for (int i = 0; i < dvInventory.Count; i++)
            {
                if (i < 4)
                {
                    DoughnutDataEntity doughnutDataEntity = new DoughnutDataEntity()
                    {
                        value = Convert.ToString(dvInventory[i]["Weight"]),
                        color = color[i],
                        highlight = highlight[i],
                        label = Convert.ToString(dvInventory[i]["WareName"])
                    };
                    doughnutDatas.Add(doughnutDataEntity);
                }
                else
                {
                    otherValue += (Convert.ToInt32(dvInventory[i]["Weight"]));
                }
            }

            if (otherValue > 0)
            {
                DoughnutDataEntity doughnutDataEntity = new DoughnutDataEntity()
                {
                    value = otherValue.ToString(),
                    color = color[4],
                    highlight = highlight[4],
                    label = "其他"
                };
                doughnutDatas.Add(doughnutDataEntity);
            }
            return Content(doughnutDatas.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetInventoryGroupJson()
        {
            string[] month = new string[12];
            string[] inWeight = new string[12];
            string[] outWeight = new string[12];

            string inGroupSql = @"
                select 
	                top 12 sum(Weight) Weight, cast(year(F_CreatorTime) as varchar(4))+'-'+right('00' +cast(month(F_CreatorTime) as varchar(2)),2) Date 
                from Inventory 
                group by year(F_CreatorTime),month(F_CreatorTime) order by year(F_CreatorTime) desc,month(F_CreatorTime) desc";
            string outGroupSql = @"
                select 
	                top 12 sum(Weight) Weight, cast(year(F_CreatorTime) as varchar(4))+'-'+right('00' +cast(month(F_CreatorTime) as varchar(2)),2) Date 
                from InventoryOut 
                group by year(F_CreatorTime),month(F_CreatorTime) order by year(F_CreatorTime) desc,month(F_CreatorTime) desc";

            DataView inGroup = DbHelper.ExecuteToDataView(inGroupSql);
            DataView outGroup = DbHelper.ExecuteToDataView(outGroupSql);

            for (int i = 0; i < 12; i++)
            {
                month[i] = DateTime.Now.AddMonths(i - 11).ToString("yyyy-MM");
                foreach (DataRowView drv in inGroup)
                {
                    if(month[i] == Convert.ToString(drv["Date"]))
                    {
                        inWeight[i] = Convert.ToString(drv["Weight"]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(inWeight[i]))
                        {
                            inWeight[i] = "0";
                        }
                    }
                }
                foreach (DataRowView drv in outGroup)
                {
                    if (month[i] == Convert.ToString(drv["Date"]))
                    {
                        outWeight[i] = Convert.ToString(drv["Weight"]);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(outWeight[i]))
                        {
                            outWeight[i] = "0";
                        }
                    }
                }
            }

            LineChartDataSet lineChartDataSet1 = new LineChartDataSet()
            {
                value = "入库",
                fillColor = "rgba(220,220,220,0.2)",
                strokeColor = "rgba(220,220,220,1)",
                pointColor = "rgba(220,220,220,1)",
                pointStrokeColor = "#fff",
                pointHighlightFill = "#fff",
                pointHighlightStroke = "rgba(220,220,220,1)",
                data = inWeight
            };

            LineChartDataSet lineChartDataSet2 = new LineChartDataSet()
            {
                value = "出库",
                fillColor = "rgba(151,187,205,0.2)",
                strokeColor = "rgba(151,187,205,1)",
                pointColor = "rgba(151,187,205,1)",
                pointStrokeColor = "#fff",
                pointHighlightFill = "#fff",
                pointHighlightStroke = "rgba(151,187,205,1)",
                data = outWeight
            };

            LineChartData lineChartData = new LineChartData()
            {
                labels = month
            };
            lineChartData.datasets.Add(lineChartDataSet1);
            lineChartData.datasets.Add(lineChartDataSet2);

            return Content(lineChartData.ToJson());
        }
    }
}
