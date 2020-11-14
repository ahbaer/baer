﻿using Baer.Application.SystemManage;
using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Baer.Web.Areas.SystemManage.Controllers
{
    public class ConfigCategoryController : ControllerBase
    {
        private ConfigCategoryApp configCategoryApp = new ConfigCategoryApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = configCategoryApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (ConfigCategoryEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.CategoryName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeJson()
        {
            var data = configCategoryApp.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (ConfigCategoryEntity item in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.text = item.CategoryName;
                tree.value = item.CategoryName;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson()
        {
            var data = configCategoryApp.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (ConfigCategoryEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.Id) == 0 ? false : true;
                treeModel.id = item.Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = configCategoryApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ConfigCategoryEntity configCategoryEntity, string keyValue)
        {
            configCategoryApp.SubmitForm(configCategoryEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            configCategoryApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
