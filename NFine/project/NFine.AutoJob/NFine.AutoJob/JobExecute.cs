using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Data.Extensions;
using NFine.Domain.Entity.SystemManage;
using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Threading.Tasks;

namespace NFine.AutoJob
{
    public class JobExecute : IJob
    {
        private AutoJobApp autoJobApp = new AutoJobApp();
        //private AutoJobLogApp autoJobLogService = new AutoJobLogService();

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {
                string f_Id = "";
                JobDataMap jobData = null;
                AutoJobEntity autoJobEntity = null;
                try
                {
                    jobData = context.JobDetail.JobDataMap;
                    f_Id = jobData["Id"].ToString();
                    // 获取数据库中的任务
                    autoJobEntity = autoJobApp.GetForm(f_Id);
                    if (autoJobEntity != null)
                    {
                        if (autoJobEntity.JobStatus == 1)
                        {
                            CronTriggerImpl trigger = context.Trigger as CronTriggerImpl;
                            if (trigger != null)
                            {
                                if (trigger.CronExpressionString != autoJobEntity.CronExpression)
                                {
                                    // 更新任务周期
                                    trigger.CronExpressionString = autoJobEntity.CronExpression;
                                    await JobScheduler.GetScheduler().RescheduleJob(trigger.Key, trigger);
                                }

                                #region 执行任务
                                switch (context.JobDetail.Key.Name)
                                {
                                    case "test":
                                        new GetContractJob().Start();
                                        break;
                                    default:
                                        //todo
                                        break;
                                }
                                #endregion
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFactory.GetLogger().Error(ex);
                }

                try
                {
                    if (autoJobEntity != null)
                    {
                        if (autoJobEntity.JobStatus == 1)
                        {
                            #region 更新下次运行时间
                            autoJobApp.SubmitForm(
                                new AutoJobEntity
                                {
                                    NextStartTime = context.NextFireTimeUtc.Value.DateTime.AddHours(8)
                                }, 
                                autoJobEntity.F_Id);
                            #endregion

                            #region 记录执行状态
                            //todo
                            //await autoJobLogService.SaveForm(new AutoJobLogEntity
                            //{
                            //    JobGroupName = context.JobDetail.Key.Group,
                            //    JobName = context.JobDetail.Key.Name,
                            //    LogStatus = obj.Tag,
                            //    Remark = obj.Message
                            //});
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFactory.GetLogger().Error(ex);
                }
            });
        }
    }
}
