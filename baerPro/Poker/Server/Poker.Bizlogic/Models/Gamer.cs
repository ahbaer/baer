using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Bizlogic.Models
{
    public class Gamer
    {
        public string name { get; set; }
        public string photoUrl { get; set; }
        public Socket socket { get; set; }
        public bool landlord { get; set; }
        public List<Card> cards { get; set; }

        public Gamer()
        {
            this.landlord = false;
            this.socket = null;
            this.cards = new List<Card>();
        }
    }
}
