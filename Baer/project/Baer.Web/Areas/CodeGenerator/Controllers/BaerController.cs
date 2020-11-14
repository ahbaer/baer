using Baer.Application.CodeGenerator;
using Baer.Code;
using Baer.Domain.Entity.CodeGenerator;
using System.Web.Mvc;

namespace Baer.Web.Areas.CodeGenerator.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-11-12 23:11
    /// 描 述：
    /// <summary>
    public class BaerController : ControllerBase
    {
        private BaerApp app = new BaerApp();

        #region ==========视图==========
        /// <summary>
        /// 列表视图
        /// <summary>
        public ActionResult BaerIndex()
        {
           return View();
        }

        /// <summary>
        /// 表单视图
        /// <summary>
        public ActionResult BaerForm()
        {
           return View();
        }

        /// <summary>
        /// 明细视图
        /// <summary>
        public ActionResult BaerDetails()
        {
           return View();
        }
        #endregion

        #region ==========业务==========
        /// <summary>
        /// 获取列表
        /// <summary>
        /// <param name="pagination">分页</param>
        /// <param name="keyword">查询关键字（需要根据实际调整）</param>
        /// <returns>实体类列表</returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        /// <summary>
        /// 获取表单
        /// <summary>
        /// <param name="f_Id">查询主键</param>
        /// <returns>表单实体类</returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string id)
        {
            var data = app.GetForm(id);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 提交表单
        /// <summary>
        /// <param name="entity">实体类</param>
        /// <param name="f_Id">表单主键</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(BaerEntity entity, string id)
        {
            app.Submit(entity, id);
            return Success("操作成功。");
        }

        /// <summary>
        /// 删除表单
        /// <summary>
        /// <param name="f_Ids">表单主键</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string ids)
        {
            foreach (string id in ids.Split(','))
            {
                app.Delete(id);
            }
            return Success("删除成功。");
        }
        #endregion
    }
}
