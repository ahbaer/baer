using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.Application
{
    public class ContractEntity : IEntity<ChangeInfoEntity>, ICreationAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }

        //我的属性
        public string ContractName { get; set; }
        public string ContractCode { get; set; }
        public double Price { get; set; }//最新价
        public double NV { get; set; }//最新成交量
        public double V { get; set; }//成交量（已这个为准）
        public string M { get; set; }//市场代码
        public string S { get; set; }//品种代码
        public string C { get; set; }//合约编码
        public string FS { get; set; }//唯一完整编码 =M+S+C
        public DateTime Time { get; set; }//时间戳
        public string ZF { get; set; }//涨幅
        public int? Sort { get; set; }
    }
}
