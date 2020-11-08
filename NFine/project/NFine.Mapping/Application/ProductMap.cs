﻿using NFine.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.Application
{
    public class ProductMap : EntityTypeConfiguration<ProductEntity>
    {
        public ProductMap()
        {
            this.ToTable("Product");
            this.HasKey(t => t.F_Id);
        }
    }
}
