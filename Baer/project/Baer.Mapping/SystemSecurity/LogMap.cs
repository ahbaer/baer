using Baer.Domain.Entity.SystemSecurity;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemSecurity
{
    public class LogMap : EntityTypeConfiguration<LogEntity>
    {
        public LogMap()
        {
            this.ToTable("Sys_Log");
            this.HasKey(t => t.Id);
        }
    }
}
