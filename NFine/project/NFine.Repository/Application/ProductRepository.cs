using NFine.Data;
using NFine.Domain.Entity.Application;
using NFine.Domain.IRepository.Application;

namespace NFine.Repository.Application
{
    public class ProductRepository : RepositoryBase<ProductEntity>, IProductRepository
    {
    }
}