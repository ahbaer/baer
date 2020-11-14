using Baer.Domain.Infrastructure;

namespace Baer.Domain.Entity.SystemManage
{
    public class ConfigCategoryEntity : BaseEntity
    {
        public string F_ParentId { get; set; }
        public string CategoryName { get; set; }
        public int? F_SortCode { get; set; }
        public string Description { get; set; }
    }
}
