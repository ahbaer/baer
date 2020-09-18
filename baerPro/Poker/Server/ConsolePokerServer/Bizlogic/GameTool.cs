using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using Poker.Bizlogic.Models;

namespace ConsolePokerServer.Bizlogic
{
    public class GameTool
    {
        //轮询指针
        public static int Polling = 0;
        //当选地主拒绝次数
        public static int RefuseNum = 0;
        //游戏玩家
        public static List<Gamer> Gamers = new List<Gamer>();
        //地主牌
        public static List<Card> LandlordCards = new List<Card>();

        /// <summary>
        /// 游戏开始
        /// </summary>
        public static void GameStart()
        {
            Tran tran = new Tran();

            foreach (Gamer gamer in Gamers)
            {
                //创建一个通信线程      
                ParameterizedThreadStart pts = new ParameterizedThreadStart(SocketTool.Recv);
                Thread thread = new Thread(pts);
                //设置为后台线程，随着主线程退出而退出 
                thread.IsBackground = true;
                //启动线程     
                thread.Start(gamer.socket);
            }

            InitPoker();

            //随机确认一名玩家作为候选地主
            Polling = new Random((int)DateTime.Now.Ticks).Next(0, 3);
            Polling = 2;//todo

            tran = new Tran()
            {
                type = 2,
                msg = "请确认是否当选地主！\r\n"
            };
            Gamers[Polling].socket.Send(Poker.Bizlogic.Common.GetSendBytes(tran));
        }

        public static void InitPoker()
        {
            List<Card> tempcards = new List<Card>();//所有牌
            LandlordCards = new List<Card>();//地主牌

            for (int point = 3; point < 16; point++)
                for(int color = 0; color < 4; color++)
                {
                    tempcards.Add(new Card(color, point));
                }

            tempcards.Add(new Card(0, 16));//大王
            tempcards.Add(new Card(1, 16));//小王

            for(int gamer = 0; gamer < 3; gamer++)
            {
                List<Card> gamerCards = new List<Card>();
                for (int poker = 0; poker < 17; poker++)
                {
                    byte[] buffer = Guid.NewGuid().ToByteArray();//生成字节数组
                    int iRoot = BitConverter.ToInt32(buffer, 0);//利用BitConvert方法把字节数组转换为整数
                    int random = new Random(iRoot).Next(0, tempcards.Count);
                    gamerCards.Add(tempcards[random]);
                    tempcards.RemoveAt(random);
                }

                gamerCards.Sort((x, y) => x.CompareTo(y));
                Gamers[gamer].cards = gamerCards;
            }

            for(int landlordCard = 0; landlordCard < 3; landlordCard++)
            {
                LandlordCards.Add(tempcards[landlordCard]);
            }
            LandlordCards.Sort((x, y) => x.CompareTo(y));

            foreach (Gamer gamer in Gamers)
            {
                Tran tran = new Tran()
                {
                    type = 1,
                    cards = gamer.cards,
                    landlordCards = LandlordCards
                };

                gamer.socket.Send(Poker.Bizlogic.Common.GetSendBytes(tran));
            }
        }

        public static void GameLogic(string rec)
        {
            Tran tran = JsonConvert.DeserializeObject<Tran>(rec.Trim('$'));
            switch (tran.type)
            {
                case 2:
                    PollingLandlord(tran);
                    break;
                case 4:
                    SendPoker(tran);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 轮询地主阶段
        /// </summary>
        /// <param name="rec"></param>
        public static void PollingLandlord(Tran tran)
        {
            Tran f_Tran = new Tran();
            Tran l_Tran = new Tran();
            if (tran.landlord || RefuseNum > 7)//同意地主 or 已过三轮，直接默认当前玩家为地址
            {
                if (!tran.landlord)//如果不是同意当选地主的，是下一位玩家为地主
                {
                    Polling = (Polling + 1) % 3;
                }

                //确认地主及给与地主牌
                Gamers[Polling].landlord = true;
                foreach (Card card in LandlordCards)
                {
                    Gamers[Polling].cards.Add(card);
                }

                //整理排序
                Gamers[Polling].cards.Sort((x, y) => x.CompareTo(y));

                f_Tran = new Tran()
                {
                    type = 1,
                    msg ="玩家[" + Common.GetRemotePoat(Polling) + "]当选地主\r\n"
                };
                l_Tran = new Tran()
                {
                    type = 3,
                    msg = "请出牌！\r\n",
                    cards = Gamers[Polling].cards
                };
                Common.SendAllGamer(f_Tran, l_Tran);
            }
            else
            {
                if((RefuseNum + 1)% 3 == 0)//一轮循环已结束，需要重新洗牌
                {
                    InitPoker();
                }

                Polling = (Polling + 1) % 3;
                RefuseNum++;

                //不当选地主
                l_Tran = new Tran()
                {
                    type = 2,
                    msg = "请确认是否当选地主！\r\n",
                };
                
                Gamers[Polling].socket.Send(Poker.Bizlogic.Common.GetSendBytes(l_Tran));
            }
        }

        /// <summary>
        /// 出牌阶段
        /// </summary>
        public static void SendPoker(Tran tran)
        {
            Tran b_Tran = new Tran();

            //当前轮询者
            Polling = (Polling + 1) % 3;
            b_Tran = new Tran()
            {
                type = 4,
                cards = tran.cards
            };
            Gamers[Polling].socket.Send(Poker.Bizlogic.Common.GetSendBytes(b_Tran));

            //下一轮轮询者
            b_Tran = new Tran()
            {
                type = 4,
                indexOpt = 1,
                cards = tran.cards
            };
            Gamers[(Polling + 1) % 3].socket.Send(Poker.Bizlogic.Common.GetSendBytes(b_Tran));
        }
    }
}