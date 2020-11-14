using Baer.Application.SystemSecurity;
using Baer.Code;
using Baer.Domain.Entity.Application;
using Baer.Domain.IRepository.Application;
using Baer.Repository.Application;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.Application
{
    public class InventoryOutApp
    {
        private IInventoryOutRepository service = new InventoryOutRepository();

        public List<InventoryOutEntity> GetList(string inventoryId)
        {
            return service.IQueryable(t => t.InventoryId == inventoryId).OrderByDescending(t => t.CreatorTime).ToList();
        }

        public InventoryOutEntity GetForm(string id)
        {
            return service.FindEntity(id);
        }

        public void SubmitForm(InventoryOutEntity inventoryOutEntity, string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                inventoryOutEntity.Modify(id);
                service.Update(inventoryOutEntity);
            }
            else
            {
                inventoryOutEntity.Create();
                service.Insert(inventoryOutEntity);

                new LogApp().WriteDbLog(
                    new ProductApp().GetNameByCode(
                        new InventoryApp().GetForm(inventoryOutEntity.InventoryId).ProductType) + "出库：" + inventoryOutEntity.Weight + "吨", 
                    DbLogType.Create);
            }
        }
    }
}
