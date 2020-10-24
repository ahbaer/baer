using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class ConfigMap : EntityTypeConfiguration<ConfigEntity>
    {
        public ConfigMap()
        {
            this.ToTable("Sys_Config");
            this.HasKey(t => t.F_Id);
        }
    }
}
