using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class FileMap : EntityTypeConfiguration<FileEntity>
    {
        public FileMap()
        {
            this.ToTable("Frame_File");
            this.HasKey(t => t.F_Id);
        }
    }
}
