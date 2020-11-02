using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class AutoJobMap : EntityTypeConfiguration<AutoJobEntity>
    {
        public AutoJobMap()
        {
            this.ToTable("Sys_AutoJob");
            this.HasKey(t => t.F_Id);
        }
    }
}
