using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using Baer.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.SystemManage
{
    public class AutoJobApp
    {
        private IAutoJobRepository service = new AutoJobRepository();

        public List<AutoJobEntity> GetList(string jobName)
        {
            var expression = ExtLinq.True<AutoJobEntity>();

            if (!string.IsNullOrEmpty(jobName))
            {
                expression = expression.And(t => t.JobName.Contains(jobName));
            }
            return service.IQueryable(expression).ToList();
        }

        public AutoJobEntity GetForm(string f_Id)
        {
            return service.FindEntity(f_Id);
        }

        public void DeleteForm(string f_Id)
        {
            service.Delete(t => t.Id == f_Id);
        }

        public void SubmitForm(AutoJobEntity areaEntity, string f_Id)
        {
            if (!string.IsNullOrEmpty(f_Id))
            {
                areaEntity.Modify(f_Id);
                service.Update(areaEntity);
            }
            else
            {
                areaEntity.Create();
                service.Insert(areaEntity);
            }
        }
    }
}
