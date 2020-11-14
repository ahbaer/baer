﻿using Baer.Data;
using Baer.Domain.Entity.SystemManage;

namespace Baer.Domain.IRepository.SystemManage
{
    public interface IConfigRepository : IRepositoryBase<ConfigEntity>
    {
        ConfigEntity GetConfigValue(string configName);
    }
}