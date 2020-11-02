using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class AutoJobLogMap : EntityTypeConfiguration<AutoJobLogEntity>
    {
        public AutoJobLogMap()
        {
            this.ToTable("Sys_AutoJobLog");
            this.HasKey(t => t.F_Id);
        }
    }
}
