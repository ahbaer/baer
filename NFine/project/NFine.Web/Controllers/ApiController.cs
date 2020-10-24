using JWT;
using JWT.Algorithms;
using JWT.Builder;
using NFine.Application.Application;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    public class ApiController : Controller
    {
        private const string secret = "zmb_baer_2020";

        private InventoryApp inventoryApp = new InventoryApp();

        
        public ActionResult GetToken()
        {
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                .WithSecret(secret)
                .AddClaim("exp", UnixEpoch.GetSecondsSince(new UtcDateTimeProvider().GetNow().AddSeconds(60)))
                .AddClaim("claim", "zmb")
                .Encode();
            return Content(token);
        }

        [HandlerApiAuth]
        public ActionResult Test()
        {
            return Content("success");
        }
    }
}
