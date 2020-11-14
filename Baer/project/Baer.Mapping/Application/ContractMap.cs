using Baer.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.Application
{
    public class ContractMap : EntityTypeConfiguration<ContractEntity>
    {
        public ContractMap()
        {
            this.ToTable("Contract");
            this.HasKey(t => t.Id);
        }
    }
}
