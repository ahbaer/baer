using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class SelDataController : Controller
    {
        private SelDataApp selDataApp = new SelDataApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJson(string table, string itemCode, string itemName, string condition = "")
        {
            var data = selDataApp.GetItemList(table, itemCode, itemName, condition);
            List<object> list = new List<object>();
            foreach (SelDataEntity item in data)
            {
                list.Add(new { id = item.ItemCode, text = item.ItemName });
            }
            return Content(list.ToJson());
        }
    }
}
