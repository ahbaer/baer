using System;

namespace NFine.Domain.Entity.SystemManage
{
    public class AutoJobLogEntity : IEntity<AreaEntity>, ICreationAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }

        public string JobGroupName { get; set; }
        public string JobName { get; set; }
        public int? LogStatus { get; set; }
        public string F_Description { get; set; }
    }
}
