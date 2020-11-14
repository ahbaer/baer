using Baer.Data;
using Baer.Domain.Entity.SystemManage;
using Baer.Domain.IRepository.SystemManage;
using System.Linq;

namespace Baer.Repository.SystemManage
{
    public class ConfigRepository : RepositoryBase<ConfigEntity>, IConfigRepository
    {
        public ConfigEntity GetConfigValue(string configName)
        {
            return dbcontext.Set<ConfigEntity>().Where(m => m.ConfigName == configName).First();
        }
    }
}
