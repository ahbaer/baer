using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class RoleAuthorizeMap : EntityTypeConfiguration<RoleAuthorizeEntity>
    {
        public RoleAuthorizeMap()
        {
            this.ToTable("Sys_RoleAuthorize");
            this.HasKey(t => t.Id);
        }
    }
}
