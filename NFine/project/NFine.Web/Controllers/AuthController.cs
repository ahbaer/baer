using NFine.Code;
using System;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    public class AuthController : Controller
    {
        public AuthController()
        {
            string machineCode = MachineCode.GetMachineCode(false, false);
            machineCode = DESEncrypt.Encrypt(machineCode);
            ViewBag.MachineCode = "机器码：" + machineCode;
        }

        [HttpGet]
        public virtual ActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SetAuthorizationCode(string authCode)
        {
            try
            {
                string tip;
                if (!Licence.IsLicence(authCode, out tip))
                {
                    return Content(new AjaxResult { state = ResultType.error.ToString(), message = tip }.ToJson());
                }
                try
                {
                    Configs.SetValue("LicenceKey", authCode);
                }
                catch (Exception ex)
                {
                    LogFactory.GetLogger().Error("授权错误1：" + ex.ToString());
                    return Content(new AjaxResult { state = ResultType.error.ToString(), message = "授权码无法写入，请检查文件权限" }.ToJson());
                }
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "授权成功" }.ToJson());
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error("授权错误2：" + ex.ToString());
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = "授权码错误" }.ToJson());
            }
        }
    }
}
