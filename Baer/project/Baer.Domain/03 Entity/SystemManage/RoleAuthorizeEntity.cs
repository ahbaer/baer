using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.SystemManage
{
    public class RoleAuthorizeEntity : BaseEntity
    {
        public string Id { get; set; }
        public int? F_ItemType { get; set; }
        public string F_ItemId { get; set; }
        public int? F_ObjectType { get; set; }
        public string F_ObjectId { get; set; }
        public int? F_SortCode { get; set; }
    }
}
