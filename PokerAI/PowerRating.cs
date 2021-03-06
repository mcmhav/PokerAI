﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerAI
{
    class PowerRating
    {
        public int first {get; private set;}
        public int second {get; private set;}
        public int third {get; private set;}
        public int fourth {get; private set;}
        public int fift {get; private set;}
        public int sixth {get; private set;}


        public PowerRating(List<Card> hand)
        {
            if (hand.Count == 2)
            {
                if(hand[0].Value == hand[1].Value)
                {
                    first = (hand[0].Value / 3) * 2;
                }
                else if (hand[0].Suit == hand[1].Suit)
                {
                    first = 6;
                }
                else
                {
                    first = (hand[0].Value / 2);
                }
                return;
            }
            hand = hand.OrderByDescending(c => c.Value).ToList();
            if (flushORstraightFlush(hand)) return;

            var ValueGroup = hand.GroupBy(c => c.Value).Select(g => new { value = g.Key, count = g.Count(), cards = g }).ToList();
            var valueGroupByCount = ValueGroup.OrderByDescending(vc => vc.count).ToList();
            int highestQuantity = valueGroupByCount.First().count;
            int highestQuantityValue = valueGroupByCount.First().value;

            if (highestQuantity == 4)
            {
                #region 4 of a kind
                first = 7;
                second = highestQuantityValue;
                third = ValueGroup[0].value == second ? ValueGroup[1].value : ValueGroup[0].value;
                #endregion
            }
            else
            {
                if (highestQuantity == 3 && valueGroupByCount[1].count > 1)
                {
                    #region Full house
                    first = 6;
                    second = highestQuantityValue;
                    
                    if(valueGroupByCount.Count > 2 && valueGroupByCount[2].count > 1)
                        third = Math.Max(valueGroupByCount[1].value, valueGroupByCount[2].value);
                    else
                        third = valueGroupByCount[1].value;

                    #endregion
                }
                else
                {
                    #region Check for straight
                    int i = 0;
                    while (i + 4 < ValueGroup.Count)
                    {
                        if (ValueGroup[i].value == ValueGroup[i + 4].value + 4)
                        {
                            first = 4;
                            second = ValueGroup[i].value;
                            return;
                        }
                        i++;
                    }
                    #endregion

                    if (highestQuantity == 3)
                    {
                        #region 3 of a kind
                        first = 3;
                        second = highestQuantityValue;

                        if (ValueGroup[0].value == second)
                            third = ValueGroup[1].value;
                        else
                            third = ValueGroup[0].value;

                        if (ValueGroup[1].value == second || ValueGroup[1].value == third)
                            fourth = ValueGroup[2].value;
                        else
                            fourth = ValueGroup[1].value;
                        #endregion
                    }
                    else if (highestQuantity == 2)
                    {
                        if (valueGroupByCount[1].count > 1)
                        {
                            #region Two pairs

                            first = 2;
                            second = highestQuantityValue;

                            if (valueGroupByCount[2].count > 1)
                                third = Math.Max(valueGroupByCount[1].value, valueGroupByCount[2].value);
                            else
                                third = valueGroupByCount[1].value;

                            fourth = ValueGroup.First(vc => vc.value != second && vc.value != third).value;

                            #endregion
                        }
                        else
                        {
                            #region One Pair
                            first = 1;
                            second = highestQuantityValue;
                            third = ValueGroup[1].value;
                            fourth = ValueGroup[2].value;
                            fift = ValueGroup[3].value;
                            #endregion
                        }
                    }
                    else
                    {
                        #region Highcard
                        first = 0;
                        second = highestQuantityValue;
                        third = ValueGroup[1].value;
                        fourth = ValueGroup[2].value;
                        fift = ValueGroup[3].value;
                        sixth = ValueGroup[4].value;
                        #endregion
                    }
                }
            }
        }

#region Unused code
            //var ValueGroup = hand.GroupBy(c => c.Value).Select(g => new { value = g.Key, count = g.Count() }).OrderBy(vc => vc.count).ThenBy(vc => vc.value).ToList();
            //switch (ValueGroup[0].count)
            //{
            //    case 4:
            //        break;
            //}
            

            //List<int> valueCount = new List<int>(13){0,0,0,0,0,0,0,0,0,0,0,0,0};
            //List<int> suitCount = new List<int>(4){0,0,0,0};

            //foreach (Card c in hand)
            //{
            //    suitCount[(int)c.Suit]++;
            //    valueCount[c.Value]++;
            //}
            //bool flush = suitCount.Max() >= 5;
            //int straightCount = 0;
            //int highest = -1;
            //bool straight = false;
            //for (int i = 12; i >= 0; i--)
            //{
            //    if (valueCount[i] > 0)
            //    {
            //        straightCount++;
            //        if (straightCount == 5)
            //        {
            //            straight = true;
            //            highest = i+4;
            //            break;
            //        }
            //    }
            //    else straightCount = 0;
            //}
            //if (straightCount >= 4 && valueCount[12] > 0)
            //{
            //    straight = true;
            //    highest = 12;
            //}
            
            //if (flush & straight)
            //{
            //    List<Card> temp = hand.Where(c => ((int) c.Suit) == suitCount.IndexOf(suitCount.Max())).OrderByDescending(c => c.Value).ToList();
            //    int i = 0;
            //    while (i + 4 < temp.Count)
            //    {
            //        if (temp[i].Value == temp[i + 4].Value + 4)
            //        {
            //            first = 8;
            //            second = temp[i].Value;
            //            return;
            //        }
            //        i++;
            //    }
            //}

            //int highestValueQuantity = ValueGroup.Max();

            //switch (highestValueQuantity)
            //{
            //    case 4:
            //        first = 7;
            //        second = ValueGroup.IndexOf(4);
            //        third = Math.Max(Math.Max(ValueGroup.IndexOf(3), ValueGroup.IndexOf(2)), ValueGroup.IndexOf(1));
            //        break;
            //    case 3:
            //        first = 6;
            //        second = ValueGroup.LastIndexOf(3);
            //        third = Math.Max(ValueGroup.LastIndexOf(second - 1, 3), ValueGroup.LastIndexOf(2));
            //        if (third == -1)
            //        {
            //            first = 3;
            //            third = ValueGroup.LastIndexOf(1);
            //            fourth = ValueGroup.FindLastIndex(third - 1, v => v == 1);
            //        }
            //        break;
            //    case 2:
            //        if (!straight && !flush)
            //        {
            //            second = ValueGroup.LastIndexOf(2);
            //            third = ValueGroup.FindLastIndex(second - 1, v => v == 2);
            //            if (third != -1)
            //            {
            //                first = 2;
            //                fourth = ValueGroup.LastIndexOf(1);
            //            }
            //            else
            //            {
            //                first = 1;
            //                third = ValueGroup.LastIndexOf(1);
            //                fourth = ValueGroup.FindLastIndex(third - 1, v => v == 1);
            //                fift = ValueGroup.FindLastIndex(fourth - 1, v => v == 1);
            //            }
            //        }
            //        break;
            //    case 1:
            //        if (!straight && !flush)
            //        {
            //            first = 0;
            //            second = ValueGroup.LastIndexOf(1);
            //            third = ValueGroup.FindLastIndex(second - 1, v => v == 0);
            //            fourth = ValueGroup.FindLastIndex(third - 1, v => v == 0);
            //            fift = ValueGroup.FindLastIndex(fourth - 1, v => v == 0);
            //            sixth = ValueGroup.FindLastIndex(fift - 1, v => v == 0);
            //        }
            //        break;
            //}
        //}

        //private straight(var )
        //{
        //    while (i + 4 < ValueGroup.Count)
        //        {
        //            if (ValueGroup[i].value == ValueGroup[i + 4].value + 4)
        //            {
        //                first = 4;
        //                second = ValueGroup[i].Value;
        //                return;
        //            }
        //            i++;
        //        }
        //}
#endregion

        private bool flushORstraightFlush(List<Card> hand){
            var flushGroup = hand.GroupBy(c => c.Suit).FirstOrDefault(g => g.Count() >= 5);
            bool flush = flushGroup != null;
            if (flush)
            {
                var flushCards = flushGroup.ToList();
                int i = 0;
                while (i + 4 < flushCards.Count)
                {
                    if (flushCards[i].Value == flushCards[i + 4].Value + 4)
                    {
                        first = 8;
                        second = flushCards[i].Value;
                        return true;
                    }
                    i++;
                }
                first = 5;
                second = flushCards[0].Value;
                third = flushCards[1].Value;
                fourth = flushCards[2].Value;
                fift = flushCards[3].Value;
                sixth = flushCards[4].Value;
                return true;
            }
            return false;
        }

        public int betterThan(PowerRating other)
        {
            if (this.first != other.first) return this.first > other.first ? 1 : -1;
            else if (this.second != other.second) return this.second > other.second ? 1 : -1;
            else if (this.third != other.third) return this.third > other.third ? 1 : -1;
            else if (this.fourth != other.fourth) return this.fourth > other.fourth ? 1 : -1;
            else if (this.fift != other.fift) return this.fift > other.fift ? 1 : -1;
            else if (this.sixth != other.sixth) return this.sixth > other.sixth ? 1 : -1;
            else return 0;
        }
    }
}