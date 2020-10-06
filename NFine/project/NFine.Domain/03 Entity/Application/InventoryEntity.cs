using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.Application
{
    public class InventoryEntity : IEntity<InventoryEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public bool? F_DeleteMark { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }

        //我的属性
        public string WareId { get; set; }
        public string WareName { get; set; }
        public string ProductCode { get; set; }//商品代码
        public string ProductName { get; set; }//商品名称
        public string ProductType { get; set; }//商品类型
        public string OriginCode { get; set; }//产地代码
        public string Year { get; set; }//年度
        public string Origin { get; set; }//产地
        public string Harbor { get; set; }//港口
        public double HorseValue { get; set; }//马值
        public double Strength { get; set; }//强度
        public double Length { get; set; }//长度
        public decimal Price { get; set; }//单价
        public double InitWeight { get; set; }//入库重量
        public double Weight { get; set; }//库存重量
        public string IsCustomsClearance { get; set; }//清关状态
        public string Description { get; set; }//备注
    }
}
