using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.Application
{
    public class InventoryOutEntity : BaseEntity
    {
        /// <summary>
        /// 关联入库
        /// </summary>
        public string InventoryId { get; set; }

        /// <summary>
        /// 库存重量
        /// </summary>
        public double Weight { get; set; }
    }
}
