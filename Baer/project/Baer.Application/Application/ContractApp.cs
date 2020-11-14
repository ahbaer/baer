using Baer.Code;
using Baer.Domain.Entity.Application;
using Baer.Domain.IRepository.Application;
using Baer.Repository.Application;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.Application
{
    public class ContractApp
    {
        private IContractRepository service = new ContractRepository();

        public List<ContractEntity> GetList(string keyword)
        {
            var expression = ExtLinq.True<ContractEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.ContractName.Contains(keyword));
                expression = expression.Or(t => t.ContractCode.Contains(keyword));
            }
            return service.IQueryable(expression).OrderByDescending(t => t.FS).ToList();
        }

        public void DeleteForm(string id)
        {
            service.Delete(t => t.Id == id);
        }
    }
}
