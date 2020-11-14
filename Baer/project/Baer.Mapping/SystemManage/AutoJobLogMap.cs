using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class AutoJobLogMap : EntityTypeConfiguration<AutoJobLogEntity>
    {
        public AutoJobLogMap()
        {
            this.ToTable("Sys_AutoJobLog");
            this.HasKey(t => t.Id);
        }
    }
}
