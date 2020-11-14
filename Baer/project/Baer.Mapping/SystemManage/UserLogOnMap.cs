using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class UserLogOnMap : EntityTypeConfiguration<UserLogOnEntity>
    {
        public UserLogOnMap()
        {
            this.ToTable("Sys_UserLogOn");
            this.HasKey(t => t.Id);
        }
    }
}
