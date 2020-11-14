using Baer.Domain.Infrastructure;

namespace Baer.Domain.Entity.SystemManage
{
    public class FileEntity : BaseEntity
    {
        public string Related_Id { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string FileContent { get; set; }
    }
}
