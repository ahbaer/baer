using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.Application
{
    /// <summary>
    /// 仓库
    /// </summary>
    public class WareHouseEntity : BaseEntity
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public string WareName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 主管
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }//备注
    }
}
