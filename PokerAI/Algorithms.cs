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
            bool flush = true;
            Suit suit = hand[0].Suit;

            List<int> valueCount = new List<int>(13)
            {
                0,0,0,0,0,0,0,0,0,0,0,0,0
            };
            foreach (Card c in hand)
            {
                if (c.Suit != suit) flush = false;
                valueCount[c.Value]++;
            }

            int best = valueCount.Max();
            switch (best)
            {
                case 4:
                    first = 7;
                    second = valueCount.IndexOf(4);
                    third = valueCount.IndexOf(1);
                    break;
                case 3:
                    second = valueCount.IndexOf(3);
                    int nextBest = valueCount.IndexOf(2);
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
    }
    
    class Algorithms
    {
        public static void PreflopPowerRating(List<Card> hand)
        {

        }

        public static void PowerRating(List<Card> hand, List<Card> communityCards)
        {
            
        }
    }
}
