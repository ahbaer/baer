﻿using Baer.Data;
using Baer.Domain.Entity.Application;
using Baer.Domain.IRepository.Application;

namespace Baer.Repository.Application
{
    public class InventoryRepository : RepositoryBase<InventoryEntity>, IInventoryRepository
    {
    }
}