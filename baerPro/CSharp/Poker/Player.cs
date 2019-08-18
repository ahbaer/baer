using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Player
    {
        public string PlayerName { get; set; }
        public string PlayerGuid { get; set; }
        public string Password { get; set; }
        public DataView Cards { get; set; }

        public Player(string name, string guid)
        {
            this.PlayerName = "【" + name + "】";
            this.PlayerGuid = guid;
        }
    }
}
