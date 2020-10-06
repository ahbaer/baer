using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.Application
{
    public class InventoryOutEntity : IEntity<InventoryOutEntity>, ICreationAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }

        //我的属性
        public string InventoryId { get; set; }//关联入库
        public double Weight { get; set; }//库存重量
    }
}
