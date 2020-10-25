using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    [HandlerApiAuth]
    public class ApiController : Controller
    {
        public ActionResult Test()
        {
            return Content("success");
        }
    }
}
