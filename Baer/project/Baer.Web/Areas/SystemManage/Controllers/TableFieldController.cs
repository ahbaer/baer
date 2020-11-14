using Baer.Application.SystemManage;
using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using System.Web.Mvc;

namespace Baer.Web.Areas.SystemManage.Controllers
{
    public class TableFieldController : ControllerBase
    {
        private TableFieldApp app = new TableFieldApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string tableId, string keyword = "")
        {
            var data = new
            {
                rows = app.GetList(pagination, tableId, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string id)
        {
            var data = app.GetForm(id);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(TableFieldEntity tableFieldEntity, string id)
        {
            app.SubmitForm(tableFieldEntity, id);
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
                app.DeleteForm(id);
            }
            return Success("删除成功。");
        }
    }
}
