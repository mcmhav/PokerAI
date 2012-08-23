using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerAI
{
    class Player
    {
        private readonly int initMoney = 1000;
        public Table Table;
        
        public bool Folded;
        public bool checked
        public List<Card> Hand;
        public int CurrentBet;
        public int Stack;

        public Player(Table table)
        {
            Table = table;
            Stack = initMoney;
        }

        public bool canPlay()
        {
            return true;
        }

		public int evaluateHand(int round)
		{
			if(0 == round)
            {
                
            }
		}

        public int actionSelector()
        {

        }

        public int placeBet()
        {
            
        }

        public int opponentModeler()
        {

        }
    }
}
