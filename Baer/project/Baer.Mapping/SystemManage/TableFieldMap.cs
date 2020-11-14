using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class TableFieldMap : EntityTypeConfiguration<TableFieldEntity>
    {
        public TableFieldMap()
        {
            this.ToTable("Sys_TableFieldInfo");
            this.HasKey(t => t.Id);
        }
    }
}
