﻿using System.Web.Mvc;

namespace Baer.Web.Areas.CottonManage
{
    public class ProductManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ProductManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              this.AreaName + "_Default",
              this.AreaName + "/{controller}/{action}/{id}",
              new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
              new string[] { "Baer.Web.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}