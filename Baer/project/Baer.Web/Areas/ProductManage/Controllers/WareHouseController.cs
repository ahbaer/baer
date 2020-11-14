using Baer.Application.Application;
using Baer.Code;
using Baer.Domain.Entity.Application;
using System.Web.Mvc;

namespace Baer.Web.Areas.ProductManage.Controllers
{
    public class WareHouseController : ControllerBase
    {
        private WareHouseApp wareHouseApp = new WareHouseApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = wareHouseApp.GetList(pagination, keyword),
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
            var data = wareHouseApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WareHouseEntity wareHouseEntity, string keyValue)
        {
            wareHouseApp.SubmitForm(wareHouseEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string ids)
        {
            foreach (string id in ids.Split(','))
            {
                wareHouseApp.DeleteForm(id);
            }
            
            return Success("删除成功。");
        }
    }
}
