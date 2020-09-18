using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Bizlogic.Models
{
    public class Card : IComparable<Card>
    {
        public int color { get; set; }
        public int point { get; set; }

        public Card(int _color, int _point)
        {
            this.color = _color;
            this.point = _point;
        }


        public int CompareTo(Card card)
        {
            return this.point == card.point ? (this.color.CompareTo(card.color)) : card.point.CompareTo(this.point);
        }
    }
}
