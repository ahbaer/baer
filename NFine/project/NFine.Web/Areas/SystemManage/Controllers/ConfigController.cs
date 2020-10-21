using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class ConfigController : ControllerBase
    {
        private ConfigApp app = new ConfigApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string itemId, string keyword)
        {
            var data = new
            {
                rows = app.GetList(pagination, itemId, keyword),
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
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ConfigEntity config, string keyValue)
        {
            string tip =  app.SubmitForm(config, keyValue);
            return string.IsNullOrEmpty(tip) ? Success("操作成功。") : Error(tip);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string f_Ids)
        {
            foreach (string f_Id in f_Ids.Split(','))
            {
                app.DeleteForm(f_Id);
            }
            return Success("删除成功。");
        }
    }
}
