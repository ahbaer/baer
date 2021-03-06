﻿using Baer.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.Application
{
    public class InventoryOutMap : EntityTypeConfiguration<InventoryOutEntity>
    {
        public InventoryOutMap()
        {
            this.ToTable("InventoryOut");
            this.HasKey(t => t.Id);
        }
    }
}
