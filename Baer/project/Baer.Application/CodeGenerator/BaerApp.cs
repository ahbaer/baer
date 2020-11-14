using Baer.Code;
using Baer.Domain.Entity.CodeGenerator;
using Baer.Domain.IRepository.CodeGenerator;
using Baer.Repository.CodeGenerator;
using System.Collections.Generic;

namespace Baer.Application.CodeGenerator
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-11-12 23:11
    /// 描 述：
    /// <summary>
    public class BaerApp
    {
        private IBaerRepository service = new BaerRepository();

        /// <summary>
        /// 获取列表
        /// <summary>
        /// <param name="pagination">分页</param>
        /// <param name="keyword">查询关键字（需要根据实际调整）</param>
        /// <returns>实体类列表</returns>
        public List<BaerEntity> GetList(Pagination pagination, string keyword = "")
        {
            var expression = ExtLinq.True<BaerEntity>();
            return service.FindList(expression, pagination);
        }

        /// <summary>
        /// 获取表单
        /// <summary>
        /// <param name="f_Id">查询主键</param>
        /// <returns>表单实体类</returns>
        public BaerEntity GetForm(string id)
        {
            return service.FindEntity(id);
        }

        /// <summary>
        /// 删除表单
        /// <summary>
        /// <param name="f_Id">删除主键</param>
        public void Delete(string id)
        {
            service.Delete(t => t.Id == id);
        }

        /// <summary>
        /// 提交表单
        /// <summary>
        /// <param name="entity">实体类</param>
        /// <param name="f_Id">表单主键</param>
        public void Submit(BaerEntity entity, string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                entity.Modify(id);
                service.Update(entity);
            }
            else
            {
                entity.Create();
                service.Insert(entity);
            }
        }
    }
}
