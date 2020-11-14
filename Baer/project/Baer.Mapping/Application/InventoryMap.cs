using Baer.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.Application
{
    public class InventoryMap : EntityTypeConfiguration<InventoryEntity>
    {
        public InventoryMap()
        {
            this.ToTable("Inventory");
            this.HasKey(t => t.Id);
        }
    }
}
