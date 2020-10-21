using NFine.Application.Application;
using NFine.Code;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    public class ApiController : Controller
    {
        private InventoryApp inventoryApp = new InventoryApp();

        public ActionResult GetForm(string f_Id)
        {
            var data = inventoryApp.GetForm(f_Id);
            return Content(data.ToJson());
        }
    }
}
