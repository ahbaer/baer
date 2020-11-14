using Baer.Application;
using Baer.Application.SystemSecurity;
using System.Web.Mvc;

namespace Baer.Web.Areas.ProductManage.Controllers
{
    public class BannerController : ControllerBase
    {
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Log()
        {
            new LogApp().WriteDbLog("上传首页轮播图", DbLogType.Create);
            return View();
        }
    }
}
