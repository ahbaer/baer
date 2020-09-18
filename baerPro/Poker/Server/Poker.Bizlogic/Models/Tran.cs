using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Bizlogic.Models
{
    public class Tran
    {
        /// <summary>
        /// 1：弹窗提示
        /// 2：当选地主提示
        /// 3：出牌（第一次，给与玩家牌）
        /// 4：出牌（非第一次，给与上一次玩家牌）
        /// </summary>
        public int type { get; set; }
        public string msg { get; set; }
        public bool landlord { get; set; }//只用于client向server传输数据，返过来不传输
        public int indexOpt { get; set; } = 0;//0表示当前玩家 1表示当前玩家下家 2表示当前玩家上家
        public List<Card> cards { get; set; }
        public List<Card> landlordCards { get; set; }//只用于client向server传输数据，返过来不传输

        public Tran()
        {
            cards = new List<Card>();
            landlordCards = new List<Card>();
        }
    }
}
