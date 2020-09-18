using Poker.Bizlogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Poker.Bizlogic
{
    public class GameTool
    {
        public const string COTINUOUS = "3456789XJQKA";
        public const string DOUBLECOTINUOUS = "33445566778899XXJJQQKKAA";
        public const string TRIPLECOTINUOUS = "333444555666777888999XXXJJJQQQKKKAAA";

        public static bool IsRuleCards(List<Card> nowCards, List<Card> lastCards, List<Card> gamerCards)
        {
            nowCards.Sort((x, y) => y.CompareTo(x));
            lastCards.Sort((x, y) => y.CompareTo(x));
            gamerCards.Sort((x, y) => y.CompareTo(x));

            bool isRule = false, isFirst = lastCards.Count == 0;
            string strCards = Poker.Bizlogic.Common.ListToString(nowCards);

            if (!Common.IsContains(gamerCards, nowCards))
            {
                return false;
            }

            isRule = IsBigger(nowCards, lastCards);
            return isRule;
        }

        private static bool IsBigger(List<Card> nowCards, List<Card> lastCards)
        {
            bool isBigger = false, isJoker = false;
            string strCards = Common.ListToString(nowCards);
            string strLastCarrds = Common.ListToString(lastCards);
            if (IsBoom(strCards, out isJoker))//本次出牌是否是炸弹
            {
                #region 炸弹
                if (isJoker)//王炸大过一切
                {
                    isBigger = true;
                }
                else
                {
                    if (IsBoom(strLastCarrds, out isJoker))//上一轮出牌是否是炸弹
                    {
                        if (!isJoker)
                        {
                            isBigger = nowCards[0].point > lastCards[0].point;
                        }
                    }
                    else
                    {
                        isBigger = true;
                    }
                }
                #endregion
            }
            else
            {
                #region 普通比较大小
                if (nowCards.Count == lastCards.Count || lastCards.Count == 0)
                {
                    int index = -1, lastIndex = -1;
                    switch (lastCards.Count)
                    {
                        case 0:
                            isBigger = FirstSendCard(strCards);
                            break;
                        case 1:
                            isBigger = nowCards[0].point > lastCards[0].point;
                            break;
                        case 2:
                            if (nowCards[0].point == nowCards[1].point)
                            {
                                isBigger = nowCards[0].point > lastCards[0].point;
                            }
                            break;
                        case 3:
                            if (TRIPLECOTINUOUS.Contains(strCards))
                            {
                                isBigger = nowCards[0].point > lastCards[0].point;
                            }
                            break;
                        case 4:
                            if (IsQuadraAdd(strCards, out index))
                            {
                                isBigger = nowCards[index].point > lastCards[index].point;
                            }
                            break;
                        case 5:
                            if ((COTINUOUS.Contains(strCards) && COTINUOUS.Contains(strLastCarrds))
                                || (IsTripleCotinuous(strCards) && IsTripleCotinuous(strLastCarrds)))
                            {
                                isBigger = nowCards[2].point > lastCards[2].point;
                            }
                            break;
                        case 6:
                            if ((COTINUOUS.Contains(strCards) && COTINUOUS.Contains(strLastCarrds))
                                || (DOUBLECOTINUOUS.Contains(strCards) && DOUBLECOTINUOUS.Contains(strLastCarrds))
                                || TRIPLECOTINUOUS.Contains(strCards) && TRIPLECOTINUOUS.Contains(strLastCarrds))
                            {
                                isBigger = nowCards[0].point > lastCards[0].point;
                            }
                            else if (IsQuadraAdd(strCards, out index) && IsQuadraAdd(strCards, out lastIndex))
                            {
                                isBigger = nowCards[index].point > lastCards[lastIndex].point;
                            }
                            break;
                        case 7:
                        case 11:
                            if (COTINUOUS.Contains(strCards) && COTINUOUS.Contains(strLastCarrds))
                            {
                                isBigger = nowCards[0].point > lastCards[0].point;
                            }
                            break;
                        case 8:
                            if((COTINUOUS.Contains(strCards) && COTINUOUS.Contains(strLastCarrds))
                                || (DOUBLECOTINUOUS.Contains(strCards) && DOUBLECOTINUOUS.Contains(strLastCarrds))
                                || (IsTripleCotinuous(strCards) && IsTripleCotinuous(strLastCarrds)))
                            {
                                isBigger = nowCards[2].point > lastCards[2].point;
                            }
                            else if(IsQuadraAdd(strCards, out index) && IsQuadraAdd(strCards, out lastIndex))
                            {
                                isBigger = nowCards[index].point > lastCards[lastIndex].point;
                            }
                            break;
                        case 9:
                            if((COTINUOUS.Contains(strCards) && COTINUOUS.Contains(strLastCarrds))
                                || (TRIPLECOTINUOUS.Contains(strCards) && TRIPLECOTINUOUS.Contains(strLastCarrds)))
                            {
                                isBigger = nowCards[0].point > lastCards[0].point;
                            }
                            break;
                        case 10:
                            if((COTINUOUS.Contains(strCards) && COTINUOUS.Contains(strLastCarrds))
                                || (DOUBLECOTINUOUS.Contains(strCards) && DOUBLECOTINUOUS.Contains(strLastCarrds)))
                            {
                                isBigger = nowCards[0].point > lastCards[0].point;
                            }
                            else if(IsTripleCotinuous(strCards) && IsTripleCotinuous(strLastCarrds))
                            {
                                isBigger = nowCards[4].point > lastCards[4].point;
                            }
                            break;
                        case 12:
                            if((COTINUOUS.Contains(strCards) && COTINUOUS.Contains(strLastCarrds))
                                || (DOUBLECOTINUOUS.Contains(strCards) && DOUBLECOTINUOUS.Contains(strLastCarrds)))
                            {
                                isBigger = nowCards[0].point > lastCards[0].point;
                            }
                            else if(IsTripleCotinuous(strCards) && IsTripleCotinuous(strLastCarrds))
                            {
                                isBigger = nowCards[3].point > lastCards[3].point;
                            }
                            break;
                        case 14:
                            if (DOUBLECOTINUOUS.Contains(strCards))
                            {
                                isBigger = nowCards[0].point > lastCards[0].point;
                            }
                            break;
                        case 15:
                            if(IsTripleCotinuous(strCards) && IsTripleCotinuous(strLastCarrds))
                            {
                                isBigger = nowCards[6].point > lastCards[6].point;
                            }
                            break;
                        case 16:
                        case 18:
                        case 20:
                            if (DOUBLECOTINUOUS.Contains(strCards) && DOUBLECOTINUOUS.Contains(strLastCarrds))
                            {
                                isBigger = nowCards[0].point > lastCards[0].point;
                            }
                            else if(IsTripleCotinuous(strCards) && IsTripleCotinuous(strLastCarrds))
                            {
                                isBigger = nowCards[8].point > lastCards[8].point;
                            }
                            break;
                    }
                }
                #endregion
            }

            return isBigger;
        }

        private static bool IsDouble(string cards)
        {
            bool isDouble = false;
            if(cards.Length == 2)
            {
                isDouble = cards.Substring(0, 1) == cards.Substring(1, 1);
            }

            return isDouble;
        }

        /// <summary>
        /// 三联
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static bool IsTripleCotinuous(string cards)
        {
            bool isCOTINUOUS = false;

            int num = 0;
            switch (cards.Length)
            {
                case 3:
                    num = 1;
                    isCOTINUOUS = TRIPLECOTINUOUS.Contains(cards);
                    break;
                case 4:
                    num = 1;
                    isCOTINUOUS = TRIPLECOTINUOUS.Contains(cards.Substring(0, 3)) || TRIPLECOTINUOUS.Contains(cards.Substring(1, 3));
                    break;
                case 5:
                    num = 1;
                    Match match = new Regex(@"([2-9JQKA])\1{3}").Match(cards);
                    if (match != null)
                    {
                        int index = cards.IndexOf(match.Value);
                        isCOTINUOUS = index == 0 ? DOUBLECOTINUOUS.Contains(cards.Substring(3, 2)) : (index == 2 ? DOUBLECOTINUOUS.Contains(cards.Substring(0, 2)) : false);
                    }
                    break;
                case 6:
                case 8:
                case 10:
                    num = 2;
                    break;
                case 9:
                case 15:
                    num = 3;
                    break;
                case 12:
                    num = TRIPLECOTINUOUS.Contains(cards) ? 4 : 3;//可能是333444555666 or 333444555678
                    break;
                case 16:
                case 20:
                    num = 4;
                    break;
                default:
                    break;
            }

            if (num > 1)
            {
                for (int i = 0; i < num; i++)
                {
                    if (cards.Substring(i, 1) == cards.Substring(i + 2, 1) && cards.Substring(i, 1) == (Convert.ToInt32(cards.Substring(i + 3, 1)) - 1).ToString())
                    {
                        isCOTINUOUS = TRIPLECOTINUOUS.Contains(cards.Substring(i, num * 3));
                        break;
                    }
                }
            }

            return isCOTINUOUS;
        }

        /// <summary>
        /// 四带一 or 四带一对
        /// </summary>
        private static bool IsQuadraAdd(string cards, out int index)
        {
            bool isQuadra = false;
            index = -1;
            Match match = new Regex(@"([2-9JQKA])\1{3}").Match(cards);
            if (match != null && (cards.Length == 4 || cards.Length == 6 || cards.Length == 8))
            {
                if (cards.Length == 8)
                {
                    index = cards.IndexOf(match.Value);

                    switch (index)
                    {
                        case 0:
                            isQuadra = DOUBLECOTINUOUS.Contains(cards.Substring(4, 2)) && DOUBLECOTINUOUS.Contains(cards.Substring(6, 2));
                            break;
                        case 2:
                            isQuadra = DOUBLECOTINUOUS.Contains(cards.Substring(0, 2)) && DOUBLECOTINUOUS.Contains(cards.Substring(6, 2));
                            break;
                        case 4:
                            isQuadra = DOUBLECOTINUOUS.Contains(cards.Substring(0, 2)) && DOUBLECOTINUOUS.Contains(cards.Substring(2, 2));
                            break;
                    }
                }
            }

            return isQuadra;
        }

        private static bool IsBoom(string cards, out bool isJoker)
        {
            bool isBoom = false;
            isJoker = false;
            if (cards.Length == 2 || cards.Length == 10)
            {
                isJoker = isBoom = cards == "WW";
            }
            else if (cards.Length == 4)
            {
                isBoom = new Regex(@"^([2-9XJQKA])\1{3}$").IsMatch(cards);
            }

            return isBoom;
        }

        private static bool FirstSendCard(string strCards)
        {
            bool isJoker = false;
            int index = -1;

            //bool canSend = false;
            //if (!canSend)
            //{
            //    canSend = strCards.Length == 1;
            //}
            //if (!canSend)
            //{
            //    canSend = DOUBLECOTINUOUS.Contains(strCards) || strCards == "22";
            //}
            //if (!canSend)
            //{
            //    canSend = COTINUOUS.Contains(strCards);
            //}
            //if (!canSend)
            //{
            //    canSend = TRIPLECOTINUOUS.Contains(strCards) || IsTripleCotinuous(strCards);
            //}
            //if (!canSend)
            //{
            //    canSend = IsQuadraAdd(strCards, out index);
            //}
            //if (!canSend)
            //{
            //    canSend = IsBoom(strCards, out isJoker);
            //}

            bool canSend = strCards.Length == 1//单张
                || IsDouble(strCards) //对子
                || (strCards.Length > 5 && DOUBLECOTINUOUS.Contains(strCards))//包括大连
                || (strCards.Length > 4 && COTINUOUS.Contains(strCards))//顺子
                || (strCards.Length > 5 && TRIPLECOTINUOUS.Contains(strCards) || IsTripleCotinuous(strCards))//三飞
                || IsQuadraAdd(strCards, out index)//四带一/二对
                || IsBoom(strCards, out isJoker);
            return canSend;
        }
    }
}
