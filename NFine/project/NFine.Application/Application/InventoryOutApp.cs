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
    public class InventoryOutApp
    {
        private IInventoryOutRepository service = new InventoryOutRepository();

        public List<InventoryOutEntity> GetList(string inventoryId)
        {
            return service.IQueryable(t => t.InventoryId == inventoryId).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public InventoryOutEntity GetForm(string f_Id)
        {
            return service.FindEntity(f_Id);
        }

        public void SubmitForm(InventoryOutEntity inventoryOutEntity, string f_Id)
        {
            if (!string.IsNullOrEmpty(f_Id))
            {
                inventoryOutEntity.Modify(f_Id);
                service.Update(inventoryOutEntity);
            }
            else
            {
                inventoryOutEntity.Create();
                service.Insert(inventoryOutEntity);
            }
        }
    }
}
