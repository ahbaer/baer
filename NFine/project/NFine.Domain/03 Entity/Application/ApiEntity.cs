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
        public InventoryQry[] inventoryQry { get; set; }
    }

    public class InventoryQry
    {
        public string name { get; set; }
        public string code { get; set; }
        public string selectedName { get; set; }
        public bool isActive { get; set; }
        public SelectedList[] selectedList { get; set; }
    }

    public class SelectedList
    {
        public string name { get; set; }
        public string code { get; set; }
        public bool selected { get; set; }
    }

}
