using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.SystemManage
{
    public class FileEntity : IEntity<FileEntity>, ICreationAudited, IModificationAudited, IDeleteAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public bool? F_DeleteMark { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }

        //我的属性
        public string Related_Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileType { get; set; }
    }
}
