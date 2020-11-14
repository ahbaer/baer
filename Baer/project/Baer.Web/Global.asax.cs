using Baer.AutoJob;
using System.Web.Mvc;
using System.Web.Routing;

namespace Baer.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // 定时任务
            new JobCenter().Start(); 
        }
    }
}