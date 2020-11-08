using NFine.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.Application
{
    public class InventoryMap : EntityTypeConfiguration<InventoryEntity>
    {
        public InventoryMap()
        {
            this.ToTable("Inventory");
            this.HasKey(t => t.F_Id);
        }
    }
}
