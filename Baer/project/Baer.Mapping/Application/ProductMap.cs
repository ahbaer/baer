using Baer.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.Application
{
    public class ProductMap : EntityTypeConfiguration<ProductEntity>
    {
        public ProductMap()
        {
            this.ToTable("Product");
            this.HasKey(t => t.Id);
        }
    }
}
