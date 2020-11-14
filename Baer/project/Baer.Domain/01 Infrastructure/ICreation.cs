using System;

namespace Baer.Domain
{
    /// <summary>
    /// 创建操作接口
    /// </summary>
    public interface ICreation
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        string CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? CreatorTime { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        string CompanyId { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        string DepartmentId { get; set; }
    }
}