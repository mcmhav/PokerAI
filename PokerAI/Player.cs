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
        public bool Checked;
        public List<Card> Hand;
        public int MyCurrentBet { get; private set; }
        public int Stack;

        public Player(Table table)
        {
            Table = table;
            Stack = initMoney;
            Hand = new List<Card>();
        }

        public bool CanPlay()
        {
            return Stack != 0 && !Folded && (!Checked);
        }

		public int EvaluateHand(int round)
		{
			if(0 == round)
            {
                
            }
            return 0;
		}

        public Action Action(int currentBet)
        {
            
            //Table.getCommunityCards();
            List<Card> temp = new List<Card>();
            temp.AddRange(Hand);
            temp.AddRange(Table.getCommunityCards());
            PowerRating pr = new PowerRating(temp);
            //if(pr.first > 1) pr.first = 3;
            
            if(true) //currentBet > 0 for later
            {
                int callAmount = currentBet - MyCurrentBet;
                if (currentBet > Stack)
                {
                    Action a = new Action(ActionType.ALLIN, Stack, 0);
                    Stack = 0;
                    return a;
                }
                else if(pr.first == 8)
                {
                    Action a = new Action(ActionType.ALLIN, callAmount, Stack-callAmount);
                    Stack = 0;
                    return a;
                }
                else if(pr.first > 2)
                {
                    Action a = new Action(ActionType.BET, callAmount, pr.first*(Stack/100));
                    Stack = pr.first*(Stack/100);
                    return a;
                }
                else
                {
                    return new Action(ActionType.FOLD, 0, 0);
                }
            }
            else if (currentBet == MyCurrentBet && Checked)
            {
                //something
            }
        }

        private bool canBet(int currentBet)
        {

            return false;
        }


        public void ActionMade(Action action, Player player)
        {

        }

        //public int ActionSelector()
        //{

        //}

        //public int PlaceBet()
        //{
            
        //}

        //public int OpponentModeler()
        //{

        //}
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

    enum ActionType {FOLD, CHECK, CALL, BET, RAISE, RERAISE, ALLIN}
}
