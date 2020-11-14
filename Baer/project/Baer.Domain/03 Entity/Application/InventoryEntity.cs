using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.Application
{
    public class InventoryEntity : BaseEntity
    {
        /// <summary>
        /// 关联仓库主键
        /// </summary>
        public string WareId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareName { get; set; }

        /// <summary>
        /// 提单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 强度
        /// </summary>
        public double Strength { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// 码值
        /// </summary>
        public string HorseValue { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 报价方式
        /// </summary>
        public string QuoteType { get; set; }

        /// <summary>
        /// 报价（一口价人民币）
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 合约（一口价美元/期货基差）
        /// </summary>
        public string Contract { get; set; }

        /// <summary>
        /// 基差
        /// </summary>
        public decimal Basis { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 船期
        /// </summary>
        public string SailingSchedule { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 是否首页推荐
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
    }
}
