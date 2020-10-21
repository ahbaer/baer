using System;

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
        public string OrderNo { get; set; }//提单号
        public string ProductType { get; set; }//商品类型
        public string Grade { get; set; }//等级
        public double Strength { get; set; }//强度
        public string Length { get; set; }//长度
        public string HorseValue { get; set; }//码值
        public string Status { get; set; }//产品状态
        public string QuoteType { get; set; }//报价方式
        public decimal Price { get; set; }//报价（一口价人民币）
        public string Contract { get; set; }//合约（一口价美元/期货基差）
        public decimal Basis { get; set; }//基差
        public string Year { get; set; }//年度
        public string SailingSchedule { get; set; }//船期
        public double Weight { get; set; }//库存
        public bool IsRecommend { get; set; }//是否首页推荐
        public string Description { get; set; }//备注
    }
}
