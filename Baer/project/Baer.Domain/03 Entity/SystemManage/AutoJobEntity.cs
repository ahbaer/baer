using Baer.Domain.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baer.Domain.Entity.SystemManage
{
    public class AutoJobEntity : BaseEntity
    {
        public string JobGroupName { get; set; }
        public string JobName { get; set; }
        public int? JobStatus { get; set; }
        public string CronExpression { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? NextStartTime { get; set; }
        public string Description { get; set; }
    }
}
