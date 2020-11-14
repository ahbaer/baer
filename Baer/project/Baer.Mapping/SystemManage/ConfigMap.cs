using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class ConfigMap : EntityTypeConfiguration<ConfigEntity>
    {
        public ConfigMap()
        {
            this.ToTable("Sys_Config");
            this.HasKey(t => t.Id);
        }
    }
}
