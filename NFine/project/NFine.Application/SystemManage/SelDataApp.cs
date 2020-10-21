/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
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
