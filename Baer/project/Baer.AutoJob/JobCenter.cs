using Baer.Application.SystemManage;
using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using Quartz;
using System;
using System.Collections.Generic;

namespace Baer.AutoJob
{
    public class JobCenter
    {
        private AutoJobApp autoJobApp = new AutoJobApp();

        public void Start()
        {
            List<AutoJobEntity> obj = autoJobApp.GetList(null);
            AddScheduleJob(obj);
        }

        /// <summary>
        /// 添加任务计划
        /// </summary>
        /// <returns></returns>
        private void AddScheduleJob(List<AutoJobEntity> entityList)
        {
            try
            {
                foreach (AutoJobEntity entity in entityList)
                {
                    if (entity.StartTime == null)
                    {
                        entity.StartTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(entity.StartTime, 1);
                    if (entity.EndTime == null)
                    {
                        entity.EndTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(entity.EndTime, 1);

                    var scheduler = JobScheduler.GetScheduler();
                    IJobDetail job = JobBuilder.Create<JobExecute>().WithIdentity(entity.JobName, entity.JobGroupName).Build();
                    job.JobDataMap.Add("Id", entity.Id);

                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(entity.JobName, entity.JobGroupName)
                                                 .WithCronSchedule(entity.CronExpression)
                                                 .Build();

                    scheduler.ScheduleJob(job, trigger);
                    scheduler.Start();
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger("JobCenter").Error(ex);
            }
        }

        /// <summary>
        /// 清除任务计划
        /// </summary>
        /// <returns></returns>
        public void ClearScheduleJob()
        {
            try
            {
                JobScheduler.GetScheduler().Clear();
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger("JobCenter").Error(ex);
            }
        }
    }
}
