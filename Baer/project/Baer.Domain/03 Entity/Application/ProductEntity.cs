using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.Application
{
    public class ProductEntity : BaseEntity
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string ImgContent { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
    }
}
