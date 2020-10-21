using NFine.Application.Application;
using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity.Application;
using System.Web.Mvc;

namespace NFine.Web.Areas.ProductManage.Controllers
{
    public class InventoryController : ControllerBase
    {
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
        public ActionResult GetAllWeight(string queryJson)
        {
            var data = inventoryApp.GetAllWeight(queryJson);
            return Content(data);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(InventoryEntity inventoryEntity, string keyValue)
        {
            string f_Id = inventoryApp.SubmitForm(inventoryEntity, keyValue);
            return Success("操作成功。", f_Id);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string f_Ids)
        {
            foreach (string f_Id in f_Ids.Split(','))
            {
                inventoryApp.DeleteForm(f_Id);
            }
            return Success("删除成功。");
        }

        
    }
}
