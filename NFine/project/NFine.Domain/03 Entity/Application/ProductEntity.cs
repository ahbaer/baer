using System;

namespace NFine.Domain.Entity.Application
{
    public class ProductEntity : IEntity<ProductEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
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
        public string ProductName { get; set; }//产品名称
        public string ProductCode { get; set; }//产品代码
        public int Sort { get; set; }//排序
        public string ImgPath { get; set; }//图片地址
        public string Description { get; set; }//备注
    }
}
