using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class TableFieldMap : EntityTypeConfiguration<TableFieldEntity>
    {
        public TableFieldMap()
        {
            this.ToTable("Sys_TableFieldInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
