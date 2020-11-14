using Baer.Code;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using Baer.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace Baer.Application.SystemManage
{
    public class ConfigApp
    {
        private IConfigRepository service = new ConfigRepository();

        public List<ConfigEntity> GetList(Pagination pagination, string parentId = "", string keyword = "")
        {
            var expression = ExtLinq.True<ConfigEntity>();
            if (!string.IsNullOrEmpty(parentId))
            {
                expression = expression.And(t => t.F_ParentId == parentId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.ConfigName.Contains(keyword));
                expression = expression.Or(t => t.ConfigValue.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

        public ConfigEntity GetConfigByName(string configName)
        {
            return service.GetConfigValue(configName);
        }

        public ConfigEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.Id == keyValue);
        }

        public string SubmitForm(ConfigEntity configEntity, string keyValue)
        {
            string tip = "";
            if (service.IQueryable(m => m.ConfigName == configEntity.ConfigName && m.Id != keyValue).ToList().Count > 0)
            {
                tip = "已存在该系统参数！";
            }
            else
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    configEntity.Modify(keyValue);
                    service.Update(configEntity);
                }
                else
                {
                    configEntity.Create();
                    service.Insert(configEntity);
                }
            }

            return tip;
        }
    }
}
