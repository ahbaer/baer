using Baer.Data;
using Baer.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Baer.Domain.IRepository.SystemManage
{
    public interface IItemsDetailRepository : IRepositoryBase<ItemsDetailEntity>
    {
        List<ItemsDetailEntity> GetItemList(string enCode);
    }
}
