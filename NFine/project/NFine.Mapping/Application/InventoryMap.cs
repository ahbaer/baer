using NFine.Domain.Entity.Application;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
