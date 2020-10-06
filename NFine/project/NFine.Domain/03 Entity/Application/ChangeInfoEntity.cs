using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.Application
{
    public class ChangeInfoEntity : IEntity<ChangeInfoEntity>, ICreationAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }

        //我的属性
        public string ChangeInfo { get; set; }
        public string Type { get; set; }
    }
}
