using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using System.Linq;

namespace NFine.Repository.SystemManage
{
    public class ConfigRepository : RepositoryBase<ConfigEntity>, IConfigRepository
    {
        public ConfigEntity GetConfigValue(string configName)
        {
            return dbcontext.Set<ConfigEntity>().Where(m => m.ConfigName == configName).First();
        }
    }
}
