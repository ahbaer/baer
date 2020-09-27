using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.Application
{
    public class WareHouseEntity : IEntity<ProductEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
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
        public string Ware_Name { get; set; }
        public double Ware_Longitude { get; set; }//经度
        public double Ware_Latitude { get; set; }//纬度
        public string Ware_Description { get; set; }
    }
}
