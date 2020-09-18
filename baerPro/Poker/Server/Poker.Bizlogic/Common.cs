using Newtonsoft.Json;
using Poker.Bizlogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Bizlogic
{
    public class Common
    {
        public static byte[] GetSendBytes(Tran tran)
        {
            string send = JsonConvert.SerializeObject(tran);
            return Encoding.Default.GetBytes(send + "$");
        }

        public static string ListToString(List<Card> cards)
        {
            string strCards = "";
            foreach (Card card in cards)
            {
                string caverPoint = "";
                switch (card.point)
                {
                    case 10:
                        caverPoint = "X";
                        break;
                    case 11:
                        caverPoint = "J";
                        break;
                    case 12:
                        caverPoint = "Q";
                        break;
                    case 13:
                        caverPoint = "K";
                        break;
                    case 14:
                        caverPoint = "A";
                        break;
                    case 15:
                        caverPoint = "2";
                        break;
                    case 16:
                        caverPoint = "W";
                        break;
                    default:
                        caverPoint = card.point.ToString();
                        break;
                }
                strCards += caverPoint;
            }

            return strCards;
        }

        public static bool IsContains(List<Card> compareCards, List<Card> cards)
        {
            foreach (Card card in cards)
            {
                Card tempCard = compareCards.FirstOrDefault(t => t.color == card.color && t.point == card.point);
                if (tempCard == default(Card))
                    return false;
            }

            return true;
        }
    }
}
