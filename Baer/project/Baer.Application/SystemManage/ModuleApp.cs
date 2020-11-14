using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using Baer.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.SystemManage
{
    public class ModuleApp
    {
        private IModuleRepository service = new ModuleRepository();

        public List<ModuleEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_SortCode).ToList();
        }
        public ModuleEntity GetForm(string keyValue)
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
        public string SubmitForm(ModuleEntity moduleEntity, string keyValue)
        {
            string f_Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleEntity.Modify(keyValue);
                service.Update(moduleEntity);
            }
            else
            {
                f_Id = moduleEntity.Create();
                service.Insert(moduleEntity);
            }
            return f_Id;
        }
    }
}
