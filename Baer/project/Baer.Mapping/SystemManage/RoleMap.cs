﻿using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class RoleMap : EntityTypeConfiguration<RoleEntity>
    {
        public RoleMap()
        {
            this.ToTable("Sys_Role");
            this.HasKey(t => t.Id);
        }
    }
}
