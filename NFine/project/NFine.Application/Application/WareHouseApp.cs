using NFine.Application.SystemManage;
using NFine.Application.SystemSecurity;
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

        public List<WareHouseEntity> GetList(Pagination pagination, string keyword = "")
        {
            var expression = ExtLinq.True<WareHouseEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.WareName.Contains(keyword));
                expression = expression.Or(t => t.Address.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

        public WareHouseEntity GetForm(string f_Id)
        {
            return service.FindEntity(f_Id);
        }

        public void DeleteForm(string f_id)
        {
            new LogApp().WriteDbLog("删除仓库：" + GetForm(f_id).WareName, DbLogType.Delete);

            service.Delete(t => t.F_Id == f_id);
        }

        public void SubmitForm(WareHouseEntity wareHouseEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                wareHouseEntity.Modify(keyValue);
                service.Update(wareHouseEntity);

                new LogApp().WriteDbLog("修改仓库：" + wareHouseEntity.WareName, DbLogType.Update);
            }
            else
            {
                wareHouseEntity.Create();
                service.Insert(wareHouseEntity);

                new LogApp().WriteDbLog("新增仓库：" + wareHouseEntity.WareName, DbLogType.Create);
            }
        }
    }
}
