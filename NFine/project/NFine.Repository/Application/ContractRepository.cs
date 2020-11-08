using NFine.Data;
using NFine.Domain.Entity.Application;
using NFine.Domain.IRepository.Application;

namespace NFine.Repository.Application
{
    public class ContractRepository : RepositoryBase<ContractEntity>, IContractRepository
    {
    }
}