using NFine.Application.Application;
using NFine.Code;
using System.Web.Mvc;

namespace NFine.Web.Areas.ProductManage.Controllers
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
