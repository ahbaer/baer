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
using System;

namespace NFine.Repository.SystemManage
{
    public class SelDataRepository : RepositoryBase<SelDataEntity>, ISelDataRepository
    {
        public List<SelDataEntity> GetItemList(string table, string itemCode, string itemName, string condition)
        {
            string strSql = "select " + itemCode + " as ItemCode," + itemName + " as ItemName from " + table + " where 1=1 " + condition + " order by F_CreatorTime asc";
            return this.FindList(strSql);
        }
    }
}
