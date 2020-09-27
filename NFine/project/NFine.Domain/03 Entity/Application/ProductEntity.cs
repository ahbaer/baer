using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NFine.Domain.Entity.Application
{
    public class ProductEntity:IEntity<ProductEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
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
        public string Pro_Name { get; set; }//名称
        public string Pro_Code { get; set; }//代码
        public string Pro_Type { get; set; }//品类
        public string Origin { get; set; }//产地
        public string OriginCode { get; set; }//产地代码
        public string Harbor { get; set; }//港口
        public string Year { get; set; }//年度
        public double HorseValue { get; set; }//马值
        public int Strength { get; set; }//强力
        public int Length { get; set; }//长度
        public decimal Price { get; set; }//价格(元/吨 or 美元/每吨)
        public int IsCustomsClearance { get; set; }//是否清关
        public string Pro_Description { get; set; }//备注
    }
}
