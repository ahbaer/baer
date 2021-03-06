﻿/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace NFine.Domain.IRepository.SystemManage
{
    public interface ISelDataRepository : IRepositoryBase<SelDataEntity>
    {
        List<SelDataEntity> GetItemList(string table, string itemCode, string itemName, string condition, string sort, bool asc);
    }
}
