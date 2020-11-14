using Baer.Data;
using Baer.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Baer.Domain.IRepository.SystemManage
{
    public interface ISelDataRepository : IRepositoryBase<SelDataEntity>
    {
        List<SelDataEntity> GetItemList(string table, string itemCode, string itemName, string condition, string sort, bool asc);
    }
}
