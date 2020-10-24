using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class ConfigCategoryMap : EntityTypeConfiguration<ConfigCategoryEntity>
    {
        public ConfigCategoryMap()
        {
            this.ToTable("Sys_ConfigCategory");
            this.HasKey(t => t.F_Id);
        }
    }
}
