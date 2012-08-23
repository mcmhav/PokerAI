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
        public Suit suit;
        public int Value;
    }
}
