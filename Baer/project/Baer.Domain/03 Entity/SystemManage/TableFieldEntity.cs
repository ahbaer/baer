using Baer.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baer.Domain.Entity.SystemManage
{
    public class TableFieldEntity : BaseEntity
    {
        /// <summary>
        /// 关联表主键
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段中文名
        /// </summary>
        public string FieldChineseName { get; set; }

        /// <summary>
        /// 显示类型
        /// </summary>
        public int ShowType { get; set; }

        /// <summary>
        /// 关联数据字典
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public int FieldLength { get; set; }

        /// <summary>
        /// 字段精度
        /// </summary>
        public int FieldAccuracy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
    }
}
