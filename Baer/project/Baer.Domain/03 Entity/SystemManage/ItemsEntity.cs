using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.SystemManage
{
    public class ItemsEntity : BaseEntity
    {
        public string F_ParentId { get; set; }
        public string F_EnCode { get; set; }
        public string F_FullName { get; set; }
        public bool? F_IsTree { get; set; }
        public int? F_Layers { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string Description { get; set; }
    }
}
