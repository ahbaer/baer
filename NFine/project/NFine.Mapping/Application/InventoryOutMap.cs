using NFine.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.Application
{
    public class InventoryOutMap : EntityTypeConfiguration<InventoryOutEntity>
    {
        public InventoryOutMap()
        {
            this.ToTable("InventoryOut");
            this.HasKey(t => t.F_Id);
        }
    }
}
