using Baer.Domain.Infrastructure;

namespace Baer.Domain.Entity.SystemManage
{
    public class ConfigEntity : BaseEntity
    {
        public string F_ParentId { get; set; }
        public string ConfigValue { get; set; }
        public string ConfigName { get; set; }
        public string Description { get; set; }
    }
}
