using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using Baer.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.SystemManage
{
    public class ConfigCategoryApp
    {
        private IConfigCategroyRepository service = new ConfigCategoryRepository();

        public List<ConfigCategoryEntity> GetList()
        {
            return service.IQueryable().ToList();
        }

        public ConfigCategoryEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.Id == keyValue);
            }
        }

        public void SubmitForm(ConfigCategoryEntity configCategoryEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                configCategoryEntity.Modify(keyValue);
                service.Update(configCategoryEntity);
            }
            else
            {
                configCategoryEntity.Create();
                service.Insert(configCategoryEntity);
            }
        }
    }
}
