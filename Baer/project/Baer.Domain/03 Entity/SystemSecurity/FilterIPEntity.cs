using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.SystemSecurity
{
    public class DbBackupEntity : BaseEntity
    {
        public string F_BackupType { get; set; }
        public string F_DbName { get; set; }
        public string F_FileName { get; set; }
        public string F_FileSize { get; set; }
        public string F_FilePath { get; set; }
        public DateTime? F_BackupTime { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string Description { get; set; }
    }
}
