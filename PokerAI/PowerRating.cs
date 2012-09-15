using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerAI
{
    class PowerRating
    {
        int first = -1;
        int second = -1;
        int third = -1;
        int fourth = -1;
        int fift = -1;
        int sixth = -1;

        public PowerRating(List<Card> hand)
        {

            List<int> valueCount = new List<int>(13){0,0,0,0,0,0,0,0,0,0,0,0,0};
            List<int> suitCount = new List<int>(4){0,0,0,0};

            foreach (Card c in hand)
            {
                suitCount[(int)c.Suit]++;
                valueCount[Math.Abs(c.Value-12)]++;
            }
            bool flush = suitCount.Max() >= 5;
            int straightCount = 0;
            int highest;
            bool straight = false;
            for (int i = 12; i >= 0; i--)
            {
                if (valueCount[i] > 0)
                {
                    straightCount++;
                    if (straightCount == 5)
                    {
                        straight = true;
                        highest = i+4;
                        break;
                    }
                }
                else straightCount = 0;
            }
            if (straightCount >= 4 && valueCount[12] > 0)
            {
                straight = true;
                highest = 12;
            }
            
            if (flush & straight)
            {
                // TODO Check for straight flush.
                first = 8;
                second = highest;
                return;
            }

            int highestValueQuantity = valueCount.Max();
            
            switch (highestValueQuantity)
            {
                case 4:
                    first = 7;
                    second = Math.Abs(valueCount.IndexOf(4) - 12);
                    third = Math.Abs(Math.Min(Math.Min(valueCount.IndexOf(3), valueCount.IndexOf(2)), valueCount.IndexOf(1)) - 12);
                    break;
                case 3:
                    second = Math.Abs(valueCount.IndexOf(3) - 12);
                    int Math.Abs(Math.Min(nextBest = valueCount.IndexOf(2);
                    if (nextBest != -1)
                    {
                        first = 6;
                        third = nextBest;
                    }
                    else
                    {
                        first = 3;
                        for(int i = 12; i >= 0; i--)
                        {
                            if (valueCount[i] == 1)
                            {
                                if(second == -1) second = i;
                                else third = i;
                            }
                        }
                    }
                    break;
                case 2:
                    first = 1;
                    for(int i = 12; i >= 0; i--)
                    {
                        if(valueCount[i] == 2)
                        {
                            if(second == -1) second = i;
                            else
                            {
                                first = 2;
                                third = i;
                            }
                        }
                        if(valueCount[i] == 1)
                        {
                            if(fourth == -1) fourth = i;
                            else if(fift == -1) fift = i;
                            else
                            {
                                third = fourth;
                                fourth = fift;
                                fift = i;
                            }
                        }
                    }
                    break;
                case 1:
                    bool straight = false;
                    int straightCount = 0;
                    for (int i = 12; i >= 0; i--)
                    {
                        if (valueCount[i] == 1)
                        {
                            straightCount++;
                            if (straightCount == 5)
                            {
                                i = -1;
                                straight = true;
                            }

                            if (second != -1) second = i;
                            else if (third != -1) third = i;
                            else if (fourth != -1) fourth = i;
                            else if (fift != -1) fift = i;
                            else sixth = i;
                        }
                        else straightCount = 0;
                    }
                    if (straight || (straightCount == 4 && valueCount[12] == 1))
                    {
                        second = first;
                        third = fourth = fift = sixth = 0;
                        first = flush ? 8 : 4;
                    }
                    else first = flush? 5 : 0;

                    break;
            }
        }

        public static int betterThan(PowerRating pr1, PowerRating powerRating2)
        {
            if (pr1.first != powerRating2.first) return pr1.first > powerRating2.first ? 1 : -1;
            else if (pr1.second != powerRating2.second) return pr1.second > powerRating2.second ? 1 : -1;
            else if (pr1.third != powerRating2.third) return pr1.third > powerRating2.third ? 1 : -1;
            else if (pr1.fourth != powerRating2.fourth) return pr1.fourth > powerRating2.fourth ? 1 : -1;
            else if (pr1.fift != powerRating2.fift) return pr1.fift > powerRating2.fift ? 1 : -1;
            else if (pr1.sixth != powerRating2.sixth) return pr1.sixth > powerRating2.sixth ? 1 : -1;
            else return 0;
        }

        public static int betterThan(List<Card> hand)
        {

        }
    }
}
