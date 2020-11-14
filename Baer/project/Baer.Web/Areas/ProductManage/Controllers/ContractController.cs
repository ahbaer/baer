using Baer.Application.Application;
using Baer.Code;
using System.Web.Mvc;

namespace Baer.Web.Areas.ProductManage.Controllers
{
    public class ContractController : ControllerBase
    {
        private ContractApp app = new ContractApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = app.GetList(keyword);
            return Content(data.ToJson());
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
