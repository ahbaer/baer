using Baer.Application.SystemManage;
using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Web.Mvc;


namespace Baer.Web.Areas.SystemManage.Controllers
{
    public class SelDataController : Controller
    {
        private SelDataApp selDataApp = new SelDataApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJson(string table, string itemCode, string itemName, string condition = "", string sort = "", bool asc = true)
        {
            var data = selDataApp.GetItemList(table, itemCode, itemName, condition, sort, asc);
            List<object> list = new List<object>();
            foreach (SelDataEntity item in data)
            {
                list.Add(new { id = item.ItemCode, text = item.ItemName });
            }
            return Content(list.ToJson());
        }
    }
}
