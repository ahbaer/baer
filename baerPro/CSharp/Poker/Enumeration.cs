using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Enumeration
    {
        public enum IsOut
        {
            已出牌 = 1,
            未出牌 = 0
        }

        public enum GameCardType
        {
            玩家 = 0,
            地主 = 1
        }

        public enum ColorType
        {
            红桃 = 0,
            方块 = 1,
            黑桃 = 2,
            梅花 = 3
        }

        public enum CardType
        {
            J = 11,
            Q = 12,
            K = 13,
            A = 14,
            小王 = 53,
            大王 = 54
        }
    }
}
