using Baer.Domain.Infrastructure;
using System;

namespace Baer.Domain.Entity.SystemSecurity
{
    public class FilterIPEntity : BaseEntity
    {
        public bool? F_Type { get; set; }
        public string F_StartIP { get; set; }
        public string F_EndIP { get; set; }
        public int? F_SortCode { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string Description { get; set; }
    }
}
