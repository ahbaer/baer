using NFine.Domain.Entity.Application;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
