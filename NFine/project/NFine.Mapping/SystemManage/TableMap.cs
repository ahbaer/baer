using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class TableMap : EntityTypeConfiguration<TableEntity>
    {
        public TableMap()
        {
            this.ToTable("Sys_TableInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
