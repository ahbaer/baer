using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.SystemManage
{
    public class AutoJobLogEntity : BaseEntity
    {
        public string JobGroupName { get; set; }
        public string JobName { get; set; }
        public int? LogStatus { get; set; }
        public string Description { get; set; }
    }
}
