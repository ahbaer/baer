using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NFine.Domain.Entity.SystemManage
{
    public class AutoJobEntity : IEntity<AreaEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public bool? F_DeleteMark { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }

        public string JobGroupName { get; set; }
        public string JobName { get; set; }
        public int? JobStatus { get; set; }
        public string CronExpression { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? NextStartTime { get; set; }
        public string F_Description { get; set; }
    }
}
