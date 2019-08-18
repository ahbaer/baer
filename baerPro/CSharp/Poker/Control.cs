using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Poker.Enumeration;

namespace Poker
{
    public class Control
    {
        public static string gameGuid;
        public static List<Player> players;//玩家队列
        public static int landload;//地主
        public static int cursor;//游标
        public static string printf = null;
        public static string scanf = null;

        /// <summary>
        /// 玩家进入
        /// add by baer
        /// </summary>
        public void playerJoin()
        {
            players = new List<Player>();

            players.Add(new Player("张三", Guid.NewGuid().ToString()));
            players.Add(new Player("李四", Guid.NewGuid().ToString()));
            players.Add(new Player("王五", Guid.NewGuid().ToString()));

            //玩家已凑齐，开始发牌
            initCard();
        }

        /// <summary>
        /// 初始化游戏
        /// 1、生产54张牌
        /// 2、发牌
        /// add by baer
        /// </summary>
        public void initCard()
        {
            //获取本局游戏唯一标志
            gameGuid = Guid.NewGuid().ToString();

            //游戏信息入库
            BaersTool.DB.Data data = new BaersTool.DB.Data("GameInfo");
            data["GameGuid"] = gameGuid;
            data.Insert();

            //玩家信息入库
            foreach (Player player in players)
            {
                data = new BaersTool.DB.Data("GameAndPlayer");
                data["GameGuid"] = gameGuid;
                data["PlayerGuid"] = player.PlayerGuid;
                data.Insert();
            }

            //生产54张牌，并插入数据库
            BaersTool.DB.Data db;
            for (int no = 1; no < 55; no++)
            {
                int point, color;
                getPointAndColor(no, out point, out color);
                db = new BaersTool.DB.Data("GameCards");
                db["No"] = no;
                db["Point"] = point;
                db["Color"] = color;
                db["GameGuid"] = gameGuid;
                db.Insert();
            }

            #region 发牌
            string strSql = "select * from GameCards where GameGuid = '" + gameGuid + "' order by RowGuid";
            DataView dvCards = BaersTool.DB.Data.ExecuteToDataView(strSql);
            for (int i = 0; i < dvCards.Count; i++)
            {
                db = new BaersTool.DB.Data("GameCards", Convert.ToString(dvCards[i]["RowGuid"]));
                if (i < 17)
                {
                    db["PlayerGuid"] = players[0].PlayerGuid;
                    db["CardType"] = (int)GameCardType.玩家;
                }
                else if (i < 34)
                {
                    db["PlayerGuid"] = players[1].PlayerGuid;
                    db["CardType"] = (int)GameCardType.玩家;
                }
                else if (i < 51)
                {
                    db["PlayerGuid"] = players[2].PlayerGuid;
                    db["CardType"] = (int)GameCardType.玩家;
                }
                else
                {
                    db["CardType"] = (int)GameCardType.地主;
                }
                db.Update();
            }
            #endregion

            printCards();

            //轮询获取地主
            getLandload(0);
        }

        public void getLandload(int count)
        {
            if (count < 3)
            {
                //第一次随机获取玩家作为地主
                if (count == 0)
                {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    cursor = rand.Next(0, 3);
                }

                printf = players[cursor].PlayerName + "请选择是否当选地主：\r\n\t0 否\t1 是";
                Console.WriteLine(printf);
                scanf = Console.ReadLine();

                if (scanf == "0")
                {
                    cursor = (++cursor) % 3;
                    getLandload(++count);
                }
                else
                {
                    //分配地主牌
                    sendLandlordCards();

                    Console.Clear();
                    printCards();
                    printf = players[cursor].PlayerName + "为本轮地主";
                    Console.WriteLine(printf);

                    //玩家出牌
                    outCard();

                    //获取赢家，游戏结束
                    getWinner();
                }
            }
            else
            {
                Console.Clear();
                printf = "本轮游戏无人当选地主，重新发牌！";
                Console.WriteLine(printf);

                initCard();
            }
        }

        /// <summary>
        /// 出牌
        /// </summary>
        public void outCard()
        {
            printf = players[cursor].PlayerName + "请出牌：";
            Console.WriteLine(printf);

            do
            {
                foreach (Player player in players)
                {
                    string strSql = "select * from GameCards where GameGuid = '" + gameGuid + "' and PlayerGuid = '" + player.PlayerGuid + "' and isnull(IsOut, '0') <> '1'";
                    player.Cards = BaersTool.DB.Data.ExecuteToDataView(strSql);
                }

                scanf = Console.ReadLine();

                if (!checkSendCard(null, scanf))
                {
                    printf = players[cursor].PlayerName + "本轮出牌：" + scanf + "不符合出牌规则，请重新出牌";
                    Console.WriteLine(printf);
                }
                else
                {
                    doOutCard(scanf);

                    Console.Clear();
                    printCards();

                    cursor = (++cursor) % 3;
                }
            }
            while (isGameOver(players[0].Cards, players[1].Cards, players[2].Cards));
        }

        public void getWinner()
        {
            if((cursor - 1) % 3 == landload)
            {
                printf = "本局游戏赢家为：" + players[landload].PlayerName;
            }
            else
            {
                if(cursor == landload)
                {

                }
                printf = "本局游戏赢家为：" + players[(landload - 1) % 3].PlayerName + players[(landload + 1) % 3].PlayerName;
            }

            Console.WriteLine(printf);
        }

        #region 数据操作
        /// <summary>
        /// 给地主发地主牌
        /// </summary>
        private void sendLandlordCards()
        {
            string strSql = "update GameCards set PlayerGuid = '" + players[cursor].PlayerGuid + "' where isnull(CardType, '') = '1'";
            BaersTool.DB.Data.ExecuteNonQuery(strSql);
        }

        /// <summary>
        /// 更新出牌状态为已出牌
        /// add by baer
        /// </summary>
        /// <param name="cards"></param>
        private void doOutCard(string cards)
        {
            cards = cards.Replace("/", "','");

            string strSql = "update GameCards set IsOut = 1 where PlayerGuid = '" + gameGuid + "' and No in ('" + cards + "')";
            BaersTool.DB.Data.ExecuteNonQuery(strSql);
        }
        #endregion

        #region 游戏展示及操作
        /// <summary>
        /// 展示所有纸牌
        /// add by baer
        /// </summary>
        public void printCards()
        {
            printf = null;
            for (int i = 0; i < 3; i++)
            {
                printf += (players[i].PlayerName + "：" + printCards(players[i].PlayerGuid, GameCardType.玩家) + "\r\n");
            }

            printf += "【地主】：" + printCards("", GameCardType.地主);
            Console.WriteLine(printf);
        }

        public string printCards(string Player, GameCardType type)
        {
            string strRet = null;
            string strSql = null;
            if (type == GameCardType.玩家)
            {
                strSql = "select * from GameCards where GameGuid = '" + gameGuid + "' and isnull(PlayerGuid,'') = '" + Player + "' and isnull(IsOut, '') <> '1' order by Point desc,Color asc";
            }
            else
            {
                strSql = "select * from GameCards where GameGuid = '" + gameGuid + "' and CardType = '" + (int)type + "' and isnull(IsOut, '') <> '1' order by Point desc,Color asc";
            }
            DataView dvCards = BaersTool.DB.Data.ExecuteToDataView(strSql);
            foreach (DataRowView drv in dvCards)
            {
                if (Convert.ToInt16(drv["Point"]) == 15)
                {
                    strRet += ((ColorType)Convert.ToInt16(drv["Color"]) + "2 ");
                }
                else
                {
                    if (Convert.ToInt16(drv["Color"]) == -1)
                    {
                        strRet += ((CardType)Convert.ToInt16(drv["Point"]) + " ");
                    }
                    else
                    {
                        strRet += ((ColorType)Convert.ToInt16(drv["Color"]) + "" + (CardType)Convert.ToInt16(drv["Point"]) + " ");
                    }

                }
            }

            return strRet;
        }

        /// <summary>
        /// 根据纸牌No获取点数及花色
        /// add by baer
        /// </summary>
        /// <param name="card"></param>
        /// <param name="point"></param>
        /// <param name="color"></param>
        public static void getPointAndColor(int card, out int point, out int color)
        {
            point = card;
            color = -1;
            if (card < 53)
            {
                point = card % 13;

                if (point < 3)
                {
                    point += 13;
                }
            }

            color = (card < 53) ? ((card - 1) / 13) : -1;
        }

        /// <summary>
        /// 判断游戏是否结束
        /// 三位玩家任意一家走完所有牌即可认为游戏结束
        /// add by baer
        /// </summary>
        /// <param name="PlayerA"></param>
        /// <param name="PlayerB"></param>
        /// <param name="PlayerC"></param>
        /// <returns></returns>
        public bool isGameOver(DataView PlayerA, DataView PlayerB, DataView PlayerC)
        {
            return PlayerA.Count == 0 || PlayerB.Count == 0 || PlayerC.Count == 0;
        }
        #endregion

        #region 游戏逻辑
        /// <summary>
        /// 判断出牌是否符合规则
        /// 按照出牌数量判断
        /// </summary>
        /// <param name="sendPoker"></param>
        /// <returns></returns>
        public bool checkSendCard(string oldSendCards, string newSendCards)
        {
            //todo
            return true;

            //if (string.IsNullOrEmpty(oldSendCards))
            //{
            //    return checkOwnerSend(newSendCards.Split('/'));
            //}
            //else
            //{
            //    string[] oldPoker = oldSendCards.Split('/');
            //    string[] newPoker = oldSendCards.Split('/');

            //    switch (oldPoker.Length)
            //    {
            //        case 1:
            //            //上家出一张牌
            //            return checkOnePoker(oldSendCards, newSendCards);
            //        default:
            //            return true;
            //    }
            //}
        }

        ///// <summary>
        ///// 玩家自己出牌
        ///// add by baer
        ///// </summary>
        ///// <param name="poker"></param>
        ///// <returns></returns>
        //public bool checkOwnerSend(string[] card)
        //{
        //    List<int> cards = new List<int>();
        //    foreach (string item in card)
        //    {
        //        int point, color;
        //        getPointAndColor(Convert.ToInt16(item), out point, out color);
        //        cards.Add(point);
        //    }

        //    return CheckSendWithCardNum(cards, cards.Count);
        //}

        //public bool CheckSendWithCardNum(List<int> cards, int num)
        //{
        //    switch (cards.Count)
        //    {
        //        case 1:
        //            //单只
        //            return true;
        //        case 2:
        //            //对子或者王炸
        //            return IsDouble(cards) || IsKingBoom(cards);
        //        case 3:
        //            //三不带
        //            return IsThreeNoWith(cards);
        //        case 4:
        //            //三带一或者炸弹
        //            return IsThreeWithOne(cards) || IsBoom(cards);
        //        case 5:
        //            //三带二或者顺子
        //            return IsThreeWithTwo(cards) || IsSingleList_Five(cards);
        //        case 6:
        //            //顺子或者飞机或者四带二
        //            return true;
        //        case 7:
        //            //顺子
        //            return true;
        //        case 8:
        //            //顺子或者飞机或者四带两对
        //            return true;
        //        case 9:
        //            //顺子或者飞机
        //            return true;
        //        case 10:
        //            //顺子或者飞机
        //            return true;
        //        case 11:
        //            //顺子
        //            return true;
        //        case 12:
        //            //顺子或者飞机
        //            return true;
        //        case 13:
        //            //顺子
        //            return true;
        //        case 14:
        //            //顺子或者飞机或者四带二
        //            return true;
        //        case 15:
        //            //顺子或者飞机
        //            return true;
        //        case 16:
        //            //顺子或者飞机
        //            return true;
        //        default:
        //            return false;
        //    }
        //}

        ///// <summary>
        ///// 上家出一张牌时，当前玩家出牌校验
        ///// </summary>
        ///// <param name="oldPoker"></param>
        ///// <param name="newPoker"></param>
        ///// <returns></returns>
        //public static bool checkOnePoker(string oldPoker, string newPoker)
        //{
        //    int po = Convert.ToInt16(oldPoker) % 13;
        //    int[] pn = new int[newPoker.Length];

        //    string[] sendPoker = newPoker.Split('0');
        //    for (int i = 0; i < newPoker.Length; i++)
        //    {
        //        if (Convert.ToInt16(newPoker[i]) == 52 || Convert.ToInt16(newPoker[i]) == 53)
        //        {
        //            pn[i] = Convert.ToInt16(newPoker[i]);
        //        }
        //        else
        //        {
        //            pn[i] = Convert.ToInt16(newPoker[i]) % 13;
        //        }
        //    }

        //    switch (sendPoker.Length)
        //    {
        //        case 1:
        //            if (pn[0] > po)
        //                return true;
        //            else
        //                return false;
        //        case 2:
        //            if (pn[0] == 52 && pn[1] == 53)
        //                return true;
        //            else
        //                return false;
        //        case 4:
        //            for (int i = 1; i < 3; i++)
        //            {
        //                if (pn[0] != pn[i])
        //                    return false;
        //            }

        //            return true;
        //        default:
        //            return false;
        //    }
        //}

        //#region 出牌规则
        //#region 对子
        ///// <summary>
        ///// 对子
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsDouble(List<int> cards)
        //{
        //    return cards[0] == cards[1];
        //}
        //#endregion

        //#region 三张
        ///// <summary>
        ///// 三不带
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsThreeNoWith(List<int> cards)
        //{
        //    return cards[0] == cards[1] && cards[1] == cards[2];
        //}

        ///// <summary>
        ///// 三带一
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsThreeWithOne(List<int> cards)
        //{
        //    if(cards.Count == 4)
        //    {
        //        return (IsThreeNoWith(Remove(cards, 3)) && cards[2] != cards[3]) ||
        //        (IsThreeNoWith(Remove(cards, 0)) && cards[0] != cards[1]);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 三带二
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsThreeWithTwo(List<int> cards)
        //{
        //    if (cards.Count == 5)
        //    {
        //        return (IsThreeWithOne(Remove(cards, 4)) && cards[3] == cards[4]) ||
        //        (IsThreeWithOne(Remove(cards, 0)) && cards[0] == cards[1]);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion

        //#region 四张

        //#endregion

        ////todo
        //#region 飞机
        ///// <summary>
        ///// 飞机(双飞不带)
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsAirplan(List<int> cards)
        //{
        //    if (cards.Count == 6)
        //    {
        //        return cards[2] == cards[3] && IsThreeNoWith(Remove(cards, 3, 3)) && IsThreeNoWith(Remove(cards, 0, 3));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 飞机(双飞单带)
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsAirplanWithOne(List<int> cards)
        //{
        //    if (cards.Count == 8)
        //    {
        //        return IsAirplan(Remove(cards, 6, 2)) || IsAirplan(Remove(cards, 0, 2)) || IsAirplan(Remove(Remove(cards, 0), 6));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 飞机(双飞双带)
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsAirplanWithTwo(List<int> cards)
        //{
        //    if (cards.Count == 10)
        //    {
        //        return IsAirplan(Remove(cards, 6, 4)) || IsAirplan(Remove(cards, 0, 4)) || IsAirplan(Remove(Remove(cards, 0, 2), 6, 2));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 三飞不带
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsAirplanThreeNoWith(List<int> cards)
        //{
        //    if (cards.Count == 9)
        //    {
        //        return IsAirplan(Remove(cards, 6, 3)) && IsAirplan(Remove(cards, 0, 3));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion

        //#region 顺子
        //public static bool IsSingleList_Five(List<int> cards)
        //{
        //    if (cards.Count == 5)
        //    {
        //        for (int i = 1; i < cards.Count; i++)
        //        {
        //            if (cards[i] + 1 != cards[i - 1])
        //            {
        //                return false;
        //            }
        //        }

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSingleList_Six(List<int> cards)
        //{
        //    if (cards.Count == 6)
        //    {
        //        return cards[0] == cards[1] + 1 && IsSingleList_Five(Remove(cards, 0));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSingleList_Seven(List<int> cards)
        //{
        //    if (cards.Count == 7)
        //    {
        //        return cards[0] == cards[1] + 1 && IsSingleList_Six(Remove(cards, 0));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSingleList_Eight(List<int> cards)
        //{
        //    if (cards.Count == 8)
        //    {
        //        return cards[0] == cards[1] + 1 && IsSingleList_Seven(Remove(cards, 0));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSingleList_Nine(List<int> cards)
        //{
        //    if (cards.Count == 9)
        //    {
        //        return cards[0] == cards[1] + 1 && IsSingleList_Eight(Remove(cards, 0));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSingleList_Ten(List<int> cards)
        //{
        //    if (cards.Count == 10)
        //    {
        //        return cards[0] == cards[1] + 1 && IsSingleList_Nine(Remove(cards, 0));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSingleList_Eleven(List<int> cards)
        //{
        //    if (cards.Count == 11)
        //    {
        //        return cards[0] == cards[1] + 1 && IsSingleList_Ten(Remove(cards, 0));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSingleList_Twelve(List<int> cards)
        //{
        //    if (cards.Count == 12)
        //    {
        //        return cards[0] == cards[1] + 1 && IsSingleList_Eleven(Remove(cards, 0));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSingleList_Thirteen(List<int> cards)
        //{
        //    if (cards.Count == 13)
        //    {
        //        return cards[0] == cards[1] + 1 && IsSingleList_Twelve(Remove(cards, 0));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsSingleList_Fourteen(List<int> cards)
        //{
        //    if (cards.Count == 14)
        //    {
        //        return cards[0] == cards[1] + 1 && IsSingleList_Thirteen(Remove(cards, 0));
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsDoubleList_Three(List<int> cards)
        //{
        //    if(cards.Count == 6)
        //    {
        //        for(int i = 2; i < 6; i += 2)
        //        {
        //            if(!IsDouble(GetList(cards, i - 2, 2)))
        //            {
        //                return false;
        //            }

        //            if(cards[i] + 1 != cards[i - 1])
        //            {
        //                return false;
        //            }
        //        }

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsDoubleList_Four(List<int> cards)
        //{
        //    if (cards.Count == 8)
        //    {
        //        if(cards[0] == cards[1] && cards[1] == (cards[2] + 1) && IsDoubleList_Three(Remove(cards, 0, 2)))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsDoubleList_Five(List<int> cards)
        //{
        //    if (cards.Count == 10)
        //    {
        //        if (cards[0] == cards[1] && cards[1] == (cards[2] + 1) && IsDoubleList_Four(Remove(cards, 0, 2)))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsDoubleList_Six(List<int> cards)
        //{
        //    if (cards.Count == 12)
        //    {
        //        if (cards[0] == cards[1] && cards[1] == (cards[2] + 1) && IsDoubleList_Five(Remove(cards, 0, 2)))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsDoubleList_Seven(List<int> cards)
        //{
        //    if (cards.Count == 14)
        //    {
        //        if (cards[0] == cards[1] && cards[1] == (cards[2] + 1) && IsDoubleList_Six(Remove(cards, 0, 2)))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool IsDoubleList_Eight(List<int> cards)
        //{
        //    if (cards.Count == 14)
        //    {
        //        if (cards[0] == cards[1] && cards[1] == (cards[2] + 1) && IsDoubleList_Seven(Remove(cards, 0, 2)))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion

        //#region 炸弹
        ///// <summary>
        ///// 炸弹
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsBoom(List<int> cards)
        //{
        //    if(cards.Count == 4)
        //    {
        //        return cards[0] == cards[1] && cards[1] == cards[2] && cards[2] == cards[3];
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion

        //#region 王炸
        ///// <summary>
        ///// 王炸
        ///// add by baer
        ///// </summary>
        ///// <param name="cards"></param>
        ///// <returns></returns>
        //public static bool IsKingBoom(List<int> cards)
        //{
        //    if(cards.Count == 2)
        //    {
        //        return cards[0] == 54 && cards[1] == 53;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion
        //#endregion
        #endregion
    }
}