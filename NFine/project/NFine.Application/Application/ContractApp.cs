using NFine.Code;
using NFine.Domain.Entity.Application;
using NFine.Domain.IRepository.Application;
using NFine.Repository.Application;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.Application
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

        public void DeleteForm(string f_Id)
        {
            service.Delete(t => t.F_Id == f_Id);
        }
    }
}
