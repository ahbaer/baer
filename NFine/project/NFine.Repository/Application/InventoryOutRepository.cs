using NFine.Data;
using NFine.Domain.Entity.Application;
using NFine.Domain.IRepository.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.Application
{
    public class InventoryOutRepository : RepositoryBase<InventoryOutEntity>, IInventoryOutRepository
    {
    }
}