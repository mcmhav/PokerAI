using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerAI
{
    enum Suit
    {
        SPADES, HEARTS, CLOVES, DIAMONDS
    }
    
    class Card
    {
        public Suit Suit;
        public int Value;

        public Card(Suit suit, int value)
        {
            Suit = suit;
            Value = value;
        }
    }

    class Deck
    {
        Stack<Card> cards;

        public Deck()
        {
            cards = new Stack<Card>();
            Shuffle();
        }

        public void Shuffle()
        {
            List<Card> temp = new List<Card>();
            for (int i = 1; i < 14; i++)
            {
                temp.Add(new Card(Suit.SPADES, i));
                temp.Add(new Card(Suit.HEARTS, i));
                temp.Add(new Card(Suit.CLOVES, i));
                temp.Add(new Card(Suit.DIAMONDS, i));
            }

            Random random = new Random();
            int randomNumber = 0;
            while (temp.Count > 0)
            {
                randomNumber = random.Next(temp.Count);
                cards.Push(temp[randomNumber]);
                temp.RemoveAt(randomNumber);
            }
        }

        public Card DrawCard()
        {
            return cards.Pop();
        }
    }
}
