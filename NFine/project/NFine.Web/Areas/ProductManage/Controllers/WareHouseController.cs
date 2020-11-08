using NFine.Application.Application;
using NFine.Code;
using NFine.Domain.Entity.Application;
using System.Web.Mvc;

namespace NFine.Web.Areas.ProductManage.Controllers
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
        public ActionResult DeleteForm(string f_Ids)
        {
            foreach (string f_Id in f_Ids.Split(','))
            {
                wareHouseApp.DeleteForm(f_Id);
            }
            
            return Success("删除成功。");
        }
    }
}
