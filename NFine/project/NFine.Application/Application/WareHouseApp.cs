using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.Application;
using NFine.Domain.IRepository.Application;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.Application;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.Application
{
    public class WareHouseApp
    {
        private IWareHouseRepository service = new WareHouseRepository();

        public List<WareHouseEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<WareHouseEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Ware_Name.Contains(keyword));
            }
            return service.IQueryable(expression).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public WareHouseEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }

        public void SubmitForm(WareHouseEntity wareHouseEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                wareHouseEntity.Modify(keyValue);
                service.Update(wareHouseEntity);
            }
            else
            {
                wareHouseEntity.Create();
                service.Insert(wareHouseEntity);
            }
        }
    }
}
