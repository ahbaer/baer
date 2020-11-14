using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.Application
{
    public class ContractEntity : BaseEntity
    {
        /// <summary>
        /// 代码名称
        /// </summary>
        public string ContractName { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// 最新价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 最新成交量
        /// </summary>
        public double NV { get; set; }

        /// <summary>
        /// 成交量（已这个为准）
        /// </summary>
        public double V { get; set; }

        /// <summary>
        /// 市场代码
        /// </summary>
        public string M { get; set; }

        /// <summary>
        /// 品种代码
        /// </summary>
        public string S { get; set; }

        /// <summary>
        /// 合约编码
        /// </summary>
        public string C { get; set; }

        /// <summary>
        /// 唯一完整编码 =M+S+C
        /// </summary>
        public string FS { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 涨幅
        /// </summary>
        public string ZF { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
    }
}
