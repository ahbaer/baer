using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class ConfigCategoryMap : EntityTypeConfiguration<ConfigCategoryEntity>
    {
        public ConfigCategoryMap()
        {
            this.ToTable("Sys_ConfigCategory");
            this.HasKey(t => t.Id);
        }
    }
}
