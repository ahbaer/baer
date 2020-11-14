using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class AutoJobMap : EntityTypeConfiguration<AutoJobEntity>
    {
        public AutoJobMap()
        {
            this.ToTable("Sys_AutoJob");
            this.HasKey(t => t.Id);
        }
    }
}
