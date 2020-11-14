using Baer.Data;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using System.Collections.Generic;

namespace Baer.Repository.SystemManage
{
    public class SelDataRepository : RepositoryBase<SelDataEntity>, ISelDataRepository
    {
        public List<SelDataEntity> GetItemList(string table, string itemCode, string itemName, string condition, string sort, bool asc)
        {
            string strSql = "select " + itemCode + " as ItemCode," + itemName + " as ItemName from " + table + " where 1=1 ";
            if(!string.IsNullOrEmpty(condition))
            {
                strSql += condition;
            }
            if(string.IsNullOrEmpty(sort))
            {
                strSql += " order by CreatorTime asc";
            }
            else
            {
                if(asc)
                {
                    strSql += " order by " + sort + " asc";
                }
                else
                {
                    strSql += " order by " + sort + " desc";
                }
            }
            return this.FindList(strSql);
        }
    }
}
