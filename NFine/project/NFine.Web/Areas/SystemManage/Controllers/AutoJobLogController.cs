using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class AutoJobLogController : ControllerBase
    {
        private AutoJobLogApp autoJobLogApp = new AutoJobLogApp();

        [HttpGet]
        public ActionResult RemoveLog()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = autoJobLogApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult RemoveLog(string keepTime)
        {
            autoJobLogApp.RemoveLog(keepTime);
            return Success("清空成功。");
        }
    }
}
