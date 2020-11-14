using Baer.Application.SystemManage;
using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using System.Web.Mvc;

namespace Baer.Web.Areas.SystemManage.Controllers
{
    public class TableController : ControllerBase
    {
        private TableApp app = new TableApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword = "")
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword),
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
        public ActionResult SubmitForm(TableEntity tableEntity, string id)
        {
            app.SubmitForm(tableEntity, id);
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
