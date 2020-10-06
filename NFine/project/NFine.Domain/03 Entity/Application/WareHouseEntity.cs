using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.Application
{
    public class WareHouseEntity : IEntity<WareHouseEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
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
        public string WareName { get; set; }
        public string Address { get; set; }//地址
        public string Director { get; set; }//仓库主管
        public string Mobile { get; set; }//联系电话
        public double Longitude { get; set; }//经度
        public double Latitude { get; set; }//纬度
        public string Description { get; set; }//备注
    }
}
