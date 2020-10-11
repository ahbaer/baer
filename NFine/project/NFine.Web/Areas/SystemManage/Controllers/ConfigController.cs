/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class ConfigController : ControllerBase
    {
        private ConfigApp app = new ConfigApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string itemId, string keyword)
        {
            var data = app.GetList(itemId, keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ConfigEntity config, string keyValue)
        {
            string tip =  app.SubmitForm(config, keyValue);
            return string.IsNullOrEmpty(tip) ? Success("操作成功。") : Error(tip);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
