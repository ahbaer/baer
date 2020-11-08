using NFine.Domain.Entity.Application;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.Application
{
    public class ContractMap : EntityTypeConfiguration<ContractEntity>
    {
        public ContractMap()
        {
            this.ToTable("Contract");
            this.HasKey(t => t.F_Id);
        }
    }
}
