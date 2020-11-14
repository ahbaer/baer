using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class ItemsMap : EntityTypeConfiguration<ItemsEntity>
    {
        public ItemsMap()
        {
            this.ToTable("Sys_Items");
            this.HasKey(t => t.Id);
        }
    }
}
