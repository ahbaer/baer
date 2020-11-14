using Baer.Code;
using System;

namespace Baer.Domain
{
    public class IEntity<TEntity>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public string Create()
        {
            var entity = this as ICreation;
            entity.Id = Common.GuId();
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (LoginInfo != null)
            {
                entity.CreatorUserId = LoginInfo.UserId;
                entity.CompanyId = LoginInfo.CompanyId;
                entity.DepartmentId = LoginInfo.CompanyId;
            }
            entity.CreatorTime = DateTime.Now;

            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        public void Modify(string id)
        {
            var entity = this as IModification;
            entity.Id = id;
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (LoginInfo != null)
            {
                entity.LastModifyUserId = LoginInfo.UserId;
            }
            entity.LastModifyTime = DateTime.Now;
        }
    }
}
