using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class UserMap : EntityTypeConfiguration<UserEntity>
    {
        public UserMap()
        {
            this.ToTable("Sys_User");
            this.HasKey(t => t.Id);
        }
    }
}
