using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JWFang.Controllers
{
    public class JWFangController : ApiController
    {
        [HttpGet]
        public string GetOne()
        {
            return "1";
        }
    }
}
