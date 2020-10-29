using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.Application
{
    public class InventoryReq
    {
        public Pagination pagination { get; set; }
        public InventoryQry inventoryQry { get; set; }
    }

    public class InventoryQry
    {
        public string strengthMin { get; set; }
        public string strengthMax { get; set; }
        public string orderNo { get; set; }
        public string isRecommend { get; set; }

        public List<string> productType { get; set; }
        public List<string> grade { get; set; }
        public List<string> length { get; set; }
        public List<string> horseValue { get; set; }
        public List<string> wareId { get; set; }
        public List<string> status { get; set; }
        public List<string> quoteType { get; set; }
    }
}
