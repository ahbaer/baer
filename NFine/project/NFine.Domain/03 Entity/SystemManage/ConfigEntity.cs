/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using System;

namespace NFine.Domain.Entity.SystemManage
{
    public class ConfigEntity : IEntity<ConfigEntity>, ICreationAudited, IModificationAudited, IDeleteAudited
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public string ConfigValue { get; set; }
        public string ConfigName { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
    }
}
