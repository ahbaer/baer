using Baer.Domain.Entity.CodeGenerator;
using System.Data.Entity.ModelConfiguration;

namespace Baer.Mapping.CodeGenerator
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-11-12 23:11
    /// 描 述：
    /// <summary>
    public class BaerMap : EntityTypeConfiguration<BaerEntity>
    {
        /// <summary>
        /// 构造函数
        /// <summary>
        public BaerMap()
        {
            this.ToTable("Baer");
            this.HasKey(t => t.Id);
        }
    }
}
