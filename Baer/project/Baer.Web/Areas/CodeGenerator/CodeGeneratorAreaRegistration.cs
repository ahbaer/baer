using System.Web.Mvc;

namespace Baer.Web.Areas.CottonManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-11-12 23:11
    /// 描 述：
    /// <summary>
    public class CodeGeneratorAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// 获取路由
        /// <summary>
        public override string AreaName
        {
            get
            {
                return "CodeGenerator";
            }
        }

        /// <summary>
        /// 注册表
        /// <summary>
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
