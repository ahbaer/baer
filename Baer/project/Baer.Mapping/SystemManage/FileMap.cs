using Baer.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.SystemManage
{
    public class FileMap : EntityTypeConfiguration<FileEntity>
    {
        public FileMap()
        {
            this.ToTable("Sys_File");
            this.HasKey(t => t.Id);
        }
    }
}
