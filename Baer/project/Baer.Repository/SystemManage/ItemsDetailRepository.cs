using Baer.Data;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Baer.Repository.SystemManage
{
    public class ItemsDetailRepository : RepositoryBase<ItemsDetailEntity>, IItemsDetailRepository
    {
        public List<ItemsDetailEntity> GetItemList(string enCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select d.* from Sys_ItemsDetail d inner join Sys_Items i on i.Id = d.F_ItemId where i.F_EnCode=@enCode and d.F_EnabledMark = 1 order by d.F_SortCode asc");
            DbParameter[] parameter = 
            {
                 new SqlParameter("@enCode",enCode)
            };
            return this.FindList(strSql.ToString(), parameter);
        }
    }
}
