using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.SystemManage
{
    public class TableFieldEntity : IEntity<TableFieldEntity>, ICreationAudited, IModificationAudited, IDeleteAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public bool? F_DeleteMark { get; set; }

        //我的属性
        public string TableId { get; set; }
        public string FieldName { get; set; }//字段名
        public string FieldChineseName { get; set; }//字段中文名
        public string FieldType { get; set; }//字段类型
        public int FieldLength { get; set; }//字段长度
        public int FieldAccuracy { get; set; }//字段精度
        public string F_Description { get; set; }//备注
    }
}
