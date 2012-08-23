using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerAI
{
    class Player
    {
        public Table Table;
        
        public bool Folded;
        public List<Card> Hand;
        public int CurrentBet;
        public int Stack;

        public Player(Table table)
        {
            Table = table;
        }
    }
}
