using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
{
    public class ItemsApp
    {
        private IItemsRepository service = new ItemsRepository();

        public List<ItemsEntity> GetList(string account)
        {
            var expression = ExtLinq.True<ItemsEntity>();

            if (string.IsNullOrEmpty(account) || !account.Equals("admin"))
            {
                expression = expression.And(t => t.F_FullName != "通用字典");
            }
            return service.IQueryable(expression).ToList();
        }

        public List<ItemsEntity> GetApiList(string f_EnCode)
        {
            var entitys = service.IQueryable(t => t.F_EnCode == f_EnCode).ToList();
            var retEntitys = new List<ItemsEntity>();
            if (entitys.Count > 0)
            {
                var f_Id = entitys[0].F_Id;
                retEntitys = service.IQueryable(t => t.F_ParentId == f_Id).ToList();
            }
            return retEntitys;
        }

        public ItemsEntity GetForm(string keyValue)
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
                service.Delete(t => t.F_Id == keyValue);
            }
        }

        public void SubmitForm(ItemsEntity itemsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsEntity.Modify(keyValue);
                service.Update(itemsEntity);
            }
            else
            {
                itemsEntity.Create();
                service.Insert(itemsEntity);
            }
        }
    }
}
