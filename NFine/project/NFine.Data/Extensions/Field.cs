using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Data.Extensions
{
    public class Field
    {
        public string fieldName { get; set; }
        public object fieldValue { get; set; }

        public Field(string name, object value)
        {
            this.fieldName = name;
            this.fieldValue = value;
        }
    }
}
