using Baer.Data;
using Baer.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Baer.Domain.IRepository.SystemManage
{
    public interface IModuleButtonRepository : IRepositoryBase<ModuleButtonEntity>
    {
        void SubmitCloneButton(List<ModuleButtonEntity> entitys);
    }
}
