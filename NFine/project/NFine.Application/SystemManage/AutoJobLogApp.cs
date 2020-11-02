using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.SystemManage
{
    public class AutoJobLogApp
    {
        private IAutoJobLogRepository service = new AutoJobLogRepository();

        public List<AutoJobLogEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<AutoJobLogEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["jobName"].IsEmpty())
            {
                string jobName = queryParam["jobName"].ToString();
                expression = expression.And(t => t.JobName == jobName);
            }
            if (!queryParam["logStatus"].IsEmpty())
            {
                int logStatus = queryParam["logStatus"].ToInt();
                expression = expression.And(t => t.LogStatus == logStatus);
            }
            if (!queryParam["timeType"].IsEmpty())
            {
                string timeType = queryParam["timeType"].ToString();
                DateTime startTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate();
                DateTime endTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate().AddDays(1);
                switch (timeType)
                {
                    case "1":
                        break;
                    case "2":
                        startTime = DateTime.Now.AddDays(-7);
                        break;
                    case "3":
                        startTime = DateTime.Now.AddMonths(-1);
                        break;
                    case "4":
                        startTime = DateTime.Now.AddMonths(-3);
                        break;
                    default:
                        break;
                }
                expression = expression.And(t => t.F_CreatorTime >= startTime && t.F_CreatorTime <= endTime);
            }
            return service.FindList(expression, pagination);
        }

        public void SubmitForm(AutoJobLogEntity autoJobLogEntity, string f_Id)
        {
            if (!string.IsNullOrEmpty(f_Id))
            {
                autoJobLogEntity.Modify(f_Id);
                service.Update(autoJobLogEntity);
            }
            else
            {
                autoJobLogEntity.Create();
                service.Insert(autoJobLogEntity);
            }
        }

        public void RemoveLog(string keepTime)
        {
            DateTime operateTime = DateTime.Now;
            if (keepTime == "7")            //保留近一周
            {
                operateTime = DateTime.Now.AddDays(-7);
            }
            else if (keepTime == "1")       //保留近一个月
            {
                operateTime = DateTime.Now.AddMonths(-1);
            }
            else if (keepTime == "3")       //保留近三个月
            {
                operateTime = DateTime.Now.AddMonths(-3);
            }
            var expression = ExtLinq.True<AutoJobLogEntity>();
            expression = expression.And(t => t.F_CreatorTime <= operateTime);
            service.Delete(expression);
        }
    }
}
