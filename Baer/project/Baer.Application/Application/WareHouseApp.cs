using Baer.Application.SystemSecurity;
using Baer.Code;
using Baer.Domain.Entity.Application;
using Baer.Domain.IRepository.Application;
using Baer.Repository.Application;
using System.Collections.Generic;

namespace Baer.Application.Application
{
    public class WareHouseApp
    {
        private IWareHouseRepository service = new WareHouseRepository();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
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

        public WareHouseEntity GetForm(string id)
        {
            return service.FindEntity(id);
        }

        public void DeleteForm(string id)
        {
            new LogApp().WriteDbLog("删除仓库：" + GetForm(id).WareName, DbLogType.Delete);

            service.Delete(t => t.Id == id);
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
