using NFine.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.Application
{
    public class WareHouseMap : EntityTypeConfiguration<WareHouseEntity>
    {
        public WareHouseMap()
        {
            this.ToTable("WareHouse");
            this.HasKey(t => t.F_Id);
        }
    }
}
