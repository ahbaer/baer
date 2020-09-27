using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.Application;
using NFine.Domain.IRepository.Application;
using NFine.Repository.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.Application
{
    public class InventoryApp
    {
        private IInventoryRepository service = new InventoryRepository();

        public List<InventoryEntity> GetList(string ware_Id, string keyword = "")
        {
            var expression = ExtLinq.True<InventoryEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Pro_Name.Contains(keyword));
            }
            expression = expression.And(t => t.Ware_Id.Equals(ware_Id));
            return service.IQueryable(expression).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public InventoryEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public void SubmitForm(InventoryEntity inventoryEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                inventoryEntity.Modify(keyValue);
                service.Update(inventoryEntity);
            }
            else
            {
                inventoryEntity.Create();
                service.Insert(inventoryEntity);
            }
        }
    }
}
