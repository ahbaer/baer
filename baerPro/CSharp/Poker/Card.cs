using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Card
    {
        public int Point { get; set; }
        public int Color { get; set; }

        public Card()
        {

        }

        public Card(int point, int color)
        {
            this.Point = point;
            this.Color = color;
        }
    }
}
