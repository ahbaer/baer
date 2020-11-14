using System;

namespace Baer.Domain.Infrastructure
{
    public class BaseEntity : IEntity<BaseEntity>, ICreation, IModification
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatorTime { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string LastModifyUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public string DepartmentId { get; set; }
    }
}
