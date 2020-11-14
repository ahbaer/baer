using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using Baer.Repository.SystemManage;
using System.Collections.Generic;

namespace Baer.Application.SystemManage
{
    public class SelDataApp
    {
        private ISelDataRepository service = new SelDataRepository();

        public List<SelDataEntity> GetItemList(string table, string itemCode, string itemName, string condition, string sort, bool asc)
        {
            return service.GetItemList(table, itemCode, itemName, condition, sort, asc);
        }
    }
}
