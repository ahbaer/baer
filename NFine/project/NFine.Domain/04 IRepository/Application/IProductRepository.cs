using NFine.Data;
using NFine.Domain.Entity.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.IRepository.Application
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
    }
}
