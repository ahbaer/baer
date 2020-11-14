using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class TableMap : EntityTypeConfiguration<TableEntity>
    {
        public TableMap()
        {
            this.ToTable("Sys_TableInfo");
            this.HasKey(t => t.Id);
        }
    }
}
