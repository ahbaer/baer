using System;

namespace Baer.Domain
{
    /// <summary>
    /// 修改操作接口
    /// </summary>
    public interface IModification
    {
        /// <summary>
        /// 唯一标志
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        string LastModifyUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime? LastModifyTime { get; set; }
    }
}