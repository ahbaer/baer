using Baer.Domain.Entity.SystemSecurity;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemSecurity
{
    public class FilterIPMap : EntityTypeConfiguration<FilterIPEntity>
    {
        public FilterIPMap()
        {
            this.ToTable("Sys_FilterIP");
            this.HasKey(t => t.Id);
        }
    }
}
