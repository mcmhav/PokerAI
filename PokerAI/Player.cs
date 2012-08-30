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
        public bool checkeds
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

        public Move DoTurn()
        {

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

    class Action
    {
        public ActionType type;
        public int callAmount;
        public int betAmount;

        public Action(ActionType type, int callAmount, int betAmount){
            this.type = type;
            this.callAmount = callAmount;
            this.betAmount = betAmount;

        }
    }

    enum ActionType {FOLD, CHECK, CALL, BET, RAISE, RERAISE}
}
