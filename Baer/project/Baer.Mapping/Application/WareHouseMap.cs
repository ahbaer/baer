using Baer.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.Application
{
    public class WareHouseMap : EntityTypeConfiguration<WareHouseEntity>
    {
        public WareHouseMap()
        {
            this.ToTable("WareHouse");
            this.HasKey(t => t.Id);
        }
    }
}
