/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NFine.Repository.SystemManage
{
    public class ConfigRepository : RepositoryBase<ConfigEntity>, IConfigRepository
    {
        public ConfigEntity GetConfigValue(string configName)
        {
            return dbcontext.Set<ConfigEntity>().Where(m => m.ConfigName == configName).First();
        }
    }
}
