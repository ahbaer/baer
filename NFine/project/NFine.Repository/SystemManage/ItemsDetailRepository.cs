/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace NFine.Repository.SystemManage
{
    public class ItemsDetailRepository : RepositoryBase<ItemsDetailEntity>, IItemsDetailRepository
    {
        public List<ItemsDetailEntity> GetItemList(string enCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select d.* from Sys_ItemsDetail d inner join Sys_Items i on i.F_Id = d.F_ItemId where i.F_EnCode=@enCode and d.F_EnabledMark = 1 and isnull(d.F_DeleteMark,0)=0 order by d.F_SortCode asc");
            DbParameter[] parameter = 
            {
                 new SqlParameter("@enCode",enCode)
            };
            return this.FindList(strSql.ToString(), parameter);
        }
    }
}
