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
        private AutoJobLogApp autoJobLogApp = new AutoJobLogApp();

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {
                string f_Id = "", msg = "成功";
                int logStatus = 1;
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
                                    case "Comrms"://期货
                                        if(!new GetComrmsJob().Start())
                                        {
                                            logStatus = 0;
                                            msg = "失败";
                                        }
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
                    msg = ex.ToString();
                    logStatus = 0;
                }

                try
                {
                    if (autoJobEntity != null)
                    {
                        if (autoJobEntity.JobStatus == 1)
                        {
                            //更新下次运行时间
                            DbHelper.ExecuteNonQuery("update Sys_AutoJob set NextStartTime='" + context.NextFireTimeUtc.Value.DateTime.AddHours(8) + "' where F_Id='" + autoJobEntity.F_Id + "'");

                            //记录执行状态
                            FRow fRow = new FRow("Sys_AutoJobLog");
                            fRow["F_CreatorTime"] = context.JobDetail.Key.Group;
                            fRow["JobGroupName"] = context.JobDetail.Key.Name;
                            fRow["LogStatus"] = logStatus;
                            fRow["F_Description"] = msg;
                            fRow.Insert();
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
