using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaersTool.DB
{
    public class Conn
    {
        public string ConnectionString { get; set; }

        public Conn(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
