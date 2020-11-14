using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.SystemManage
{
    public class TableEntity : BaseEntity
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表名（中文）
        /// </summary>
        public string TableChineseName { get; set; }

        /// <summary>
        /// 自动生成代码类名前缀
        /// </summary>
        public string ClassPrefix { get; set; }

        /// <summary>
        /// 自动生成代码输出模块
        /// </summary>
        public string OutputModel { get; set; }

        /// <summary>
        /// 自动生成代码列表选中字段
        /// </summary>
        public string SelectListField { get; set; }

        /// <summary>
        /// 自动生成代码表单选中字段
        /// </summary>
        public string SelectFormField { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
    }
}
